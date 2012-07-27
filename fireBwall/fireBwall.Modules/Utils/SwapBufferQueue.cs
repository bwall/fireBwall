using System.Collections.Generic;
using System.Threading;
using fireBwall.Logging;
using System;

namespace fireBwall.Utils
{
    /// <summary>
    /// The point of this is to make a queue that can take input from multiple threads at the same time while a single thread is dumping the information in the queue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SwapBufferQueue<T>
    {
        private volatile Queue<T> bufferA = new Queue<T>();
        private volatile Queue<T> bufferB = new Queue<T>();

        //if false, A is being filled
        private bool swap = false;
        private readonly object dumpLock = new object();
        private readonly object addLock = new object();
        private volatile ReaderWriterLock swapLock = new ReaderWriterLock();

        /// <summary>
        /// Adds an object to the current queue for filling
        /// </summary>
        /// <param name="t">The object to be added</param>
        public void Enqueue(T t)
        {
            try
            {
                swapLock.AcquireReaderLock(new TimeSpan(0, 1, 0));
                try
                {
                    lock (addLock)
                    {
                        if (swap)
                        {
                            bufferB.Enqueue(t);
                        }
                        else
                        {
                            bufferA.Enqueue(t);
                        }
                    }
                }
                finally
                {
                    swapLock.ReleaseReaderLock();
                }
            }
            catch (ApplicationException ex)
            {
                LogCenter.Instance.LogException(ex);
            }
        }

        /// <summary>
        /// Swaps the current buffer states, and empties the queue and returns a copy
        /// </summary>
        /// <returns>A copy of the queue</returns>
        public Queue<T> DumpBuffer()
        {
            Queue<T> ret = null;
            try
            {
                swapLock.AcquireWriterLock(new TimeSpan(0, 1, 0));
                try
                {
                    swap = !swap;
                }
                finally
                {
                    swapLock.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                LogCenter.Instance.LogException(ex);
            }
            try
            {
                swapLock.AcquireReaderLock(new TimeSpan(0, 1, 0));
                try
                {
                    lock (dumpLock)
                    {
                        if (swap)
                        {
                            ret = new Queue<T>(bufferA);
                            bufferA.Clear();
                        }
                        else
                        {
                            ret = new Queue<T>(bufferB);
                            bufferB.Clear();
                        }
                    }
                }
                finally
                {
                    swapLock.ReleaseReaderLock();
                }
            }
            catch (ApplicationException ex)
            {
                LogCenter.Instance.LogException(ex);
            }
            return ret;
        }
    }
}
