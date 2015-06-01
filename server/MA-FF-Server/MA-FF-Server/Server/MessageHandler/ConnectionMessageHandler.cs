using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;

namespace WebAnalyzer.Server.MessageHandler
{
    class ConnectionMessageHandler : IObserver<Object>
    {
        readonly ConnectionManager _connectionManager;
        readonly WebsocketConnection _connection;

        public ConnectionMessageHandler(ConnectionManager connectionManager, WebsocketConnection connection)
        {
            _connectionManager = connectionManager;
            _connection = connection;
        }

        public void OnCompleted()
        {
            Console.WriteLine("Connection Message: Completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("Connection message : " + error.Message);
        }

        public void OnNext(Object omsgIn)
        {
            dynamic msgIn = omsgIn;

            Logger.Log("Message received: " + msgIn);

            // extract data from json object and handle it
            String command = msgIn.command;

            if(command.Equals("connectRequest"))
            {
                respondToHandshake();
            }
            else if(command.Equals("connectComplete"))
            {
                completeHandshake();
            }

        }

        private void respondToHandshake()
        {
            Object msg = new { command = "connectionResponse", message = "Establishing Connection...", timestamp = Timestamp.GetMillisecondsUnixTimestamp() };

            _connection.Out.OnNext(msg);
        }

        private void completeHandshake()
        {
            _connection.Established = true;

            _connectionManager.AddWebsocketConnection(_connection);
        }
    }
}
