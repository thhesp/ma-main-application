using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Events;
using WebAnalyzer.Models.MessageModel;
using WebAnalyzer.Models.MessageModel.In.EventMessages;

namespace WebAnalyzer.Server.MessageHandler
{
    class EventMessageHandler : IObserver<Object>
    {
        public event AddWebpageEventHandler AddWebpage;

        readonly ConnectionManager _connectionManager;
        readonly WebsocketConnection _connection;

        public EventMessageHandler(WebsocketConnection connection)
        {
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
            if (omsgIn is URLChangeEventMessage)
            {
                processURLChangeEvent((URLChangeEventMessage)omsgIn);
            }
            else if (omsgIn is ScrollEventMessage)
            {
                processScrollEvent((ScrollEventMessage)omsgIn);
            }
            else if (omsgIn is ResizeEventMessage)
            {
                processResizeChangeEvent((ResizeEventMessage)omsgIn);
            }
        }

        private void processScrollEvent(ScrollEventMessage msg)
        {

        }

        private void processURLChangeEvent(URLChangeEventMessage msg)
        {
            AddWebpage(this, new AddWebpageEvent(msg.URL, msg.WindowWidth, msg.WindowHeight, _connection.UID));
        }

        private void processResizeChangeEvent(ResizeEventMessage msg)
        {

        }
    }
}
