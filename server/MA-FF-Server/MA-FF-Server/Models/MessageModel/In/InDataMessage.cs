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
    /// <summary>
    /// Model for incoming data messages (answers to outgoing data messages aka request)
    /// </summary>
    public class InDataMessage : Message
    {
        /// <summary>
        /// Data Message Types
        /// </summary>
        public enum MESSAGE_TYPE { SMALL = 0, NORMAL = 1 };


        /* 
         * 
         * Message Data
         * 
         */

        /// <summary>
        /// Data Message Type
        /// </summary>
        private MESSAGE_TYPE _type = MESSAGE_TYPE.SMALL;

        /// <summary>
        /// Server received timestamp
        /// </summary>
        private String _serverReceived;

        /// <summary>
        /// Client received timestamp
        /// </summary>
        private String _clientReceived;

        /// <summary>
        /// Client sent timestamp
        /// </summary>
        private String _clientSent;

        /// <summary>
        /// Webpage url
        /// </summary>
        private String _url;

        /// <summary>
        /// UniqueID of message / gaze
        /// </summary>
        private String _uniqueId;

        /// <summary>
        /// Data about the left eye
        /// </summary>
        private DOMElementModel _leftElement;

        /// <summary>
        /// Data about the right eye
        /// </summary>
        private DOMElementModel _rightElement;

        /// <summary>
        /// Constructor
        /// </summary>
        public InDataMessage()
        {
            _serverReceived = Util.Timestamp.GetMillisecondsUnixTimestamp();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_timestamp">???</param>
        public InDataMessage(String _timestamp)
            : base(_timestamp)
        {
            _serverReceived = Util.Timestamp.GetMillisecondsUnixTimestamp();
        }

        /// <summary>
        /// Getter / Setter for the message type
        /// </summary>
        public MESSAGE_TYPE Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Getter / Setter for the message/ gaze uniqueID
        /// </summary>
        public String UniqueID
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }

        /// <summary>
        /// Getter / Setter for the webpage url
        /// </summary>
        public String URL
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// Getter / Setter for the server received timestamp
        /// </summary>
        public String ServerReceived
        {
            get { return _serverReceived; }
            set { _serverReceived = value; }
        }

        /// <summary>
        /// Getter / Setter for the client received timestamp
        /// </summary>
        public String ClientReceived
        {
            get { return _clientReceived; }
            set { _clientReceived = value; }
        }

        /// <summary>
        /// Getter / Setter for the client sent timestamp
        /// </summary>
        public String ClientSent
        {
            get { return _clientSent; }
            set { _clientSent = value; }
        }

        /// <summary>
        /// Getter / Setter for the left eye
        /// </summary>
        public DOMElementModel LeftElement
        {
            get { return _leftElement; }
            set { _leftElement = value; }
        }

        /// <summary>
        /// Getter / Setter for the right eye
        /// </summary>
        public DOMElementModel RightElement
        {
            get { return _rightElement; }
            set { _rightElement = value; }
        }
    }
}
