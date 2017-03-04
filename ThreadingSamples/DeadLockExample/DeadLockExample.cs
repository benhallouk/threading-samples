using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadingSamples.ThreadExample
{
    public class DeadLockExample
    {
        private int _number;
        public object _lockObject = new object();

        public DeadLockExample(int number)
        {
            _number = number;
        }


        public int GetNumber()
        {
            return _number;
        }

        public void Compute(DeadLockExample deadLockExample)
        {
            lock (_lockObject)
            {
                Thread.Sleep(1);
                lock (deadLockExample._lockObject)
                {
                    Console.WriteLine("Thread number started {0}", Thread.CurrentThread.ManagedThreadId);
                    _number = deadLockExample.GetNumber();
                }
            }
        }

    }
}
