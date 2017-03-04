using System;
using ThreadingSamples.ThreadExample;

namespace ThreadingSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Main started");

            ThreadExampleRunner.Run();

            Console.WriteLine("Main ends");
            Console.ReadLine();
        }
    }
}
