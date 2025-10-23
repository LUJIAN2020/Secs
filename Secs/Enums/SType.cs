namespace Secs.Enums
{
    /// <summary>
    /// Session Type
    /// </summary>
    public enum SType : byte
    {
        /// <summary>
        /// data message
        /// </summary>
        DataMessage = 0,
        /// <summary>
        /// Selection Request
        /// </summary>
        SelectReq = 1,
        /// <summary>
        /// Selection Response
        /// </summary>
        SelectRsp = 2,
        /// <summary>
        /// Deselect Request
        /// </summary>
        DeselectReq = 3,
        /// <summary>
        /// Deselect Response
        /// </summary>
        DeselectRsp = 4,
        /// <summary>
        /// Link Test Request
        /// </summary>
        LinktestReq = 5,
        /// <summary>
        /// Link Test Response
        /// </summary>
        LinktestRsp = 6,
        /// <summary>
        /// Reject Response
        /// </summary>
        RejectRsp = 7,
        /// <summary>
        /// Separate Request (Disconnect Request)
        /// </summary>
        SeparateReq = 9,
    }
}
