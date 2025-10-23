using Secs.Enums;
using System;

namespace Secs
{
    public class HsmsOptions
    {
        /// <summary>
        /// DeviceId
        /// </summary>
        public ushort SessionId { get; set; }

        /// <summary>
        /// Configure connection as Active or Passive mode.
        /// </summary>
        public ConnectionMode ConnectionMode { get; set; }

        /// <summary>
        /// When <see cref="IsActive"/> is <see langword="true"/> the IP address will be treated remote device's IP address, 
        /// opposite the connection will bind on this IP address as Passive mode.
        /// Default value is "127.0.0.1".
        /// </summary>
        public string IP { get; set; } = "127.0.0.1";

        /// <summary>
        /// When <see cref="IsActive"/> is <see langword="true"/> the port number will be treated remote device's TCP port number, 
        /// opposite the connection will bind on the port number as Passive mode.
        /// Default value is 5000.
        /// </summary>
        public int Port { get; set; } = 5000;

        /// <summary>
        /// Configure the timer interval in milliseconds between each <see cref="MessageType.LinkTestRequest"/> request.
        /// Default value is 60000.
        /// </summary>
        public int LinkTestInterval { get; set; } = 60000;

        /// <summary>
        /// Whether to perform link tests
        /// </summary>
        public bool LinkTest { get; set; } = false;

        private int t3 = 45 * 1000;
        /// <summary>
        /// Reply timeout. Specifies maximum amount of time an entity expecting a reply message will wait for that reply.
        /// </summary>
        public int T3
        {
            get { return t3; }
            set
            {
                if (value < 1 * 1000 || value > 120 * 1000)
                    throw new Exception("T3 value range is 1-120 second");

                t3 = value;
            }
        }

        private int t5 = 10 * 1000;
        /// <summary>
        /// Connection Separation Timeout. Specifies the amount of time which must elapse between successive attempts to connect to a given remote entity.
        /// </summary>
        public int T5
        {
            get { return t5; }
            set
            {
                if (value < 1 * 1000 || value > 240 * 1000)
                    throw new Exception("T5 value range is 1-240 second");

                t5 = value;
            }
        }

        private int t6 = 5 * 1000;
        /// <summary>
        /// Control Transaction Timeout. Specifies the time which a control transaction may remain open before it is considered a communications failure.
        /// </summary>
        public int T6
        {
            get { return t6; }
            set
            {
                if (value < 1 * 1000 || value > 240 * 1000)
                    throw new Exception("T6 value range is 1-240 second");

                t6 = value;
            }
        }

        private int t7 = 10 * 1000;
        /// <summary>
        /// Time which a TCP/IP connection can remain in NOT SELECTED state(i.e., no HSMS activity) before it isonsidered a communications failure.
        /// </summary>
        public int T7
        {
            get { return t7; }
            set
            {
                if (value < 1 * 1000 || value > 240 * 1000)
                    throw new Exception("T7 value range is 1-240 second");

                t7 = value;
            }
        }

        private int t8 = 5 * 1000;
        /// <summary>
        /// Maximum time between successive bytes of a single  
        /// HSMS message which may expire before it is considered a communications failure.
        /// </summary>
        public int T8
        {
            get { return t8; }
            set
            {
                if (value < 1 * 1000 || value > 240 * 1000)
                    throw new Exception("T8 value range is 1-120 second");

                t8 = value;
            }
        }
    }
}
