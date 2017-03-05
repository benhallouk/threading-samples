using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadingSamples.ThreadExample
{
    public class DeadLockSolutionUsingMutex
    {
        private int _number;
        Mutex mutex = new Mutex();

        public DeadLockSolutionUsingMutex(int number)
        {
            _number = number;
        }


        public int GetNumber()
        {
            return _number;
        }

        public void Compute(DeadLockSolutionUsingMutex deadLockSolution)
        {
            if (mutex.WaitOne())
            {
                try
                {
                    Thread.Sleep(1);
                    Console.WriteLine("Thread number started {0}", Thread.CurrentThread.ManagedThreadId);
                    _number = deadLockSolution.GetNumber();
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }            
        }

    }
}
