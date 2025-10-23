using System;
using System.Collections.Generic;
using System.Linq;

namespace Secs.Extensions
{
    internal static class ArrayExtension
    {
        public static T[] Combine<T>(this T[] firstArray, T[] nextArray) where T : struct
        {
            int offset = firstArray.Length;
            Array.Resize(ref firstArray, firstArray.Length + nextArray.Length);
            Array.Copy(nextArray, 0, firstArray, offset, nextArray.Length);
            return firstArray;
        }
        public static T[] Combine<T>(this T[] firstArray, params T[][] nextArray) where T : struct
        {
            if (nextArray.Length == 0) return firstArray;

            int offset = firstArray.Length;
            int newSize = offset + nextArray.Sum(c => c.Length);
            Array.Resize(ref firstArray, newSize);
            for (int i = 0; i < nextArray.Length; i++)
            {
                Array.Copy(nextArray[i], 0, firstArray, offset, nextArray[i].Length);
                offset += nextArray[i].Length;
            }
            return firstArray;
        }
        public static T[] Slice<T>(this T[] array, int start)
            => array.Slice(start, array.Length - start);
        public static T[] Slice<T>(this T[] array, int start, int length)
        {
            if (start < 0 || start > array.Length)
                throw new ArgumentOutOfRangeException(nameof(start), start, "Index error");

            if (length < 0 || length > array.Length - start)
                throw new ArgumentOutOfRangeException(nameof(start), start, "Length error");

            var buffer = new T[length];
            Array.Copy(array, start, buffer, 0, buffer.Length);
            return buffer;
        }
        public static T[] Slice<T>(this IEnumerable<T> collection, int start)
            => collection.Slice(start, collection.Count() - start);
        public static T[] Slice<T>(this IEnumerable<T> collection, int start, int length)
            => collection.ToArray().Slice(start, length);
        public static string ToHexString(this byte[] buffer, string separator = " ")
            => string.Join(separator, buffer.Select(c => c.ToString("X2"))).ToUpper();
        public static T[] DeepClone<T>(this T[] values) where T : struct
        {
            if (values.Length == 0)
                return Array.Empty<T>();

            var dst = new T[values.Length];
            Buffer.BlockCopy(values, 0, dst, 0, values.Length);
            return dst;
        }
    }
}