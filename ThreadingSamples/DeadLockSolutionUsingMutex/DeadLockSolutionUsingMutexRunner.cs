using System;
using System.Threading;

namespace ThreadingSamples.ThreadExample
{
    public static class DeadLockSolutionUsingMutexRunner
    {
        public static void Run()
        {
            var lockExample1 = new DeadLockSolutionUsingMutex(1);
            var lockExample2 = new DeadLockSolutionUsingMutex(2);

            for (int i = 0; i < 100; i++)
            {            
                Thread thread = new Thread(()=> {
                    if (i % 2 == 0)
                    {
                        lockExample1.Compute(lockExample2);
                    }
                    else
                    {
                        lockExample2.Compute(lockExample1);
                    }     
                });
                thread.Start();
            }

            Console.WriteLine("lockExample1 {0}", lockExample1.GetNumber());
            Console.WriteLine("lockExample2 {0}", lockExample2.GetNumber());
        }
    }
}
