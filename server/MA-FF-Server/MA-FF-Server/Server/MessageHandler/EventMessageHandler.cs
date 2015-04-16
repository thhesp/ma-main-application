using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Server.MessageHandler
{
    class EventMessageHandler : IObserver<Object>
    {
        readonly ConnectionManager _connectionManager;
        readonly WebsocketConnection _connection;

        public EventMessageHandler(ConnectionManager connectionManager, WebsocketConnection connection)
        {
            _connectionManager = connectionManager;
            _connection = connection;
        }

        public void OnCompleted()
        {
            Console.WriteLine("Event Message: Completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("Event message : " + error.Message);
        }

        public void OnNext(Object omsgIn)
        {
            dynamic msgIn = omsgIn;



            
        }

        private void processClickEvent(Object msg)
        {

        }

        private void processScrollEvent(Object msg)
        {

        }
    }
}
