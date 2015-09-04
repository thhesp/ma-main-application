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

        private String _url;

        public ConnectionMessage(CONNECTION_MESSAGE_TYPE type)
        {
            _type = type;
        }

        public String URL
        {
            get { return _url; }
            set { _url = value; }
        }

        public CONNECTION_MESSAGE_TYPE Type
        {
            get { return _type; }
        }
    }
}
