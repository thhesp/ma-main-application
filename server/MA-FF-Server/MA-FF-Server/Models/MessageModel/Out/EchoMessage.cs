using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel
{
    /// <summary>
    /// Outgoing echo message (not used currently and outdated)
    /// </summary>
    class EchoMessage : Message
    {
        /// <summary>
        /// Origial message data
        /// </summary>
        dynamic _origMsg;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_timestamp">Receive timestamp of the original message</param>
        /// <param name="origMsg">Original message</param>
        public EchoMessage(String _timestamp, dynamic origMsg) : base(_timestamp)
        {
            _origMsg = origMsg;
        }

        /// <summary>
        /// Getter for the message
        /// </summary>
        public dynamic MessageObj
        {
            get { return _origMsg; }
        }
    }
}
