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

        private int _windowWidth = 0;
        private int _windowHeight = 0;

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

        public int WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; }
        }

        public int WindowHeight
        {
            get { return _windowHeight; }
            set { _windowHeight = value; }
        }
    }
}
