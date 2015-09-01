using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    public delegate void MessageSentEventHandler(object sender, MessageSentEvent e);

    public class MessageSentEvent : EventArgs
    {
        private String _uid;

        private String _sentTimestamp;

        public MessageSentEvent(String uid)
        {
            _uid = uid;
            _sentTimestamp = Util.Timestamp.GetMillisecondsUnixTimestamp();
        }

        public String UID
        {
            get { return _uid; }
        }

        public String SentTimestamp
        {
            get { return _sentTimestamp; }
        }
    }
}
