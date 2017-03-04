using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadingSamples.ThreadExample
{
    public class DeadLockSolution
    {
        private int _number;
        public int Id { get; private set; }
        public object _lockObject = new object();

        public DeadLockSolution(int id, int number)
        {
            Id = id;
            _number = number;
        }


        public int GetNumber()
        {
            return _number;
        }

        public void Compute(DeadLockSolution deadLockSolution)
        {
            var firstLock = _lockObject;
            var secondLock = deadLockSolution._lockObject;

            if (Id > deadLockSolution.Id)
            {
                firstLock = deadLockSolution._lockObject;
                secondLock = _lockObject;
            }
             
            lock (firstLock)
            {
                Thread.Sleep(1);
                lock (secondLock)
                {
                    Console.WriteLine("Thread number started {0}", Thread.CurrentThread.ManagedThreadId);
                    _number = deadLockSolution.GetNumber();
                }
            }
        }

    }
}
