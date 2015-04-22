using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.Base
{
    public class Message
    {
        String _timestamp;
        dynamic _messageObj;

        public Message(String timestamp, dynamic messageObj)
        {
            _timestamp = timestamp;
            _messageObj = messageObj;
        }

        public String Timestamp
        {
            get { return _timestamp; }
        }

        public dynamic MessageObj
        {
            get { return _messageObj; }
        }
    }
}
