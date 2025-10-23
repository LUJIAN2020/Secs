using Secs.Enums;
using Secs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Secs.Messages
{

    /// <summary>
    /// HSMS Data Item<br/>
    /// 1. Data Structure<br/>
    /// 2. byte1: LengthSize (bit0-bit1) + Format (bit2-bit7)<br/>
    /// 3. byte2~byte4: Count<br/>
    /// 4. Actual Data Payload<br/>
    /// </summary>
    public class HsmsBody
    {
        public HsmsBody()
        {
        }
        public HsmsBody(byte[] buffer)
        {
            byte type = (byte)(buffer[0] & 0b11111100); //数据格式
            var format = (SecsFormat)type;
            byte lengthByteCount = (byte)(buffer[0] & 0b00000011); //数据长度字节个数
            var bytesCountBuffer = buffer.Slice(1, lengthByteCount);
            int bytesCount = ConvertToBytesCount(bytesCountBuffer);  //字节长度
            int index = 1;
            index += lengthByteCount;
            Format = format;
            Count = ConvertToCount(format, bytesCount);//项个数
            if (format == SecsFormat.L)
            {
                if (Count > 0)
                {
                    UnwrapListItem(buffer.Slice(index), this, Count);
                }
            }
            else
            {
                RawData = buffer.Slice(index, bytesCount);
                Value = ConverterToValue(RawData, format);
                index += bytesCount;
            }
        }
        public HsmsBody(string sml) : this(ParseBodyBytes(sml))
        {
        }
        private static int ConvertToBytesCount(byte[] bytes)
        {
            var buffer = new byte[4];
            for (int i = 0; i < bytes.Length; i++)
            {
                buffer[i] = bytes[i];
            }
            return BitConverter.ToInt32(buffer, 0);
        }
        private static int UnwrapListItem(byte[] buffer, HsmsBody body, int total)
        {
            int index = 0;
            while (total-- > 0)
            {
                byte b = buffer[index];
                byte type = (byte)(b & 0b11111100);//数据格式
                var format = (SecsFormat)type;
                byte lengthByteCount = (byte)(b & 0b00000011);
                index++;
                byte[] bytesCountBuffer = buffer.Slice(index, lengthByteCount);//数据长度字节个数
                index += lengthByteCount;
                int bytesCount = ConvertToBytesCount(bytesCountBuffer);  //字节长度
                var _body = CreateHsmsBodyFromSecsFormat(format);
                _body.Count = ConvertToCount(format, bytesCount);//项个数
                if (format == SecsFormat.L)
                {
                    if (_body.Count > 0)
                    {
                        index += UnwrapListItem(buffer.Slice(index), _body, _body.Count);
                    }
                }
                else
                {
                    _body.RawData = buffer.Slice(index, bytesCount);
                    _body.Value = ConverterToValue(_body.RawData, format);
                    index += bytesCount;
                }
                body.SubBodys ??= new();
                body.SubBodys.Add(_body);
            }
            return index;
        }
        private static HsmsBody CreateHsmsBodyFromSecsFormat(SecsFormat format) => format switch
        {
            SecsFormat.L => new L(),
            SecsFormat.B => new B(),
            SecsFormat.BOOLEAN => new BOOLEAN(),
            SecsFormat.A => new A(),
            SecsFormat.I1 => new I1(),
            SecsFormat.I2 => new I2(),
            SecsFormat.I4 => new I4(),
            SecsFormat.I8 => new I8(),
            SecsFormat.U1 => new U1(),
            SecsFormat.U2 => new U2(),
            SecsFormat.U4 => new U4(),
            SecsFormat.U8 => new U8(),
            SecsFormat.F4 => new F4(),
            SecsFormat.F8 => new F8(),
            _ => throw new Exception($"Unsupported data format, `{format.ToString()}`"),
        };
        private static int ConvertToBytesCount(SecsFormat secsFormat, int count)
        {
            switch (secsFormat)
            {
                case SecsFormat.I2:
                case SecsFormat.U2:
                    count *= 2;
                    break;
                case SecsFormat.I4:
                case SecsFormat.U4:
                case SecsFormat.F4:
                    count *= 4;
                    break;
                case SecsFormat.I8:
                case SecsFormat.U8:
                case SecsFormat.F8:
                    count *= 8;
                    break;
            }
            return count;
        }
        private static int ConvertToCount(SecsFormat secsFormat, int bytesCount)
        {
            switch (secsFormat)
            {
                case SecsFormat.I2:
                case SecsFormat.U2:
                    bytesCount /= 2;
                    break;
                case SecsFormat.I4:
                case SecsFormat.U4:
                case SecsFormat.F4:
                    bytesCount /= 4;
                    break;
                case SecsFormat.I8:
                case SecsFormat.U8:
                case SecsFormat.F8:
                    bytesCount /= 8;
                    break;
            }
            return bytesCount;
        }

        /// <summary>
        /// Data format, first byte: bit2-bit7
        /// </summary>
        public SecsFormat Format { get; protected set; }

        /// <summary>
        /// Total number of length bytes, first byte: bit0-bit1
        /// </summary>
        public byte LengthSize => GetLengthSize(Count);

        /// <summary>
        /// Number of data items. 
        /// For LIST structures, it represents the item count; for others, it represents the byte count, determined by LengthSize.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Actual value
        /// </summary>
        public object? Value { get; set; }

        /// <summary>
        /// If it is a LIST, it may have sub-items
        /// </summary>
        public List<HsmsBody>? SubBodys { get; set; }

        /// <summary>
        /// Raw data bytes count
        /// </summary>
        public int BytesCount => ConvertToBytesCount(Format, Count);

        /// <summary>
        /// Raw bytes
        /// </summary>
        public byte[] RawData { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// Parameter description, optional and can be ignored
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Get the maximum length size in bytes
        /// </summary>
        /// <param name="count">Number of data items</param>
        /// <returns>LengthSize in bytes</returns>
        public static byte GetLengthSize(int count)
        {
            return count switch
            {
                <= byte.MaxValue => 1,
                > byte.MaxValue and <= ushort.MaxValue => 2,
                _ => 3,
            };
        }
        public static byte[] ConverterToBytes(HsmsBody body)
        {
            var list = new List<byte>();
            ConverterToBytes(body, ref list);
            return list.ToArray();
        }
        public static string ConverterToSml(HsmsBody body, bool addDescription = false)
        {
            var sb = new StringBuilder();
            int span = 0;
            HsmsBodyToSml(body, sb, ref span, addDescription, SpanceSize);
            return sb.ToString().Trim();
        }
        public static object? GetHsmsBodyDefaultValue(SecsFormat format)
        {
            return format switch
            {
                SecsFormat.L => default,
                SecsFormat.B => Array.Empty<byte>(),
                SecsFormat.A => string.Empty,
                SecsFormat.BOOLEAN => Array.Empty<bool>(),
                SecsFormat.I1 => Array.Empty<sbyte>(),
                SecsFormat.I2 => Array.Empty<short>(),
                SecsFormat.I4 => Array.Empty<int>(),
                SecsFormat.I8 => Array.Empty<long>(),
                SecsFormat.U1 => Array.Empty<byte>(),
                SecsFormat.U2 => Array.Empty<ushort>(),
                SecsFormat.U4 => Array.Empty<uint>(),
                SecsFormat.U8 => Array.Empty<ulong>(),
                SecsFormat.F4 => Array.Empty<float>(),
                SecsFormat.F8 => Array.Empty<double>(),
                _ => throw new Exception($"Unsupported data format, `{format}`"),
            };
        }
        public static object? ConverterToValue(byte[] data, SecsFormat format)
        {
            if (data.Length == 0)
                return GetHsmsBodyDefaultValue(format);

            return format switch
            {
                SecsFormat.L => default,
                SecsFormat.A => Encoding.ASCII.GetString(data),
                SecsFormat.B => data,
                SecsFormat.BOOLEAN => data.Select(c => c == 1).ToArray(),
                SecsFormat.I1 => data.Select(c => (sbyte)c).ToArray(),
                SecsFormat.I2 => data.ToInt16Array(),
                SecsFormat.I4 => data.ToInt32Array(),
                SecsFormat.I8 => data.ToInt64Array(),
                SecsFormat.U1 => data,
                SecsFormat.U2 => data.ToUInt16Array(),
                SecsFormat.U4 => data.ToUInt32Array(),
                SecsFormat.U8 => data.ToUInt64Array(),
                SecsFormat.F4 => data.ToFloatArray(),
                SecsFormat.F8 => data.ToDoubleArray(),
                _ => throw new Exception($"Unsupported data format, `{format}`"),
            };
        }
        public const int SpanceSize = 4;
        public const string Spance = " ";
        public const string Annotation1 = "/* ";
        public const string Annotation2 = " */";
        public const string LeftSquareBrackets = "[";
        public const string RightSquareBrackets = "]";
        public static char[] SpanceSeparator = new char[] { ' ' };
        private static void HsmsBodyToSml(HsmsBody body, StringBuilder sb, ref int span, bool addDescription, int spanceSize = SpanceSize)
        {
            string spaces = new(' ', span);
            switch (body.Format)
            {
                case SecsFormat.L:
                    {
                        sb.Append($"{spaces}<{body.Format}[{body.Count}]");
                        AddDescription(addDescription, body, sb);
                        if (body.SubBodys != null)
                        {
                            span += spanceSize;
                            foreach (var sub in body.SubBodys)
                            {
                                HsmsBodyToSml(sub, sb, ref span, addDescription);
                            }
                            span -= spanceSize;
                            sb.AppendLine($"{spaces}>");
                        }
                        break;
                    }
                case SecsFormat.A:
                    sb.Append($"{spaces}<{body.Format}[{body.Count}] \"{body.Value}\">");
                    AddDescription(addDescription, body, sb);
                    break;
                case SecsFormat.B:
                case SecsFormat.BOOLEAN:
                    {
                        string hex = string.Join(" ", body.RawData.Select(c => $"0x{c:X2}"));
                        sb.Append($"{spaces}<{body.Format}[{body.Count}] {hex}>");
                        AddDescription(addDescription, body, sb);
                        break;
                    }
                case SecsFormat.I1:
                case SecsFormat.I2:
                case SecsFormat.I4:
                case SecsFormat.I8:
                case SecsFormat.U1:
                case SecsFormat.U2:
                case SecsFormat.U4:
                case SecsFormat.U8:
                case SecsFormat.F4:
                case SecsFormat.F8:
                    {
                        var values = new List<string>();
                        if (body.Value is Array array)
                        {
                            foreach (var value in array)
                            {
                                values.Add(value.ToString());
                            }
                        }
                        string valueText = string.Join(" ", values);
                        sb.Append($"{spaces}<{body.Format}[{body.Count}] {valueText}>");
                        AddDescription(addDescription, body, sb);
                        break;
                    }
                default:
                    sb.AppendLine($"{spaces}<{body.Format}[1] {body.Value}>");
                    break;
            }
        }
        private static void AddDescription(bool addDescription, HsmsBody body, StringBuilder sb)
        {
            if (addDescription && !string.IsNullOrWhiteSpace(body.Description))
            {
                sb.Append(Spance)
                    .Append(Annotation1)
                    .Append(body.Description)
                    .Append(Annotation2);
            }
            sb.Append("\r\n");
        }
        private static void ConverterToBytes(HsmsBody body, ref List<byte> buffer)
        {
            if (body.Format == SecsFormat.L)
            {
                byte b1 = (byte)((byte)body.Format + body.LengthSize);
                buffer.Add(b1);
                buffer.Add((byte)body.Count);
                if (body.SubBodys != null)
                {
                    foreach (var item in body.SubBodys)
                    {
                        ConverterToBytes(item, ref buffer);
                    }
                }
            }
            else
            {
                var header = new byte[body.LengthSize + 1];
                //type(bit3-bit7) + number of bytes for data length(bit0-bit1)
                header[0] = (byte)((byte)body.Format + body.LengthSize);
                //Number of bytes
                var lens = BitConverter.GetBytes(body.BytesCount);
                Array.Copy(lens, 0, header, 1, body.LengthSize);
                buffer.AddRange(header);
                buffer.AddRange(body.RawData);
            }
        }
        private static byte[] ParseBodyBytes(string sml)
        {
            var rows = new List<string>();
            var segments = sml.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < segments.Length; i++)
            {
                string row = segments[i].Trim();
                if (row.Length > 0 && row[0] == '<')
                {
                    row = row.Remove(0, 1);
                    if (row.Length > 0 && row[row.Length - 1] == '>')
                    {
                        row = row.Remove(row.Length - 1, 1);
                    }
                    rows.Add(row);
                }
            }
            return ParseSegmets(rows);
        }
        private static byte[] ParseSegmets(IEnumerable<string> segments)
        {
            var list = new List<byte>();
            foreach (var segment in segments)
            {
                /* 
L[2]
    L[3]
        BOOLEAN[2] 0x01 0x00>
        F4[1] 0.5567>
        L[1]
            F8[1] 0.9>
        
    
    B[5] 0x00 0x05 0x06 0x09 0xFF>

                 */

                if (segment.Length < 4)
                    throw new Exception($"Format error, \"{segment}\" is invalid. The minimum data length is 4, Sample:`L[3]`");

                int leftBracketIndex = segment.IndexOf(LeftSquareBrackets);
                int rightBracketIndex = segment.IndexOf(RightSquareBrackets);
                if (leftBracketIndex == 0)
                    throw new Exception($"Format error, \"{segment}\" is invalid. The content not Contains `{LeftSquareBrackets}`");

                if (leftBracketIndex == 0)
                    throw new Exception($"Format error, \"{segment}\" is invalid. The content not Contains `{RightSquareBrackets}`");

                string format = segment.Substring(0, leftBracketIndex);
                if (!Enum.TryParse<SecsFormat>(format, out var secsFormat))
                    throw new Exception($"Format parsing error. \"{format}\" is invalid. Expected: L,B,BOOLEAN,A,I1,I2,I4,I8,U1,U2,U4,U8,F4,F8");
                byte secsFormatValue = (byte)secsFormat;

                string countText = segment.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1);
                if (!int.TryParse(countText, out int count))
                    throw new Exception($"Data length format parsing error. \"{segment}\" is invalid.");

                var dataBytes = Array.Empty<byte>();
                if (secsFormat != SecsFormat.L)
                {
                    if (segment[rightBracketIndex + 1] != ' ')
                        throw new Exception($"Format error, \"{segment}\" is invalid. `]` followed by a space");

                    string content = segment.Substring(rightBracketIndex + 2).Trim();
                    dataBytes = GetContentBytes(secsFormat, content);
                }
                int bytesCount = ConvertToBytesCount(secsFormat, count);
                var lengthBytes = GetLengthBytes(bytesCount, secsFormatValue);
                list.AddRange(lengthBytes);
                if (dataBytes.Length > 0)
                {
                    list.AddRange(dataBytes);
                }
            }
            return list.ToArray();
        }
        private static byte[] GetContentBytes(SecsFormat secsFormat, string content)
        {
            switch (secsFormat)
            {
                case SecsFormat.B:
                case SecsFormat.BOOLEAN:
                    return content.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(c => Convert.ToByte(c.ToLower().Replace("0x", ""), 16))
                        .ToArray();
                case SecsFormat.A:
                    int leftDot = content.IndexOf('"');
                    int rightDot = content.LastIndexOf('"');
                    content = content.Substring(leftDot + 1, rightDot - leftDot - 1);
                    return Encoding.ASCII.GetBytes(content);
                case SecsFormat.I1:
                case SecsFormat.I2:
                case SecsFormat.I4:
                case SecsFormat.I8:
                case SecsFormat.U1:
                case SecsFormat.U2:
                case SecsFormat.U4:
                case SecsFormat.U8:
                case SecsFormat.F4:
                case SecsFormat.F8:
                    return ToBytes(ToValueContens(content), secsFormat);
                default:
                    throw new Exception($"Unsupported SecsFormat `{secsFormat}`");
            }
        }
        private static string[] ToValueContens(string content)
        {
            return content.Split(SpanceSeparator, StringSplitOptions.RemoveEmptyEntries);
        }
        private static byte[] ToBytes(string[] contents, SecsFormat format)
        {
            return format switch
            {
                SecsFormat.I1 => contents.Select(c => (byte)sbyte.Parse(c)).ToArray(),
                SecsFormat.I2 => contents.Select(c => short.Parse(c)).ToArray().ToBytes(false),
                SecsFormat.I4 => contents.Select(c => int.Parse(c)).ToArray().ToBytes(false),
                SecsFormat.I8 => contents.Select(c => long.Parse(c)).ToArray().ToBytes(false),
                SecsFormat.U1 => contents.Select(c => byte.Parse(c)).ToArray(),
                SecsFormat.U2 => contents.Select(c => ushort.Parse(c)).ToArray().ToBytes(false),
                SecsFormat.U4 => contents.Select(c => uint.Parse(c)).ToArray().ToBytes(false),
                SecsFormat.U8 => contents.Select(c => ulong.Parse(c)).ToArray().ToBytes(false),
                SecsFormat.F4 => contents.Select(c => float.Parse(c)).ToArray().ToBytes(false),
                SecsFormat.F8 => contents.Select(c => double.Parse(c)).ToArray().ToBytes(false),
                _ => throw new Exception($"Unsupported secs format `{format}`"),
            };
        }
        private static byte[] GetLengthBytes(int length, byte secsFormatValue)
        {
            switch (length)
            {
                case 0:
                    return new byte[] { (byte)(secsFormatValue | 0b0000_00001) };
                case > 0 and <= 0xFF:
                    {
                        byte byte1 = (byte)(secsFormatValue | 0b0000_00001);
                        byte byte2 = (byte)length;
                        return new byte[] { byte1, byte2 };
                    }

                case > 0xFF and <= 0xFF_FF:
                    {
                        byte byte1 = (byte)(secsFormatValue | 0b0000_00010);
                        byte byte2 = (byte)(length >> 8);
                        byte byte3 = (byte)length;
                        return new byte[] { byte1, byte2, byte3 };
                    }

                case > 0xFF_FF and <= 0xFF_FF_FF:
                    {
                        byte byte1 = (byte)(secsFormatValue | 0b0000_00011);
                        byte byte2 = (byte)(length >> 16);
                        byte byte3 = (byte)(length >> 8);
                        byte byte4 = (byte)length;
                        return new byte[] { byte1, byte2, byte3, byte4 };
                    }

                default:
                    throw new Exception("Data length must not exceed 0xFFF");
            }
        }
    }
}