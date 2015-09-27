using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Events;
using WebAnalyzer.Models.EventModel;
using WebAnalyzer.Models.MessageModel;
using WebAnalyzer.Models.MessageModel.In.EventMessages;

namespace WebAnalyzer.Server.MessageHandler
{
    class EventMessageHandler : IObserver<Object>
    {
        public event AddWebpageEventHandler AddWebpage;
        public event AddBrowserEventHandler AddBrowserEvent;

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
            ScrollEventModel eventModel = new ScrollEventModel(msg.ScrollX, msg.ScrollY, Util.Timestamp.GetMillisecondsUnixTimestamp());

            eventModel.URL = msg.URL;

            eventModel.EventTimestamp = msg.EventTimestamp;

            AddBrowserEvent(this, new AddBrowserEvent(eventModel, _connection.UID));
        }

        private void processURLChangeEvent(URLChangeEventMessage msg)
        {
            AddWebpage(this, new AddWebpageEvent(msg.URL, msg.WindowWidth, msg.WindowHeight, _connection.UID));
        }

        private void processResizeChangeEvent(ResizeEventMessage msg)
        {
            ResizeEventModel eventModel = new ResizeEventModel(msg.WindowHeight, msg.WindowWidth, Util.Timestamp.GetMillisecondsUnixTimestamp());

            eventModel.URL = msg.URL;

            eventModel.EventTimestamp = msg.EventTimestamp;

            AddBrowserEvent(this, new AddBrowserEvent(eventModel, _connection.UID));
        }
    }
}
