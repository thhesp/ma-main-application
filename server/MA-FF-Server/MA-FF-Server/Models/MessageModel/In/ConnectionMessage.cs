using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel
{
    /// <summary>
    /// Model for the connection messages
    /// </summary>
    class ConnectionMessage : Message
    {

        /* 
         * 
         * Message Data
         * 
         */

        /// <summary>
        /// Enumeration of connection message types
        /// </summary>
        public enum CONNECTION_MESSAGE_TYPE { REQUEST = 0, COMPLETE = 1 };

        /// <summary>
        /// connection message type of the message
        /// </summary>
        private CONNECTION_MESSAGE_TYPE _type;

        /// <summary>
        /// URL of the webpage
        /// </summary>
        private String _url;

        /// <summary>
        /// window width
        /// </summary>
        private int _windowWidth = 0;

        /// <summary>
        /// window height
        /// </summary>
        private int _windowHeight = 0;


        /// <summary>
        /// Constructor which sets the connection message type
        /// </summary>
        /// <param name="type">connection type of the message</param>
        public ConnectionMessage(CONNECTION_MESSAGE_TYPE type)
        {
            _type = type;
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
        /// Getter for the connection message type
        /// </summary>
        public CONNECTION_MESSAGE_TYPE Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Getter/ Setter for the window width
        /// </summary>
        public int WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; }
        }

        /// <summary>
        /// Getter/ Setter for the window height
        /// </summary>
        public int WindowHeight
        {
            get { return _windowHeight; }
            set { _windowHeight = value; }
        }
    }
}
