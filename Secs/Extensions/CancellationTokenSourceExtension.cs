using System.Threading;

namespace Secs.Extensions
{
    internal static class CancellationTokenSourceExtension
    {
        public static void Destroy(this CancellationTokenSource? tokenSource)
        {
            if (tokenSource != null)
            {
                if (!tokenSource.IsCancellationRequested)
                {
                    tokenSource.Cancel();
                }
                tokenSource.Dispose();
            }
        }
        public static CancellationTokenSource NewTokenSource(this CancellationTokenSource? tokenSource)
        {
            Destroy(tokenSource);
            return new CancellationTokenSource();
        }
        public static CancellationTokenSource NewTokenSource(this CancellationTokenSource? tokenSource, int millisecondsDelay)
        {
            Destroy(tokenSource);
            return new CancellationTokenSource(millisecondsDelay);
        }
    }
}
