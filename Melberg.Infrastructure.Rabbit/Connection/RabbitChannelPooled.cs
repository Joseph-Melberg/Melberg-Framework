using System.Threading;
using System.Threading.Tasks;
using Melberg.Infrastructure.Rabbit.Consumers;

namespace Melberg.Infrastructure.Rabbit.Connection
{
    public class RabbitChannelPooled : RabbitChannel
    {
        private readonly RabbitConnectionPooledChannels _owner;

        public RabbitChannelPooled(IModel rabbitChannel, IConnection rabbitConnection, RabbitConnectionPooledChannels owner)
            : base(rabbitChannel, rabbitConnection)
        {
            _owner = owner;
        }

        public override Task ConsumeAsync(
            string queue,
            ushort prefetch,
            IMessageConsumer handler,
            CancellationToken cancellationToken,
            int checkCancellationInterval,
            bool noAck)
        {
            throw new System.Exception("Channel Pool should not be set for a Rabbit connection used for consuming messages.");
        }

        protected override void Dispose(bool disposing)
        {
            if (_owner == null)
            {
                base.Dispose(disposing);
                return;
            }

            ReturnChannel();
        }

        private void ReturnChannel()
        {
            _owner.Return(this);
        }

        internal void DisposeChannel()
        {
            base.Dispose(true);
        }
    }
}