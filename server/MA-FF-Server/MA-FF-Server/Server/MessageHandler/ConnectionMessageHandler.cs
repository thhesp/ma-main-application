using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;

using WebAnalyzer.Events;

namespace WebAnalyzer.Server.MessageHandler
{
    class ConnectionMessageHandler : IObserver<Object>
    {
        public event AddConnectionEventHandler AddConnection;

        readonly WebsocketConnection _connection;

        public ConnectionMessageHandler(WebsocketConnection connection)
        {
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
            //Object msg = new { command = "connectionResponse" };

            String msg = "{ \"command\":\"connectionResponse\" }";

            _connection.Out.OnNext(msg);
        }

        private void completeHandshake()
        {
            _connection.Established = true;

            AddConnection(this, new AddConnectionEvent(_connection));
        }
    }
}
