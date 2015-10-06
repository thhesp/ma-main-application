using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// MessageSentEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">MessageSentEvent which is sent</param>
    public delegate void MessageSentEventHandler(object sender, MessageSentEvent e);

    /// <summary>
    /// Event which informs the testcontroller that the message for this gaze was sent.
    /// </summary>
    public class MessageSentEvent : EventArgs
    {
        /// <summary>
        /// UID of the gaze to which the message belongs
        /// </summary>
        private String _uid;

        /// <summary>
        /// Timestamp on which the message was sent
        /// </summary>
        private String _sentTimestamp;

        /// <summary>
        /// EventConstructor
        /// </summary>
        /// <param name="uid">UID of the gaze to which the message belongs</param>
        public MessageSentEvent(String uid)
        {
            _uid = uid;
            _sentTimestamp = Util.Timestamp.GetMillisecondsUnixTimestamp();
        }

        /// <summary>
        /// Getter of the gaze UID
        /// </summary>
        public String UID
        {
            get { return _uid; }
        }

        /// <summary>
        /// Timestamp on which the message was sent
        /// </summary>
        public String SentTimestamp
        {
            get { return _sentTimestamp; }
        }
    }
}
