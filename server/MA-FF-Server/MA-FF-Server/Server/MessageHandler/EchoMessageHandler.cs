using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;
using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Server.MessageHandler
{
    class EchoMessageHandler : IObserver<Object>
    {
        readonly ConnectionManager _connectionManager;
        readonly WebsocketConnection _connection;

        public EchoMessageHandler(ConnectionManager connectionManager, WebsocketConnection connection)
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
            //_connection.Out.OnNext(omsgIn);

            Message message = new Message(Timestamp.GetMillisecondsUnixTimestamp(), omsgIn);
            _connection.EnqueueMessage(message);
        }
    }
}
