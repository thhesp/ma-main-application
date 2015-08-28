using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel
{
    using Newtonsoft.Json;
    using System.IO;
    using Util = Util;

    abstract public class Message
    {
        String _timestamp;
        

        public Message(String timestamp)
        {
            _timestamp = timestamp;
        }


        public String Timestamp
        {
            get { return _timestamp; }
        }
    }
}
