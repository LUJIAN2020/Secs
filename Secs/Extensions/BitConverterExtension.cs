using System;
using System.Linq;

namespace Secs.Extensions
{
    internal static class BitConverterExtension
    {
        public static short ToInt16(this byte[] buffer, bool isLittleEndian = true)
        {
            if (buffer.Length != 2)
                throw new ArgumentOutOfRangeException(nameof(buffer), buffer.Length, "The array length must be 2");

            return BitConverter.ToInt16(isLittleEndian ? buffer.ReverseToNewBytes() : buffer, 0);
        }
        public static short[] ToInt16Array(this byte[] buffer, bool isLittleEndian = true)
        {
            var values = new short[buffer.Length / 2];
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = isLittleEndian
                    ? BitConverter.ToInt16(new byte[]
                    {
                        buffer[i * 2 + 1],
                        buffer[i * 2]
                    }, 0)
                    : BitConverter.ToInt16(new byte[]
                    {
                        buffer[i * 2 + 2],
                        buffer[i * 2 + 3]
                    }, 0);
            }
            return values;
        }
        public static ushort ToUInt16(this byte[] buffer, bool isLittleEndian = true)
        {
            if (buffer.Length != 2)
                throw new ArgumentOutOfRangeException(nameof(buffer), buffer.Length, "The array length must be 2");

            return BitConverter.ToUInt16(isLittleEndian ? buffer.ReverseToNewBytes() : buffer, 0);
        }
        public static ushort[] ToUInt16Array(this byte[] buffer, bool isLittleEndian = true)
        {
            var values = new ushort[buffer.Length / 2];
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = isLittleEndian
                    ? BitConverter.ToUInt16(new byte[]
                    {
                        buffer[i * 2 + 1],
                        buffer[i * 2]
                    }, 0)
                    : BitConverter.ToUInt16(new byte[]
                    {
                        buffer[i * 2 + 2],
                        buffer[i * 2 + 3]
                    }, 0);
            }
            return values;
        }
        public static int ToInt32(this byte[] buffer, bool isLittleEndian = true)
        {
            if (buffer.Length != 4)
                throw new ArgumentOutOfRangeException(nameof(buffer), buffer.Length, "The array length must be 4");

            return BitConverter.ToInt32(isLittleEndian ? buffer.ReverseToNewBytes() : buffer, 0);
        }
        public static int[] ToInt32Array(this byte[] buffer, bool isLittleEndian = true)
        {
            var values = new int[buffer.Length / 4];
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = isLittleEndian
                    ? BitConverter.ToInt32(new byte[]
                    {
                        buffer[i * 4 + 3],
                        buffer[i * 4 + 2],
                        buffer[i * 4 + 1],
                        buffer[i * 4]
                    }, 0)
                    : BitConverter.ToInt32(new byte[]
                    {
                        buffer[i * 4],
                        buffer[i * 4 + 1],
                        buffer[i * 4 + 2],
                        buffer[i * 4 + 3]
                    }, 0);
            }
            return values;
        }
        public static uint ToUInt32(this byte[] buffer, bool isLittleEndian = true)
        {
            if (buffer.Length != 4)
                throw new ArgumentOutOfRangeException(nameof(buffer), buffer.Length, "The array length must be 4");

            return BitConverter.ToUInt32(isLittleEndian ? buffer.ReverseToNewBytes() : buffer, 0);
        }
        public static uint[] ToUInt32Array(this byte[] buffer, bool isLittleEndian = true)
        {
            var values = new uint[buffer.Length / 4];
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = isLittleEndian
                    ? BitConverter.ToUInt32(new byte[]
                    {
                        buffer[i * 4 + 3],
                        buffer[i * 4 + 2],
                        buffer[i * 4 + 1],
                        buffer[i * 4]
                    }, 0)
                    : BitConverter.ToUInt32(new byte[]
                    {
                        buffer[i * 4],
                        buffer[i * 4 + 1],
                        buffer[i * 4 + 2],
                        buffer[i * 4 + 3]
                    }, 0);
            }
            return values;
        }
        public static float ToFloat(this byte[] buffer, bool isLittleEndian = true)
        {
            if (buffer.Length != 4)
                throw new ArgumentOutOfRangeException(nameof(buffer), buffer.Length, "The array length must be 4");

            return BitConverter.ToSingle(isLittleEndian ? buffer.ReverseToNewBytes() : buffer, 0);
        }
        public static float[] ToFloatArray(this byte[] buffer, bool isLittleEndian = true)
        {
            var values = new float[buffer.Length / 4];
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = isLittleEndian
                    ? BitConverter.ToSingle(new byte[]
                    {
                        buffer[i * 4 + 3],
                        buffer[i * 4 + 2],
                        buffer[i * 4 + 1],
                        buffer[i * 4]
                    }, 0)
                    : BitConverter.ToSingle(new byte[]
                    {
                        buffer[i * 4],
                        buffer[i * 4 + 1],
                        buffer[i * 4 + 2],
                        buffer[i * 4 + 3]
                    }, 0);
            }
            return values;
        }
        public static long ToInt64(this byte[] buffer, bool isLittleEndian = true)
        {
            if (buffer.Length != 8)
                throw new ArgumentOutOfRangeException(nameof(buffer), buffer.Length, "The array length must be 8");

            return BitConverter.ToInt64(isLittleEndian ? buffer.ReverseToNewBytes() : buffer, 0);
        }
        public static long[] ToInt64Array(this byte[] buffer, bool isLittleEndian = true)
        {
            var values = new long[buffer.Length / 8];
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = isLittleEndian
                    ? BitConverter.ToInt64(new byte[]
                    {
                        buffer[i * 8 + 7],
                        buffer[i * 8 + 6],
                        buffer[i * 8 + 5],
                        buffer[i * 8 + 4],
                        buffer[i * 8 + 3],
                        buffer[i * 8 + 2],
                        buffer[i * 8 + 1],
                        buffer[i * 8]
                    }, 0)

                    : BitConverter.ToInt64(new byte[]
                    {
                        buffer[i * 8],
                        buffer[i * 8 + 1],
                        buffer[i * 8 + 2],
                        buffer[i * 8 + 3],
                        buffer[i * 8 + 4],
                        buffer[i * 8 + 5],
                        buffer[i * 8 + 6],
                        buffer[i * 8 + 7]
                    }, 0);
            }
            return values;
        }
        public static ulong ToUInt64(this byte[] buffer, bool isLittleEndian = true)
        {
            if (buffer.Length != 8)
                throw new ArgumentOutOfRangeException(nameof(buffer), buffer.Length, "The array length must be 8");

            return BitConverter.ToUInt64(isLittleEndian ? buffer.ReverseToNewBytes() : buffer, 0);
        }
        public static ulong[] ToUInt64Array(this byte[] buffer, bool isLittleEndian = true)
        {
            var values = new ulong[buffer.Length / 8];
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = isLittleEndian
                    ? BitConverter.ToUInt64(new byte[]
                    {
                        buffer[i * 8 + 7],
                        buffer[i * 8 + 6],
                        buffer[i * 8 + 5],
                        buffer[i * 8 + 4],
                        buffer[i * 8 + 3],
                        buffer[i * 8 + 2],
                        buffer[i * 8 + 1],
                        buffer[i * 8]
                    }, 0)

                    : BitConverter.ToUInt64(new byte[]
                    {
                        buffer[i * 8],
                        buffer[i * 8 + 1],
                        buffer[i * 8 + 2],
                        buffer[i * 8 + 3],
                        buffer[i * 8 + 4],
                        buffer[i * 8 + 5],
                        buffer[i * 8 + 6],
                        buffer[i * 8 + 7]
                    }, 0);
            }
            return values;
        }
        public static double ToDouble(this byte[] buffer, bool isLittleEndian = true)
        {
            if (buffer.Length != 8)
                throw new ArgumentOutOfRangeException(nameof(buffer), buffer.Length, "The array length must be 8");

            return BitConverter.ToDouble(isLittleEndian ? buffer.ReverseToNewBytes() : buffer, 0);
        }
        public static double[] ToDoubleArray(this byte[] buffer, bool isLittleEndian = true)
        {
            var values = new double[buffer.Length / 8];
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = isLittleEndian
                    ? BitConverter.ToDouble(new byte[]
                    {
                        buffer[i * 8 + 7],
                        buffer[i * 8 + 6],
                        buffer[i * 8 + 5],
                        buffer[i * 8 + 4],
                        buffer[i * 8 + 3],
                        buffer[i * 8 + 2],
                        buffer[i * 8 + 1],
                        buffer[i * 8]
                    }, 0)

                    : BitConverter.ToDouble(new byte[]
                    {
                        buffer[i * 8],
                        buffer[i * 8 + 1],
                        buffer[i * 8 + 2],
                        buffer[i * 8 + 3],
                        buffer[i * 8 + 4],
                        buffer[i * 8 + 5],
                        buffer[i * 8 + 6],
                        buffer[i * 8 + 7]
                    }, 0);
            }
            return values;
        }

        public static byte[] ToBytes(this short value, bool isLittleEndian = true)
        {
            return isLittleEndian
                ? BitConverter.GetBytes(value)
                : BitConverter.GetBytes(value).Reverse().ToArray();
        }
        public static byte[] ToBytes(this short[] values, bool isLittleEndian = true)
        {
            var bytes = new byte[values.Length * 2];
            for (int i = 0; i < values.Length; i++)
            {
                var buf = values[i].ToBytes(isLittleEndian);
                bytes[i * 2] = buf[0];
                bytes[i * 2 + 1] = buf[1];
            }
            return bytes;
        }
        public static byte[] ToBytes(this ushort value, bool isLittleEndian = true)
        {
            return isLittleEndian
                ? BitConverter.GetBytes(value)
                : BitConverter.GetBytes(value).Reverse().ToArray();
        }
        public static byte[] ToBytes(this ushort[] values, bool isLittleEndian = true)
        {
            var bytes = new byte[values.Length * 2];
            for (int i = 0; i < values.Length; i++)
            {
                var buf = values[i].ToBytes(isLittleEndian);
                bytes[i * 2] = buf[0];
                bytes[i * 2 + 1] = buf[1];
            }
            return bytes;
        }
        public static byte[] ToBytes(this int value, bool isLittleEndian = true)
        {
            return isLittleEndian
                ? BitConverter.GetBytes(value)
                : BitConverter.GetBytes(value).Reverse().ToArray();
        }
        public static byte[] ToBytes(this int[] values, bool isLittleEndian = true)
        {
            var bytes = new byte[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                var buf = values[i].ToBytes(isLittleEndian);
                bytes[i * 4] = buf[0];
                bytes[i * 4 + 1] = buf[1];
                bytes[i * 4 + 2] = buf[2];
                bytes[i * 4 + 3] = buf[3];
            }
            return bytes;
        }
        public static byte[] ToBytes(this uint value, bool isLittleEndian = true)
        {
            return isLittleEndian
                ? BitConverter.GetBytes(value)
                : BitConverter.GetBytes(value).Reverse().ToArray();
        }
        public static byte[] ToBytes(this uint[] values, bool isLittleEndian = true)
        {
            var bytes = new byte[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                var buf = values[i].ToBytes(isLittleEndian);
                bytes[i * 4] = buf[0];
                bytes[i * 4 + 1] = buf[1];
                bytes[i * 4 + 2] = buf[2];
                bytes[i * 4 + 3] = buf[3];
            }
            return bytes;
        }
        public static byte[] ToBytes(this long value, bool isLittleEndian = true)
        {
            return isLittleEndian
                ? BitConverter.GetBytes(value)
                : BitConverter.GetBytes(value).Reverse().ToArray();
        }
        public static byte[] ToBytes(this long[] values, bool isLittleEndian = true)
        {
            var bytes = new byte[values.Length * 8];
            for (int i = 0; i < values.Length; i++)
            {
                var buf = values[i].ToBytes(isLittleEndian);
                bytes[i * 8] = buf[0];
                bytes[i * 8 + 1] = buf[1];
                bytes[i * 8 + 2] = buf[2];
                bytes[i * 8 + 3] = buf[3];
                bytes[i * 8 + 4] = buf[4];
                bytes[i * 8 + 5] = buf[5];
                bytes[i * 8 + 6] = buf[6];
                bytes[i * 8 + 7] = buf[7];
            }
            return bytes;
        }
        public static byte[] ToBytes(this ulong value, bool isLittleEndian = true)
        {
            return isLittleEndian
                ? BitConverter.GetBytes(value)
                : BitConverter.GetBytes(value).Reverse().ToArray();
        }
        public static byte[] ToBytes(this ulong[] values, bool isLittleEndian = true)
        {
            var bytes = new byte[values.Length * 8];
            for (int i = 0; i < values.Length; i++)
            {
                var buf = values[i].ToBytes(isLittleEndian);
                bytes[i * 8] = buf[0];
                bytes[i * 8 + 1] = buf[1];
                bytes[i * 8 + 2] = buf[2];
                bytes[i * 8 + 3] = buf[3];
                bytes[i * 8 + 4] = buf[4];
                bytes[i * 8 + 5] = buf[5];
                bytes[i * 8 + 6] = buf[6];
                bytes[i * 8 + 7] = buf[7];
            }
            return bytes;
        }
        public static byte[] ToBytes(this float value, bool isLittleEndian = true)
        {
            return isLittleEndian
                ? BitConverter.GetBytes(value)
                : BitConverter.GetBytes(value).Reverse().ToArray();
        }
        public static byte[] ToBytes(this float[] values, bool isLittleEndian = true)
        {
            var bytes = new byte[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                var buf = values[i].ToBytes(isLittleEndian);
                bytes[i * 4] = buf[0];
                bytes[i * 4 + 1] = buf[1];
                bytes[i * 4 + 2] = buf[2];
                bytes[i * 4 + 3] = buf[3];
            }
            return bytes;
        }
        public static byte[] ToBytes(this double value, bool isLittleEndian = true)
        {
            return isLittleEndian
                 ? BitConverter.GetBytes(value)
                 : BitConverter.GetBytes(value).Reverse().ToArray();
        }
        public static byte[] ToBytes(this double[] values, bool isLittleEndian = true)
        {
            var bytes = new byte[values.Length * 8];
            for (int i = 0; i < values.Length; i++)
            {
                var buf = values[i].ToBytes(isLittleEndian);
                bytes[i * 8] = buf[0];
                bytes[i * 8 + 1] = buf[1];
                bytes[i * 8 + 2] = buf[2];
                bytes[i * 8 + 3] = buf[3];
                bytes[i * 8 + 4] = buf[4];
                bytes[i * 8 + 5] = buf[5];
                bytes[i * 8 + 6] = buf[6];
                bytes[i * 8 + 7] = buf[7];
            }
            return bytes;
        }
        private static byte[] ReverseToNewBytes(this byte[] buffer)
        {
            var dst = new byte[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
            {
                dst[i] = buffer[buffer.Length - 1 - i];
            }
            return dst;
        }
    }
}
