using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Server;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// AddConnectionEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">AddConnectionEvent which is sent</param>
    public delegate void AddConnectionEventHandler(object sender, AddConnectionEvent e);

    /// <summary>
    /// Informs the ConnectionManager that a new connection has to be added to the connection queue.
    /// </summary>
    public class AddConnectionEvent : EventArgs
    {
        /// <summary>
        /// The WebSocketConnection
        /// </summary>
        private WebsocketConnection _connection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection">The new Websocket connection</param>
        public AddConnectionEvent(WebsocketConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Getter for the Connection
        /// </summary>
        public WebsocketConnection Connection
        {
            get { return _connection; }
        }
    }
}
