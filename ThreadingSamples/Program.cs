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
            ThreadLifeTimeExampleRunner.Run();

            Console.WriteLine("Main ends");
        }
    }
}
