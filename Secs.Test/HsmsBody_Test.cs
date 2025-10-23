using Secs.Enums;
using Secs.Messages;
using System.Text.Json;

namespace Secs.Test
{
    [TestClass]
    public class HsmsBody_Test
    {
        [TestMethod]
        public void HsmsBody_ConverterToBytes_Test()
        {
            var expected = new byte[]
            {
                0x01 ,0x12 ,0x41 ,0x06 ,0x41 ,0x62 ,0x63 ,0x31 ,0x32 ,0x33 ,0xA5 ,0x01 ,0x01 ,0xA9 ,0x02 ,0x00 ,0x02 ,0xB1 ,0x04 ,0x00,
                0x00 ,0x00 ,0x04 ,0xA1 ,0x08 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x08 ,0x65 ,0x01 ,0xFF ,0x65 ,0x01 ,0x01 ,0x69,
                0x02 ,0xFF ,0xFE ,0x69 ,0x02 ,0x00 ,0x02 ,0x71 ,0x04 ,0xFF ,0xFF ,0xFF ,0xFC ,0x71 ,0x04 ,0x00 ,0x00 ,0x00 ,0x04 ,0x61,
                0x08 ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF ,0xF8 ,0x61 ,0x08 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x08 ,0x91,
                0x04 ,0xBF ,0x9E ,0x06 ,0x10 ,0x91 ,0x04 ,0x3F ,0x9E ,0x06 ,0x10 ,0x81 ,0x08 ,0xBF ,0xF3 ,0xC0 ,0xC1 ,0xFC ,0x8F ,0x32,
                0x38 ,0x81 ,0x08 ,0x3F ,0xF3 ,0xC0 ,0xC1 ,0xFC ,0x8F ,0x32 ,0x38 ,0x01 ,0x02 ,0x01 ,0x11 ,0x41 ,0x06 ,0x41 ,0x62 ,0x63,
                0x31 ,0x32 ,0x33 ,0xA5 ,0x01 ,0x01 ,0xA9 ,0x02 ,0x00 ,0x02 ,0xB1 ,0x04 ,0x00 ,0x00 ,0x00 ,0x04 ,0xA1 ,0x08 ,0x00 ,0x00,
                0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x08 ,0x65 ,0x01 ,0xFF ,0x65 ,0x01 ,0x01 ,0x69 ,0x02 ,0xFF ,0xFE ,0x69 ,0x02 ,0x00 ,0x02,
                0x71 ,0x04 ,0xFF ,0xFF ,0xFF ,0xFC ,0x71 ,0x04 ,0x00 ,0x00 ,0x00 ,0x04 ,0x61 ,0x08 ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF,
                0xFF ,0xF8 ,0x61 ,0x08 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x08 ,0x91 ,0x04 ,0xBF ,0x9E ,0x06 ,0x10 ,0x91 ,0x04,
                0x3F ,0x9E ,0x06 ,0x10 ,0x81 ,0x08 ,0xBF ,0xF3 ,0xC0 ,0xC1 ,0xFC ,0x8F ,0x32 ,0x38 ,0x81 ,0x08 ,0x3F ,0xF3 ,0xC0 ,0xC1,
                0xFC ,0x8F ,0x32 ,0x38 ,0x01 ,0x11 ,0x41 ,0x06 ,0x41 ,0x62 ,0x63 ,0x31 ,0x32 ,0x33 ,0xA5 ,0x01 ,0x01 ,0xA9 ,0x02 ,0x00,
                0x02 ,0xB1 ,0x04 ,0x00 ,0x00 ,0x00 ,0x04 ,0xA1 ,0x08 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x08 ,0x65 ,0x01 ,0xFF,
                0x65 ,0x01 ,0x01 ,0x69 ,0x02 ,0xFF ,0xFE ,0x69 ,0x02 ,0x00 ,0x02 ,0x71 ,0x04 ,0xFF ,0xFF ,0xFF ,0xFC ,0x71 ,0x04 ,0x00,
                0x00 ,0x00 ,0x04 ,0x61 ,0x08 ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF ,0xF8 ,0x61 ,0x08 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00,
                0x00 ,0x00 ,0x08 ,0x91 ,0x04 ,0xBF ,0x9E ,0x06 ,0x10 ,0x91 ,0x04 ,0x3F ,0x9E ,0x06 ,0x10 ,0x81 ,0x08 ,0xBF ,0xF3 ,0xC0,
                0xC1 ,0xFC ,0x8F ,0x32 ,0x38 ,0x81 ,0x08 ,0x3F ,0xF3 ,0xC0 ,0xC1 ,0xFC ,0x8F ,0x32 ,0x38
            };
            var body = new L
            (
                new A("Abc123"),
                new U1(1),
                new U2(2),
                new U4(4),
                new U8(8),
                new I1(-1),
                new I1(1),
                new I2(-2),
                new I2(2),
                new I4(-4),
                new I4(4),
                new I8(-8),
                new I8(8),
                new F4(-1.23456f),
                new F4(1.23456f),
                new F8(-1.23456),
                new F8(1.23456),
                new L
                (
                    new L
                    (
                        new A("Abc123"),
                        new U1(1),
                        new U2(2),
                        new U4(4),
                        new U8(8),
                        new I1(-1),
                        new I1(1),
                        new I2(-2),
                        new I2(2),
                        new I4(-4),
                        new I4(4),
                        new I8(-8),
                        new I8(8),
                        new F4(-1.23456f),
                        new F4(1.23456f),
                        new F8(-1.23456),
                        new F8(1.23456)
                    ),
                    new L
                    (
                        new A("Abc123"),
                        new U1(1),
                        new U2(2),
                        new U4(4),
                        new U8(8),
                        new I1(-1),
                        new I1(1),
                        new I2(-2),
                        new I2(2),
                        new I4(-4),
                        new I4(4),
                        new I8(-8),
                        new I8(8),
                        new F4(-1.23456f),
                        new F4(1.23456f),
                        new F8(-1.23456),
                        new F8(1.23456)
                    )
                )
            );
            var actual = HsmsBody.ConverterToBytes(body);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Bytes_Constructor_To_HsmsBody_Test()
        {
            var bytes = new byte[]
           {
                0x01 ,0x12 ,0x41 ,0x06 ,0x41 ,0x62 ,0x63 ,0x31 ,0x32 ,0x33 ,0xA5 ,0x01 ,0x01 ,0xA9 ,0x02 ,0x00 ,0x02 ,0xB1 ,0x04 ,0x00,
                0x00 ,0x00 ,0x04 ,0xA1 ,0x08 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x08 ,0x65 ,0x01 ,0xFF ,0x65 ,0x01 ,0x01 ,0x69,
                0x02 ,0xFF ,0xFE ,0x69 ,0x02 ,0x00 ,0x02 ,0x71 ,0x04 ,0xFF ,0xFF ,0xFF ,0xFC ,0x71 ,0x04 ,0x00 ,0x00 ,0x00 ,0x04 ,0x61,
                0x08 ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF ,0xF8 ,0x61 ,0x08 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x08 ,0x91,
                0x04 ,0xBF ,0x9E ,0x06 ,0x10 ,0x91 ,0x04 ,0x3F ,0x9E ,0x06 ,0x10 ,0x81 ,0x08 ,0xBF ,0xF3 ,0xC0 ,0xC1 ,0xFC ,0x8F ,0x32,
                0x38 ,0x81 ,0x08 ,0x3F ,0xF3 ,0xC0 ,0xC1 ,0xFC ,0x8F ,0x32 ,0x38 ,0x01 ,0x02 ,0x01 ,0x11 ,0x41 ,0x06 ,0x41 ,0x62 ,0x63,
                0x31 ,0x32 ,0x33 ,0xA5 ,0x01 ,0x01 ,0xA9 ,0x02 ,0x00 ,0x02 ,0xB1 ,0x04 ,0x00 ,0x00 ,0x00 ,0x04 ,0xA1 ,0x08 ,0x00 ,0x00,
                0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x08 ,0x65 ,0x01 ,0xFF ,0x65 ,0x01 ,0x01 ,0x69 ,0x02 ,0xFF ,0xFE ,0x69 ,0x02 ,0x00 ,0x02,
                0x71 ,0x04 ,0xFF ,0xFF ,0xFF ,0xFC ,0x71 ,0x04 ,0x00 ,0x00 ,0x00 ,0x04 ,0x61 ,0x08 ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF,
                0xFF ,0xF8 ,0x61 ,0x08 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x08 ,0x91 ,0x04 ,0xBF ,0x9E ,0x06 ,0x10 ,0x91 ,0x04,
                0x3F ,0x9E ,0x06 ,0x10 ,0x81 ,0x08 ,0xBF ,0xF3 ,0xC0 ,0xC1 ,0xFC ,0x8F ,0x32 ,0x38 ,0x81 ,0x08 ,0x3F ,0xF3 ,0xC0 ,0xC1,
                0xFC ,0x8F ,0x32 ,0x38 ,0x01 ,0x11 ,0x41 ,0x06 ,0x41 ,0x62 ,0x63 ,0x31 ,0x32 ,0x33 ,0xA5 ,0x01 ,0x01 ,0xA9 ,0x02 ,0x00,
                0x02 ,0xB1 ,0x04 ,0x00 ,0x00 ,0x00 ,0x04 ,0xA1 ,0x08 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x08 ,0x65 ,0x01 ,0xFF,
                0x65 ,0x01 ,0x01 ,0x69 ,0x02 ,0xFF ,0xFE ,0x69 ,0x02 ,0x00 ,0x02 ,0x71 ,0x04 ,0xFF ,0xFF ,0xFF ,0xFC ,0x71 ,0x04 ,0x00,
                0x00 ,0x00 ,0x04 ,0x61 ,0x08 ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF ,0xF8 ,0x61 ,0x08 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00,
                0x00 ,0x00 ,0x08 ,0x91 ,0x04 ,0xBF ,0x9E ,0x06 ,0x10 ,0x91 ,0x04 ,0x3F ,0x9E ,0x06 ,0x10 ,0x81 ,0x08 ,0xBF ,0xF3 ,0xC0,
                0xC1 ,0xFC ,0x8F ,0x32 ,0x38 ,0x81 ,0x08 ,0x3F ,0xF3 ,0xC0 ,0xC1 ,0xFC ,0x8F ,0x32 ,0x38
           };
            var actual = new HsmsBody(bytes);
            var expected = new L
            (
                new A("Abc123"),
                new U1(1),
                new U2(2),
                new U4(4),
                new U8(8),
                new I1(-1),
                new I1(1),
                new I2(-2),
                new I2(2),
                new I4(-4),
                new I4(4),
                new I8(-8),
                new I8(8),
                new F4(-1.23456f),
                new F4(1.23456f),
                new F8(-1.23456),
                new F8(1.23456),
                new L
                (
                    new L
                    (
                        new A("Abc123"),
                        new U1(1),
                        new U2(2),
                        new U4(4),
                        new U8(8),
                        new I1(-1),
                        new I1(1),
                        new I2(-2),
                        new I2(2),
                        new I4(-4),
                        new I4(4),
                        new I8(-8),
                        new I8(8),
                        new F4(-1.23456f),
                        new F4(1.23456f),
                        new F8(-1.23456),
                        new F8(1.23456)
                    ),
                    new L
                    (
                        new A("Abc123"),
                        new U1(1),
                        new U2(2),
                        new U4(4),
                        new U8(8),
                        new I1(-1),
                        new I1(1),
                        new I2(-2),
                        new I2(2),
                        new I4(-4),
                        new I4(4),
                        new I8(-8),
                        new I8(8),
                        new F4(-1.23456f),
                        new F4(1.23456f),
                        new F8(-1.23456),
                        new F8(1.23456)
                    )
                )
            );
            string str1 = JsonSerializer.Serialize(actual);
            string str2 = JsonSerializer.Serialize(expected);
            Assert.AreEqual(str1, str2);
        }

        [TestMethod]
        public void Sml_Constructor_To_HsmsBody_Test()
        {
            string sml = @"
<L[18]
    <A[6] ""Abc123"">
    <U1[1] 1>
    <U2[1] 2>
    <U4[1] 4>
    <U8[1] 8>
    <I1[1] -1>
    <I1[1] 1>
    <I2[1] -2>
    <I2[1] 2>
    <I4[1] -4>
    <I4[1] 4>
    <I8[1] -8>
    <I8[1] 8>
    <F4[1] -1.23456>
    <F4[1] 1.23456>
    <F8[1] -1.23456>
    <F8[1] 1.23456>
    <L[2]
        <L[17]
            <A[6] ""Abc123"">
            <U1[1] 1>
            <U2[1] 2>
            <U4[1] 4>
            <U8[1] 8>
            <I1[1] -1>
            <I1[1] 1>
            <I2[1] -2>
            <I2[1] 2>
            <I4[1] -4>
            <I4[1] 4>
            <I8[1] -8>
            <I8[1] 8>
            <F4[1] -1.23456>
            <F4[1] 1.23456>
            <F8[1] -1.23456>
            <F8[1] 1.23456>
        >
        <L[19]
            <A[6] ""Abc123"">
            <U1[1] 1>
            <U2[1] 2>
            <U4[1] 4>
            <U8[1] 8>
            <I1[1] -1>
            <I1[1] 1>
            <I2[1] -2>
            <I2[1] 2>
            <I4[1] -4>
            <I4[1] 4>
            <I8[1] -8>
            <I8[1] 8>
            <F4[1] -1.23456>
            <F4[1] 1.23456>
            <F8[1] -1.23456>
            <F8[1] 1.23456>
            <B[3] 0x13 0xAB 0x1F>
            <BOOLEAN[2] 0x00 0x01>
        >
    >
>".Trim();
            var actual = new HsmsBody(sml);
            var expected = new L
                (
                    new A("Abc123"),
                    new U1(1),
                    new U2(2),
                    new U4(4),
                    new U8(8),
                    new I1(-1),
                    new I1(1),
                    new I2(-2),
                    new I2(2),
                    new I4(-4),
                    new I4(4),
                    new I8(-8),
                    new I8(8),
                    new F4(-1.23456f),
                    new F4(1.23456f),
                    new F8(-1.23456),
                    new F8(1.23456),
                    new L
                    (
                        new L
                        (
                            new A("Abc123"),
                            new U1(1),
                            new U2(2),
                            new U4(4),
                            new U8(8),
                            new I1(-1),
                            new I1(1),
                            new I2(-2),
                            new I2(2),
                            new I4(-4),
                            new I4(4),
                            new I8(-8),
                            new I8(8),
                            new F4(-1.23456f),
                            new F4(1.23456f),
                            new F8(-1.23456),
                            new F8(1.23456)
                        ),
                        new L
                        (
                            new A("Abc123"),
                            new U1(1),
                            new U2(2),
                            new U4(4),
                            new U8(8),
                            new I1(-1),
                            new I1(1),
                            new I2(-2),
                            new I2(2),
                            new I4(-4),
                            new I4(4),
                            new I8(-8),
                            new I8(8),
                            new F4(-1.23456f),
                            new F4(1.23456f),
                            new F8(-1.23456),
                            new F8(1.23456),
                            new B(new byte[] { 0x13, 0xAB, 0x1F }),
                            new BOOLEAN(new bool[] { false, true })
                        )
                    )
                );
            string str1 = JsonSerializer.Serialize(expected);
            string str2 = JsonSerializer.Serialize(actual);
            Assert.AreEqual(str1, str2);
        }

        [TestMethod]
        public void HsmsBody_ConverterToSml_Test()
        {
            string expected = @"
<L[18]
    <A[6] ""Abc123"">
    <U1[1] 1>
    <U2[1] 2>
    <U4[1] 4>
    <U8[1] 8>
    <I1[1] -1>
    <I1[1] 1>
    <I2[1] -2>
    <I2[1] 2>
    <I4[1] -4>
    <I4[1] 4>
    <I8[1] -8>
    <I8[1] 8>
    <F4[1] -1.23456>
    <F4[1] 1.23456>
    <F8[1] -1.23456>
    <F8[1] 1.23456>
    <L[2]
        <L[17]
            <A[6] ""Abc123"">
            <U1[1] 1>
            <U2[1] 2>
            <U4[1] 4>
            <U8[1] 8>
            <I1[1] -1>
            <I1[1] 1>
            <I2[1] -2>
            <I2[1] 2>
            <I4[1] -4>
            <I4[1] 4>
            <I8[1] -8>
            <I8[1] 8>
            <F4[1] -1.23456>
            <F4[1] 1.23456>
            <F8[1] -1.23456>
            <F8[1] 1.23456>
        >
        <L[19]
            <A[6] ""Abc123"">
            <U1[1] 1>
            <U2[1] 2>
            <U4[1] 4>
            <U8[1] 8>
            <I1[1] -1>
            <I1[1] 1>
            <I2[1] -2>
            <I2[1] 2>
            <I4[1] -4>
            <I4[1] 4>
            <I8[1] -8>
            <I8[1] 8>
            <F4[1] -1.23456>
            <F4[1] 1.23456>
            <F8[1] -1.23456>
            <F8[1] 1.23456>
            <B[3] 0x13 0xAB 0x1F>
            <BOOLEAN[2] 0x00 0x01>
        >
    >
>".Trim();
            var body = new L
                (
                    new A("Abc123"),
                    new U1(1),
                    new U2(2),
                    new U4(4),
                    new U8(8),
                    new I1(-1),
                    new I1(1),
                    new I2(-2),
                    new I2(2),
                    new I4(-4),
                    new I4(4),
                    new I8(-8),
                    new I8(8),
                    new F4(-1.23456f),
                    new F4(1.23456f),
                    new F8(-1.23456),
                    new F8(1.23456),
                    new L
                    (
                        new L
                        (
                            new A("Abc123"),
                            new U1(1),
                            new U2(2),
                            new U4(4),
                            new U8(8),
                            new I1(-1),
                            new I1(1),
                            new I2(-2),
                            new I2(2),
                            new I4(-4),
                            new I4(4),
                            new I8(-8),
                            new I8(8),
                            new F4(-1.23456f),
                            new F4(1.23456f),
                            new F8(-1.23456),
                            new F8(1.23456)
                        ),
                        new L
                        (
                            new A("Abc123"),
                            new U1(1),
                            new U2(2),
                            new U4(4),
                            new U8(8),
                            new I1(-1),
                            new I1(1),
                            new I2(-2),
                            new I2(2),
                            new I4(-4),
                            new I4(4),
                            new I8(-8),
                            new I8(8),
                            new F4(-1.23456f),
                            new F4(1.23456f),
                            new F8(-1.23456),
                            new F8(1.23456),
                            new B(new byte[] { 0x13, 0xAB, 0x1F }),
                            new BOOLEAN(new bool[] { false, true })
                        )
                    )
                );
            var actual = HsmsBody.ConverterToSml(body).Trim();
            Assert.AreEqual(expected, actual);
        }



        [TestMethod]
        [DataRow(SecsFormat.A)]
        public void HsmsBody_GetHsmsBodyDefaultValue_Test_Return_String_Empty(SecsFormat format)
        {
            string expected = string.Empty;
            var actual = HsmsBody.GetHsmsBodyDefaultValue(format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(SecsFormat.B)]
        public void HsmsBody_B_GetHsmsBodyDefaultValue_Test_Return_Array_Empty_Bytes(SecsFormat format)
        {
            var expected = Array.Empty<byte>();
            var actual = HsmsBody.GetHsmsBodyDefaultValue(format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(SecsFormat.L)]
        public void HsmsBody_L_GetHsmsBodyDefaultValue_Test_Return_Null(SecsFormat format)
        {
            object? expected = default;
            var actual = HsmsBody.GetHsmsBodyDefaultValue(format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(SecsFormat.BOOLEAN)]
        public void HsmsBody_L_GetHsmsBodyDefaultValue_Test_Return_Booleans(SecsFormat format)
        {
            object? expected = Array.Empty<bool>();
            var actual = HsmsBody.GetHsmsBodyDefaultValue(format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(SecsFormat.I1)]
        public void HsmsBody_L_GetHsmsBodyDefaultValue_Test_Return_SBytes(SecsFormat format)
        {
            object? expected = Array.Empty<sbyte>();
            var actual = HsmsBody.GetHsmsBodyDefaultValue(format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(SecsFormat.I2)]
        public void HsmsBody_L_GetHsmsBodyDefaultValue_Test_Return_Shorts(SecsFormat format)
        {
            object? expected = Array.Empty<short>();
            var actual = HsmsBody.GetHsmsBodyDefaultValue(format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(SecsFormat.I4)]
        public void HsmsBody_L_GetHsmsBodyDefaultValue_Test_Return_Ints(SecsFormat format)
        {
            object? expected = Array.Empty<int>();
            var actual = HsmsBody.GetHsmsBodyDefaultValue(format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(SecsFormat.I8)]
        public void HsmsBody_L_GetHsmsBodyDefaultValue_Test_Return_Longs(SecsFormat format)
        {
            object? expected = Array.Empty<long>();
            var actual = HsmsBody.GetHsmsBodyDefaultValue(format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(SecsFormat.U1)]
        public void HsmsBody_L_GetHsmsBodyDefaultValue_Test_Return_Bytes(SecsFormat format)
        {
            object? expected = Array.Empty<byte>();
            var actual = HsmsBody.GetHsmsBodyDefaultValue(format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(SecsFormat.U2)]
        public void HsmsBody_L_GetHsmsBodyDefaultValue_Test_Return_UShorts(SecsFormat format)
        {
            object? expected = Array.Empty<ushort>();
            var actual = HsmsBody.GetHsmsBodyDefaultValue(format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(SecsFormat.U4)]
        public void HsmsBody_L_GetHsmsBodyDefaultValue_Test_Return_UInts(SecsFormat format)
        {
            object? expected = Array.Empty<uint>();
            var actual = HsmsBody.GetHsmsBodyDefaultValue(format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(SecsFormat.U8)]
        public void HsmsBody_L_GetHsmsBodyDefaultValue_Test_Return_ULongs(SecsFormat format)
        {
            object? expected = Array.Empty<ulong>();
            var actual = HsmsBody.GetHsmsBodyDefaultValue(format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(SecsFormat.F4)]
        public void HsmsBody_L_GetHsmsBodyDefaultValue_Test_Return_Floats(SecsFormat format)
        {
            object? expected = Array.Empty<float>();
            var actual = HsmsBody.GetHsmsBodyDefaultValue(format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(SecsFormat.F8)]
        public void HsmsBody_L_GetHsmsBodyDefaultValue_Test_Return_Doubles(SecsFormat format)
        {
            object? expected = Array.Empty<double>();
            var actual = HsmsBody.GetHsmsBodyDefaultValue(format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void L_HsmsBody_Bytes_ConverterToValue_Test()
        {
            object? expected = default;
            var actual = HsmsBody.ConverterToValue(new byte[0], SecsFormat.L);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void A_HsmsBody_Bytes_ConverterToValue_Test()
        {
            object expected = "0123";
            var actual = HsmsBody.ConverterToValue(new byte[] { 0x30, 0x31, 0x32, 0x33 }, SecsFormat.A);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void B_HsmsBody_Bytes_ConverterToValue_Test()
        {
            var expected = new byte[] { 0x30, 0x31, 0x32, 0x33 };
            var actual = HsmsBody.ConverterToValue(new byte[] { 0x30, 0x31, 0x32, 0x33 }, SecsFormat.B);
            Assert.IsInstanceOfType<byte[]>(actual);
            CollectionAssert.AreEqual(expected, (byte[])actual);
        }

        [TestMethod]
        public void BOOLEAN_HsmsBody_Bytes_ConverterToValue_Test()
        {
            var expected = new bool[] { true, false };
            var actual = HsmsBody.ConverterToValue(new byte[] { 1, 0 }, SecsFormat.BOOLEAN);
            Assert.IsInstanceOfType<bool[]>(actual);
            CollectionAssert.AreEqual(expected, (bool[])actual);
        }

        [TestMethod]
        public void I1_HsmsBody_Bytes_ConverterToValue_Test()
        {
            var expected = new sbyte[] { -1 };
            var actual = HsmsBody.ConverterToValue(new byte[] { 0xFF }, SecsFormat.I1);
            Assert.IsInstanceOfType<sbyte[]>(actual);
            CollectionAssert.AreEqual(expected, (sbyte[])actual);
        }

        [TestMethod]
        public void I2_HsmsBody_Bytes_ConverterToValue_Test()
        {
            var expected = new short[] { -8956 };
            var actual = HsmsBody.ConverterToValue(new byte[] { 0xDD, 0x04 }, SecsFormat.I2);
            Assert.IsInstanceOfType<short[]>(actual);
            CollectionAssert.AreEqual(expected, (short[])actual);
        }

        [TestMethod]
        public void I4_HsmsBody_Bytes_ConverterToValue_Test()
        {
            var expected = new int[] { 755045120 };
            var actual = HsmsBody.ConverterToValue(new byte[] { 0x2D, 0x01, 0X13, 0X00 }, SecsFormat.I4);
            Assert.IsInstanceOfType<int[]>(actual);
            CollectionAssert.AreEqual(expected, (int[])actual);
        }

        [TestMethod]
        public void I8_HsmsBody_Bytes_ConverterToValue_Test()
        {
            var expected = new long[] { 3242894098159440640 };
            var actual = HsmsBody.ConverterToValue(new byte[] { 0x2D, 0x01, 0X13, 0X00, 0x2D, 0x01, 0X13, 0X00 }, SecsFormat.I8);
            Assert.IsInstanceOfType<long[]>(actual);
            CollectionAssert.AreEqual(expected, (long[])actual);
        }
    }
}
