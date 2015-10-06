using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Server;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// AddWebpageEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">AddWebpageEvent which is sent</param>
    public delegate void AddWebpageEventHandler(object sender, AddWebpageEvent e);

    /// <summary>
    /// Event which is triggered from the connection, when a new webpage gets opened or an inactive website gets active
    /// </summary>
    public class AddWebpageEvent : EventArgs
    {

        /// <summary>
        /// URL of the website
        /// </summary>
        private String _url;

        /// <summary>
        /// UID of the websocket connection
        /// </summary>
        private String _connectionUID;

        /// <summary>
        /// Timestamp on which the event happend
        /// </summary>
        private String _timestamp;

        /// <summary>
        /// Current windowWidth of the browser
        /// </summary>
        private int _windowWidth = 0;

        /// <summary>
        /// Current windowHeight of the browser
        /// </summary>
        private int _windowHeight = 0;

        /// <summary>
        /// EventConstructor
        /// </summary>
        /// <param name="url">URL of the website</param>
        /// <param name="windowWidth">Current windowWidth of the browser</param>
        /// <param name="windowHeight">Current windowHeight of the browser</param>
        /// <param name="connectionUID">UID of the websocket connection</param>
        public AddWebpageEvent(String url, int windowWidth, int windowHeight, String connectionUID)
        {
            _url = url;
            _connectionUID = connectionUID;
            _windowHeight = windowHeight;
            _windowWidth = windowWidth;
            _timestamp = Util.Timestamp.GetMillisecondsUnixTimestamp();
        }

        /// <summary>
        /// Getter for the webpage URL
        /// </summary>
        public String URL
        {
            get { return _url; }
        }

        /// <summary>
        /// UID of the websocket connection
        /// </summary>
        public String ConnectionUID
        {
            get { return _connectionUID; }
        }

        /// <summary>
        /// Getter for timestamp on which the new webpage was added
        /// </summary>
        public String Timestamp
        {
            get { return _timestamp; }
        }

        /// <summary>
        /// Getter of the browser window width
        /// </summary>
        public int WindowWidth
        {
            get { return _windowWidth; }
        }

        /// <summary>
        /// Getter of the browser window height
        /// </summary>
        public int WindowHeight
        {
            get { return _windowHeight; }
        }
    }
}
