using Secs.Enums;
using Secs.Messages;

namespace Secs.Test
{
    [TestClass]
    public sealed class HsmsHeader_Test
    {
        [TestMethod]
        public void SelectReq_HsmsHeader_ToBytes_Test()
        {
            var header = new HsmsHeader(0x1534, 0, 0, SType.SelectReq, 0x2100f4F7);
            var actual = HsmsHeader.ConverterToBytes(header);
            var expected = new byte[] { 0x15, 0x34, 0x00, 0x00, 0x00, 0x01, 0x21, 0x00, 0xF4, 0xF7 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SelectRsp_HsmsHeader_ToBytes_Test()
        {
            var header = new HsmsHeader(0x1534, 0, 0, SType.SelectRsp, 0x2100f4F7);
            var actual = HsmsHeader.ConverterToBytes(header);
            var expected = new byte[] { 0x15, 0x34, 0x00, 0x00, 0x00, 0x02, 0x21, 0x00, 0xF4, 0xF7 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DeselectReq_HsmsHeader_ToBytes_Test()
        {
            var header = new HsmsHeader(0x1534, 0, 0, SType.DeselectReq, 0x2100f4F7);
            var actual = HsmsHeader.ConverterToBytes(header);
            var expected = new byte[] { 0x15, 0x34, 0x00, 0x00, 0x00, 0x03, 0x21, 0x00, 0xF4, 0xF7 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DeselectRsp_HsmsHeader_ToBytes_Test()
        {
            var header = new HsmsHeader(0x1534, 0, 0, SType.DeselectRsp, 0x2100f4F7);
            var actual = HsmsHeader.ConverterToBytes(header);
            var expected = new byte[] { 0x15, 0x34, 0x00, 0x00, 0x00, 0x04, 0x21, 0x00, 0xF4, 0xF7 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LinktestReq_HsmsHeader_ToBytes_Test()
        {
            var header = new HsmsHeader(0x1534, 0, 0, SType.LinktestReq, 0x2100f4F7);
            var actual = HsmsHeader.ConverterToBytes(header);
            var expected = new byte[] { 0x15, 0x34, 0x00, 0x00, 0x00, 0x05, 0x21, 0x00, 0xF4, 0xF7 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LinktestRsp_HsmsHeader_ToBytes_Test()
        {
            var header = new HsmsHeader(0x1534, 0, 0, SType.LinktestRsp, 0x2100f4F7);
            var actual = HsmsHeader.ConverterToBytes(header);
            var expected = new byte[] { 0x15, 0x34, 0x00, 0x00, 0x00, 0x06, 0x21, 0x00, 0xF4, 0xF7 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RejectRsp_HsmsHeader_ToBytes_Test()
        {
            var header = new HsmsHeader(0x1534, 0, 0, SType.RejectRsp, 0x2100f4F7);
            var actual = HsmsHeader.ConverterToBytes(header);
            var expected = new byte[] { 0x15, 0x34, 0x00, 0x00, 0x00, 0x07, 0x21, 0x00, 0xF4, 0xF7 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SeparateReq_HsmsHeader_ToBytes_Test()
        {
            var header = new HsmsHeader(0x1534, 0, 0, SType.SeparateReq, 0x2100f4F7);
            var actual = HsmsHeader.ConverterToBytes(header);
            var expected = new byte[] { 0x15, 0x34, 0x00, 0x00, 0x00, 0x09, 0x21, 0x00, 0xF4, 0xF7 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DataMessage_Reply_HsmsHeader_ToBytes_Test()
        {
            var header = new HsmsHeader(0x1534, 1, 1, SType.DataMessage, 0x2100f4F7, true);
            var actual = HsmsHeader.ConverterToBytes(header);
            var expected = new byte[] { 0x15, 0x34, 0x81, 0x01, 0x00, 0x00, 0x21, 0x00, 0xF4, 0xF7 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DataMessage_Not_Reply_HsmsHeader_ToBytes_Test()
        {
            var header = new HsmsHeader(0x1534, 1, 1, SType.DataMessage, 0x2100f4F7, false);
            var actual = HsmsHeader.ConverterToBytes(header);
            var expected = new byte[] { 0x15, 0x34, 0x01, 0x01, 0x00, 0x00, 0x21, 0x00, 0xF4, 0xF7 };
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
