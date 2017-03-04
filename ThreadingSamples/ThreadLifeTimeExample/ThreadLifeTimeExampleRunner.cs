using System.Threading;

namespace ThreadingSamples.ThreadExample
{
    public static class ThreadLifeTimeExampleRunner
    {
        public static void Run()
        {
            for (int i = 1; i < 11; i++)
            {
                var threadExample = new ThreadLifeTimeExample(i, string.Format("title {0}",i), string.Format("description",i));

                Thread thread = new Thread(threadExample.Compute);
                thread.IsBackground = true;
                thread.Start();
            }
        }
    }
}
