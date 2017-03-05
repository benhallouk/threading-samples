using System;
using ThreadingSamples.ThreadExample;

namespace ThreadingSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Main started");

            // Uncomment to run the simple thread getting started example
            //ThreadExampleRunner.Run();

            // Uncomment to see the behavior of background thread
            //ThreadLifeTimeExampleRunner.Run();

            // Uncomment to see how a shutdown cordination would work 
            //ThreadShutdownCordinationExampleRunner.Run();

            // Uncomment to see a quueing system using lock and Monitor Wait and PulseAll
            //LockExampleRunner.Run();

            // uncomment to see how deadlock happen
            //DeadLockExampleRunner.Run();

            // uncomment to see deadlock solution using lock order policy
            //DeadLockSolutionRunner.Run();

            DeadLockSolutionUsingMutexRunner.Run();

            Console.WriteLine("Main ends");
        }
    }
}