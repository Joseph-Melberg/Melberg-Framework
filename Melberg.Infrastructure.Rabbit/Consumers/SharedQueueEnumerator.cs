using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Melberg.Infrastructure.Rabbit.Consumers
{
    public struct SharedQueueEnumerator<T> : IEnumerator<T>
    {
        private readonly SharedQueue<T> _queue;
        private T _current;

       
        public SharedQueueEnumerator(SharedQueue<T> queue)
        {
            _queue = queue;
            _current = default(T);
        }

        object IEnumerator.Current
        {
            get
            {
                if (_current == null)
                {
                    throw new InvalidOperationException();
                }
                return _current;
            }
        }

        T IEnumerator<T>.Current
        {
            get
            {
                if (_current == null)
                {
                    throw new InvalidOperationException();
                }
                return _current;
            }
        }

        public void Dispose()
        {
            //this makes no sense in this context
        }

        bool IEnumerator.MoveNext()
        {
            try
            {
                _current = _queue.Dequeue();
                return true;
            }
            catch (EndOfStreamException)
            {
                _current = default(T);
                return false;
            }
        }

        /// <inheritdoc />
        /// <summary>Reset()ting a SharedQueue doesn't make sense, so
        /// this method always throws
        /// InvalidOperationException.</summary>
        void IEnumerator.Reset()
        {
            throw new InvalidOperationException("SharedQueue.Reset() does not make sense");
        }
    }
}