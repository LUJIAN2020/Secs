using System;
using System.Threading;

namespace Secs.Extensions
{
    internal class SystemBytesHelper
    {
        private static int systemBytes = 0;
        private static readonly ThreadLocal<Random> _random = new(() => new Random(Guid.NewGuid().GetHashCode()));
        public static int IncrementSystemBytes()
        {
            return Interlocked.Increment(ref systemBytes);
        }
        public static int RandomSystemBytes()
        {
            return _random.Value.Next(int.MinValue, int.MaxValue);
        }
    }
}
