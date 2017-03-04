using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadingSamples.ThreadExample
{
    public class LockExample
    {
        private Queue<string> _objectTray = new Queue<string>();

        public void ComputeDequeing()
        {
            Console.WriteLine("Thread number started {0}",Thread.CurrentThread.ManagedThreadId);

            lock (_objectTray)
            {
                while (_objectTray.Count == 0)
                {
                    Monitor.Wait(_objectTray);
                }

                var obj = _objectTray.Dequeue();
                Console.WriteLine("Processing {0} ...", obj);
            }
        }

        public void ComputeEnqueing()
        {
            Console.WriteLine("Thread number started {0}", Thread.CurrentThread.ManagedThreadId);

            lock (_objectTray)
            {
                for (int i = 0; i < 5; i++)
                {
                    _objectTray.Enqueue(string.Format("Item {0} from thread {1}",i, Thread.CurrentThread.ManagedThreadId));
                }
                Monitor.PulseAll(_objectTray);
            }
        }

    }
}
