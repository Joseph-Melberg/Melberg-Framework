using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Melberg.Infrastructure.Rabbit.Consumers
{
    public class SharedQueue : SharedQueue<object>
    {
    }

    /// <summary>
    /// A thread-safe shared queue implementation.
    /// </summary>
    public class SharedQueue<T> : IEnumerable<T>
    {
        ///<summary>Flag holding our current status.</summary>
        protected bool IsOpen = true;

        ///<summary>The shared queue.</summary>
        ///<remarks>
        ///Subclasses must ensure appropriate locking discipline when
        ///accessing this field. See the implementation of Enqueue,
        ///Dequeue.
        ///</remarks>
        protected Queue<T> MainQueue = new Queue<T>();


        protected Queue<TaskCompletionSource<T>> WaitingQueue = new Queue<TaskCompletionSource<T>>();


        ///<summary>Close the queue. Causes all further Enqueue()
        ///operations to throw EndOfStreamException, and all pending
        ///or subsequent Dequeue() operations to throw an
        ///EndOfStreamException once the queue is empty.</summary>
        public void Close()
        {
            lock (MainQueue)
            {
                IsOpen = false;
                Monitor.PulseAll(MainQueue);

#if NETFX_CORE

                // let all waiting tasks know we just closed by passing them an exception
                if (WaitingQueue.Count <= 0) return;

                try 
                {
                    EnsureIsOpen();
                }
                catch (Exception ex) 
                {
                    foreach (var tcs in WaitingQueue) 
                    {
                        tcs.TrySetException(ex);
                    }
                }
#endif
            }
        }

        ///<summary>Retrieve the first item from the queue, or block if none available</summary>
        ///<remarks>
        ///Callers of Dequeue() will block if no items are available
        ///until some other thread calls Enqueue() or the queue is
        ///closed. In the latter case this method will throw
        ///EndOfStreamException.
        ///</remarks>
        public T Dequeue()
        {
            lock (MainQueue)
            {
                while (MainQueue.Count == 0)
                {
                    EnsureIsOpen();
                    Monitor.Wait(MainQueue);
                }
                return MainQueue.Dequeue();
            }
        }
        
        /// <summary>
        /// Asynchronously retrieves the first item from the queue.
        /// </summary>
        public Task<T> DequeueAsync() 
        {
            lock (MainQueue) 
            {
                EnsureIsOpen();

                if (MainQueue.Count > 0)
                {
                    return Task.FromResult(this.Dequeue());
                }

                var tcs = new TaskCompletionSource<T>();
                WaitingQueue.Enqueue(tcs);
                return tcs.Task;
            }
        }

        ///<summary>Retrieve the first item from the queue, or return
        ///nothing if no items are available after the given
        ///timeout</summary>
        ///<remarks>
        ///<para>
        /// If one or more items are present on the queue at the time
        /// the call is made, the call will return
        /// immediately. Otherwise, the calling thread blocks until
        /// either an item appears on the queue, or
        /// millisecondsTimeout milliseconds have elapsed.
        ///</para>
        ///<para>
        /// Returns true in the case that an item was available before
        /// the timeout, in which case the out parameter "result" is
        /// set to the item itself.
        ///</para>
        ///<para>
        /// If no items were available before the timeout, returns
        /// false, and sets "result" to null.
        ///</para>
        ///<para>
        /// A timeout of -1 (i.e. System.Threading.Timeout.Infinite)
        /// will be interpreted as a command to wait for an
        /// indefinitely long period of time for an item to become
        /// available. Usage of such a timeout is equivalent to
        /// calling Dequeue() with no arguments. See also the MSDN
        /// documentation for
        /// System.Threading.Monitor.Wait(object,int).
        ///</para>
        ///<para>
        /// If no items are present and the queue is in a closed
        /// state, or if at any time while waiting the queue
        /// transitions to a closed state (by a call to Close()), this
        /// method will throw EndOfStreamException.
        ///</para>
        ///</remarks>
        public bool Dequeue(int millisecondsTimeout, out T result)
        {
            if (millisecondsTimeout == Timeout.Infinite)
            {
                result = Dequeue();
                return true;
            }

            var startTime = DateTime.Now;

            lock (MainQueue)
            {
                while (MainQueue.Count == 0)
                {
                    EnsureIsOpen();
                    var elapsedTime = (int)((DateTime.Now - startTime).TotalMilliseconds);

                    var remainingTime = millisecondsTimeout - elapsedTime;
                    
                    if (remainingTime <= 0)
                    {
                        result = default(T);
                        return false;
                    }

                    Monitor.Wait(MainQueue, remainingTime);
                }

                result = MainQueue.Dequeue();
                return true;
            }
        }

        ///<summary>Retrieve the first item from the queue, or return
        ///defaultValue immediately if no items are
        ///available</summary>
        ///<remarks>
        ///<para>
        /// If one or more objects are present in the queue at the
        /// time of the call, the first item is removed from the queue
        /// and returned. Otherwise, the defaultValue that was passed
        /// in is returned immediately. This defaultValue may be null,
        /// or in cases where null is part of the range of the queue,
        /// may be some other sentinel object. The difference between
        /// DequeueNoWait() and Dequeue() is that DequeueNoWait() will
        /// not block when no items are available in the queue,
        /// whereas Dequeue() will.
        ///</para>
        ///<para>
        /// If at the time of call the queue is empty and in a
        /// closed state (following a call to Close()), then this
        /// method will throw EndOfStreamException.
        ///</para>
        ///</remarks>
        public T DequeueNoWait(T defaultValue)
        {
            lock (MainQueue)
            {
                if (MainQueue.Count == 0)
                {
                    EnsureIsOpen();
                    return defaultValue;
                }
                    
                return MainQueue.Dequeue();
                
            }
        }

        ///<summary>Place an item at the end of the queue.</summary>
        ///<remarks>
        ///If there is a thread waiting for an item to arrive, the
        ///waiting thread will be woken, and the newly Enqueued item
        ///will be passed to it. If the queue is closed on entry to
        ///this method, EndOfStreamException will be thrown.
        ///</remarks>
        public void Enqueue(T o)
        {
            lock (MainQueue)
            {
                EnsureIsOpen();
#if NETFX_CORE

                while (WaitingQueue.Count > 0)
                {
                    var tcs = WaitingQueue.Dequeue();
                    if (tcs != null && tcs.TrySetResult(o)) 
                    {
                        // We successfully set a task return result, so
                        // no need to Enqueue or Monitor.Pulse
                        return;
                    }
                }
#endif

                MainQueue.Enqueue(o);
                Monitor.Pulse(MainQueue);

            }
        }

        ///<summary>Implementation of the IEnumerable interface, for
        ///permitting SharedQueue to be used in foreach
        ///loops.</summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SharedQueueEnumerator<T>(this);
        }

        ///<summary>Implementation of the IEnumerable interface, for
        ///permitting SharedQueue to be used in foreach
        ///loops.</summary>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new SharedQueueEnumerator<T>(this);
        }

        ///<summary>Call only when the lock on MainQueue is held.</summary>
        /// <exception cref="EndOfStreamException" />
        private void EnsureIsOpen()
        {
            if (!IsOpen)
            {
                throw new EndOfStreamException("SharedQueue closed");
            }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}