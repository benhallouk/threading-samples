using System.Threading;

namespace ThreadingSamples.ThreadExample
{
    public static class ThreadExampleRunner
    {
        public static void Run()
        {
            for (int i = 1; i < 11; i++)
            {
                var threadExample = new ThreadExample(i, string.Format("title {0}",i), string.Format("description",i));

                Thread thread = new Thread(threadExample.Compute);
                thread.Start();
            }
        }
    }
}
