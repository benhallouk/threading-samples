using System;
using System.Threading;

namespace ThreadingSamples.ThreadExample
{
    public class ThreadShutdownCordinationExample
    {
        private readonly int _number;
        private readonly string _title;
        private readonly string _description;

        public ThreadShutdownCordinationExample(int number, string title, string description)
        {
            _number = number;
            _title = title;
            _description = description;
        }
        public void Compute()
        {
            while (!ThreadShutdownCordinationExampleRunner.Cancel)
            {
                Console.WriteLine("Computting...");
                Thread.Sleep(2000);                
            }
        }
    }
}
