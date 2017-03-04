using System.Threading;

namespace ThreadingSamples.ThreadExample
{
    public static class LockExampleRunner
    {
        public static void Run()
        {
            var lockExample = new LockExample();

            for (int i = 0; i < 10; i++)
            {            
                Thread thread = new Thread(lockExample.ComputeEnqueing);
                thread.Start();
            }

            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(lockExample.ComputeDequeing);
                thread.Start();
            }
        }
    }
}
