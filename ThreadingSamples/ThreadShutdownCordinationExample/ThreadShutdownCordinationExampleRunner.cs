using System.Threading;

namespace ThreadingSamples.ThreadExample
{
    public static class ThreadShutdownCordinationExampleRunner
    {
        public volatile static bool Cancel = false;

        public static void Run()
        {
            
            var threadExample = new ThreadShutdownCordinationExample(1, "title 1", "description 1");

            Thread thread = new Thread(threadExample.Compute);
            thread.Start();

            Thread.Sleep(1000);

            Cancel = true;
            thread.Join();
        }
    }
}
