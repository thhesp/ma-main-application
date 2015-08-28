using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel
{
    class ConnectionMessage : Message
    {

        /* 
         * 
         * Message Data
         * 
         */

        public enum CONNECTION_MESSAGE_TYPE { REQUEST = 0, COMPLETE = 1 };


        private CONNECTION_MESSAGE_TYPE _type;

        public ConnectionMessage(CONNECTION_MESSAGE_TYPE type)
        {
            _type = type;
        }

        public CONNECTION_MESSAGE_TYPE Type
        {
            get { return _type; }
        }
    }
}
