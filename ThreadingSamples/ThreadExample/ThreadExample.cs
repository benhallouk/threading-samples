using System;
using System.Threading;

namespace ThreadingSamples.ThreadExample
{
    public class ThreadExample
    {
        private readonly int _number;
        private readonly string _title;
        private readonly string _description;

        public ThreadExample(int number, string title, string description)
        {
            _number = number;
            _title = title;
            _description = description;
        }
        public void Compute()
        {
            Console.WriteLine("Thread number started {0}",Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(10);
            Console.WriteLine("Access to instance private data:");
            Console.WriteLine("Number {0}",_number);
            Console.WriteLine("Title {0}",_title);
            Console.WriteLine("Description {0}",_description);
        }
    }
}
