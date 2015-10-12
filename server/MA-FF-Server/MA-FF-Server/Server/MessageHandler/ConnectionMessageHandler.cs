using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;

using WebAnalyzer.Events;
using WebAnalyzer.Models.MessageModel;

namespace WebAnalyzer.Server.MessageHandler
{
    /// <summary>
    /// MessageHandler for Connection messages
    /// </summary>
    class ConnectionMessageHandler : IObserver<Object>
    {
        /// <summary>
        /// Eventhandler for the AddConnection Event
        /// </summary>
        public event AddConnectionEventHandler AddConnection;

        /// <summary>
        /// Eventhandler for the AddWebpage Event
        /// </summary>
        public event AddWebpageEventHandler AddWebpage;

        /// <summary>
        /// Readonly reference to the websocket connection
        /// </summary>
        readonly WebsocketConnection _connection;

        /// <summary>
        /// Constructor with the websocket connection
        /// </summary>
        /// <param name="connection">The connection which received the message</param>
        public ConnectionMessageHandler(WebsocketConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Method which gets called once the processing of the message is completed.
        /// </summary>
        public void OnCompleted()
        {
            Console.WriteLine("Connection Message: Completed");
        }

        /// <summary>
        /// Method which gets called if an error occurs while processing the message.
        /// </summary>
        /// <param name="error">Errorexception which occured</param>
        public void OnError(Exception error)
        {
            Console.WriteLine("Connectionmessage error: " + error.Message);
        }

        /// <summary>
        /// Method used for processing the message
        /// </summary>
        /// <param name="omsgIn">The Message which was received</param>
        public void OnNext(Object omsgIn)
        {
            ConnectionMessage msgIn = (ConnectionMessage) omsgIn;

            Logger.Log("Connectionmessage received: " + msgIn.Type);

            if (msgIn.Type == ConnectionMessage.CONNECTION_MESSAGE_TYPE.REQUEST)
            {
                respondToHandshake();
            }
            else if (msgIn.Type == ConnectionMessage.CONNECTION_MESSAGE_TYPE.COMPLETE)
            {
                AddWebpage(this, new AddWebpageEvent(msgIn.URL, msgIn.WindowWidth, msgIn.WindowHeight, _connection.UID));
                completeHandshake();
            }

        }

        /// <summary>
        /// Sends the respond to the handshake over the connection
        /// </summary>
        private void respondToHandshake()
        {
            //Object msg = new { command = "connectionResponse" };

            String msg = "{ \"command\":\"connectionResponse\" }";

            _connection.Out.OnNext(msg);
        }

        /// <summary>
        /// Triggers the event which adds the message to the connection.
        /// </summary>
        private void completeHandshake()
        {
            AddConnection(this, new AddConnectionEvent(_connection));
        }
    }
}
