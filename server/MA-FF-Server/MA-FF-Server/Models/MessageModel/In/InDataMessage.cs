using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Util;
using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.MessageModel
{
    public class InDataMessage : DataMessage
    {
        public enum MESSAGE_TYPE { SMALL = 0, NORMAL = 1 };


        /* 
         * 
         * Message Data
         * 
         */

        private MESSAGE_TYPE _type = MESSAGE_TYPE.SMALL;

        private bool _error = false;

        private String _serverReceived;

        private String _clientReceived;

        private String _clientSent;

        private String _serverSent;

        private String _url;

        private DOMElementModel _leftElement;
        private DOMElementModel _rightElement;

        public InDataMessage()
        {
            _serverReceived = Util.Timestamp.GetMillisecondsUnixTimestamp();
        }

        public InDataMessage(String _timestamp)
            : base(_timestamp)
        {
            _serverReceived = Util.Timestamp.GetMillisecondsUnixTimestamp();
        }

        public MESSAGE_TYPE Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public Boolean Error
        {
            get { return _error; }
            set { _error = value; }
        }

        public String URL
        {
            get { return _url; }
            set { _url = value; }
        }

        public String ServerReceived
        {
            get { return _serverReceived; }
            set { _serverReceived = value; }
        }

        public String ServerSent
        {
            get { return _serverSent; }
            set { _serverSent = value; }
        }

        public String ClientReceived
        {
            get { return _clientReceived; }
            set { _clientReceived = value; }
        }

        public String ClientSent
        {
            get { return _clientSent; }
            set { _clientSent = value; }
        }

        public DOMElementModel LeftElement
        {
            get { return _leftElement; }
            set { _leftElement = value; }
        }

        public DOMElementModel RightElement
        {
            get { return _rightElement; }
            set { _rightElement = value; }
        }
    }
}
