namespace Secs.Enums
{
    /// <summary>
    /// HSMS连接状态
    /// </summary>
    public enum ConnectionState
    {
        /// <summary>
        /// The entity is ready to listen for or initiate a TCP/IP connection, 
        /// but no connection has been established yet, 
        /// or all previously established TCP/IP connections have been terminated.
        /// </summary>
        NotConnected,
        /// <summary>
        /// A TCP/IP connection has been established. 
        /// This state has two substates: NOT SELECTED and SELECTED.
        /// </summary>
        Connected,
        /// <summary>
        /// No HSMS session has been established, 
        /// or any previously established HSMS session has ended.
        /// </summary>
        NotSelected,
        /// <summary>
        /// At least one HSMS session has been established. 
        /// This is the normal operating state of HSMS: data messages can be exchanged in this state.
        /// </summary>
        Selected,
    }
}
