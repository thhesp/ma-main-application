using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;
using WebAnalyzer.Models.MessageModel;

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

            EchoMessage message = new EchoMessage(Timestamp.GetMillisecondsUnixTimestamp(), omsgIn);
            _connection.EnqueueMessage(message);
        }
    }
}
