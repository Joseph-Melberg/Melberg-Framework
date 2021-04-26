using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace Melberg.Infrastructure.Rabbit.Connection
{
    public class RabbitConnectionPooledChannels : RabbitConnection
    {
        private readonly ConcurrentBag<IChannel> _channelPool = new ConcurrentBag<IChannel>();
        //The maximum number of channels that should be kept in the pool
        private readonly int _maxChannels;
        //The current number of channels in the pool
        private int _currentChannelCount;
        //The flag if the class is being disposed of
        private int _disposing;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rabbitConnection"></param>
        /// <param name="logger"></param>
        /// <param name="maxChannelPoolSize">The maximum number of channels to be kept in the pool. This is not the maximum number
        /// of channels that can be created. </param>
        public RabbitConnectionPooledChannels(RabbitMQ.Client.IConnection rabbitConnection, int maxChannelPoolSize)
            : base(rabbitConnection)
        {
            if (maxChannelPoolSize < 0)
            {
                throw new ArgumentException("Invalid max channel count.");
            }

            _maxChannels = maxChannelPoolSize;
        }

        protected override IChannel CreateChannelCore(RabbitMQ.Client.IConnection connection)
        {
            if (_disposing == 1)
                throw new ObjectDisposedException("RabbitConnectionPooledChannels is disposed");

            IChannel channel;
            if (_channelPool.TryTake(out channel))
            {
                Interlocked.Decrement(ref _currentChannelCount);

                if (IsOpen && !channel.IsClosed)
                {
                    return channel;
                }
                //Channel or connection isn't open clean it up.
                var pooledChannel = channel as RabbitChannelPooled;
                pooledChannel?.DisposeChannel();
            }

            channel = new RabbitChannelPooled(connection.CreateModel(), this,this);

            return channel;
        }

        private const int MaxSpinAttempts = 10;

        internal void Return(RabbitChannelPooled channel)
        {
            var spin = new SpinWait();
            var attemptCount = 0;

            while (true)
            {
                if (!IsOpen || channel.IsClosed)
                {
                    channel.DisposeChannel();
                    return;
                }

                //Dirty read of current channel count
                var currentChannelCount = _currentChannelCount;

                //Ditch the channel if we are over or at the max channel count or disposing
                if (_disposing == 1 || currentChannelCount >= _maxChannels)
                {
                    //dispose channel too many channels already in pool
                    //If the pool is too small this might happen more then we would like
                    channel.DisposeChannel();
                    return;
                }
                //Try to add the channel to the pool
                //If we lost the race into the pool try a few more times before dropping out
                //We want to avoid locking when reading and writing to the pool.
                if (Interlocked
                        .CompareExchange(ref _currentChannelCount, _currentChannelCount + 1, currentChannelCount) == currentChannelCount)
                {
                    //If we are disposing don't add to the pool.
                    if (_disposing == 1)
                    {
                        channel.DisposeChannel();
                        return;
                    }
                    //If we won the race add it to the pool.
                    //The _currentChannelCount was already incremented.
                    _channelPool.Add(channel);
                    return;
                }

                //We don't want to try forever.
                //Spin a few times and see if we can get
                //back into the pool otherwise dump the 
                //channel
                if (++attemptCount >= MaxSpinAttempts)
                {
                    channel.DisposeChannel();
                    return;
                }

                spin.SpinOnce();
            }
        }

        private static void TryDisposeChannel(IChannel channel)
        {
            var pooledChannel = channel as RabbitChannelPooled;
            if (pooledChannel != null)
            {
                pooledChannel.DisposeChannel();
            }
            else
            {
                channel.Dispose();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing
                && Interlocked.CompareExchange(ref _disposing, 1, 0) == 0)
            {
                //Using our best knowledge of the current channel count
                //Remove everything from the pool and dispose of it.
                var currentChannelCount = _currentChannelCount;
                foreach (var _ in Enumerable.Range(1, currentChannelCount))
                {
                    IChannel outChannel;
                    if (_channelPool.TryTake(out outChannel))
                    {
                        TryDisposeChannel(outChannel);
                    }
                    else
                    {
                        break;
                    }
                }
                //If something didn't see the disposal start
                //Try cleaning the rest up.
                if (!_channelPool.IsEmpty)
                {
                    foreach (var item in _channelPool)
                    {
                        TryDisposeChannel(item);
                    }
                }
            }

            base.Dispose(disposing);
        }
    }
}