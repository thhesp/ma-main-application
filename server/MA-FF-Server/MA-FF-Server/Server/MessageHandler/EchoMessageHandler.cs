using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;
using WebAnalyzer.Models.MessageModel;

namespace WebAnalyzer.Server.MessageHandler
{
    /// <summary>
    /// MessageHandler for echo messages
    /// </summary>
    class EchoMessageHandler : IObserver<Object>
    {
        /// <summary>
        /// Readonly reference to the connection manager
        /// </summary>
        readonly ConnectionManager _connectionManager;

        /// <summary>
        /// Readonly reference to the websocket connection
        /// </summary>
        readonly WebsocketConnection _connection;

        /// <summary>
        /// Constructor with the websocket connection and the connectionManager
        /// </summary>
        /// <param name="connectionManager">The connectionManager</param>
        /// <param name="connection">The connection which received the message</param>
        public EchoMessageHandler(ConnectionManager connectionManager, WebsocketConnection connection)
        {
            _connectionManager = connectionManager;
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
            Console.WriteLine("Connection message : " + error.Message);
        }

        /// <summary>
        /// Method used for processing the message
        /// </summary>
        /// <param name="omsgIn">The Message which was received</param>
        public void OnNext(Object omsgIn)
        {
            //_connection.Out.OnNext(omsgIn);

            EchoMessage message = new EchoMessage(Timestamp.GetMillisecondsUnixTimestamp(), omsgIn);
            _connection.EnqueueMessage(message);
        }
    }
}
