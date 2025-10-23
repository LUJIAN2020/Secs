namespace Secs.Enums
{
    /// <summary>
    /// Data Format
    /// </summary>
    public enum SecsFormat : byte
    {
        /// <summary>
        /// List / Array
        /// </summary>
        L = 0b_0000_0000,

        /// <summary>
        /// Binary
        /// </summary>
        B = 0b_0010_0000,

        /// <summary>
        /// Boolean
        /// </summary>
        BOOLEAN = 0b_0010_0100,

        /// <summary>
        /// ASCII
        /// </summary>
        A = 0b_0100_0000,

        /// <summary>
        /// sbyte
        /// </summary>
        I1 = 0b_0110_0100,

        /// <summary>
        /// short (Int16)
        /// </summary>
        I2 = 0b_0110_1000,

        /// <summary>
        /// int (Int32)
        /// </summary>
        I4 = 0b_0111_0000,

        /// <summary>
        /// long (Int64)
        /// </summary>
        I8 = 0b_0110_0000,

        /// <summary>
        /// byte (UInt8)
        /// </summary>
        U1 = 0b_1010_0100,

        /// <summary>
        /// ushort (UInt16)
        /// </summary>
        U2 = 0b_1010_1000,

        /// <summary>
        /// uint (UInt32)
        /// </summary>
        U4 = 0b_1011_0000,

        /// <summary>
        /// ulong (UInt64)
        /// </summary>
        U8 = 0b_1010_0000,

        /// <summary>
        /// float (Single)
        /// </summary>
        F4 = 0b_1001_0000,

        /// <summary>
        /// double (Double)
        /// </summary>
        F8 = 0b_1000_0000,
    }
}
