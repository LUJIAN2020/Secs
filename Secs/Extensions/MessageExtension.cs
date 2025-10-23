using Secs.Messages;

namespace Secs.Extensions
{
    public static class MessageExtension
    {
        public static string ToSml(this HsmsMessage message)
        {
            return HsmsMessage.ConverterToSml(message);
        }
        public static string ToSmlBody(this HsmsBody body)
        {
            return HsmsBody.ConverterToSml(body);
        }
        public static HsmsBody ToHsmsBody(this string smlBody)
        {
            return new HsmsBody(smlBody);
        }
        public static byte[] ToBytes(this HsmsHeader header)
        {
            return HsmsHeader.ConverterToBytes(header);
        }
        public static byte[] ToBytes(this HsmsBody body)
        {
            return HsmsBody.ConverterToBytes(body);
        }
        public static byte[] ToBytes(this HsmsMessage message)
        {
            return HsmsMessage.ConverterToBytes(message);
        }
    }
}
