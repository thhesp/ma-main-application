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
    /// <summary>
    /// MessageHandler for Event messages
    /// </summary>
    class EventMessageHandler : IObserver<Object>
    {
        /// <summary>
        /// Eventhandler for the AddWebpage Event
        /// </summary>
        public event AddWebpageEventHandler AddWebpage;

        /// <summary>
        /// Eventhandler for the AddBrowserEvent Event
        /// </summary>
        public event AddBrowserEventHandler AddBrowserEvent;

        /// <summary>
        /// Readonly reference to the websocket connection
        /// </summary>
        readonly WebsocketConnection _connection;

        /// <summary>
        /// Constructor with the websocket connection
        /// </summary>
        /// <param name="connection">The connection which received the message</param>
        public EventMessageHandler(WebsocketConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Method which gets called once the processing of the message is completed.
        /// </summary>
        public void OnCompleted()
        {
            Console.WriteLine("Event Message: Completed");
        }

        /// <summary>
        /// Method which gets called if an error occurs while processing the message.
        /// </summary>
        /// <param name="error">Errorexception which occured</param>
        public void OnError(Exception error)
        {
            Console.WriteLine("Event message : " + error.Message);
        }

        /// <summary>
        /// Method used for processing the message
        /// </summary>
        /// <param name="omsgIn">The Message which was received</param>
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

        /// <summary>
        /// Processes scroll events
        /// </summary>
        /// <param name="msg">The scroll event message</param>
        private void processScrollEvent(ScrollEventMessage msg)
        {
            ScrollEventModel eventModel = new ScrollEventModel(msg.ScrollX, msg.ScrollY, Util.Timestamp.GetMillisecondsUnixTimestamp());

            eventModel.URL = msg.URL;

            eventModel.EventTimestamp = msg.EventTimestamp;

            AddBrowserEvent(this, new AddBrowserEvent(eventModel, _connection.UID));
        }


        /// <summary>
        /// Processes url change events
        /// </summary>
        /// <param name="msg">The url change event message</param>
        private void processURLChangeEvent(URLChangeEventMessage msg)
        {
            AddWebpage(this, new AddWebpageEvent(msg.URL, msg.WindowWidth, msg.WindowHeight, _connection.UID));
        }


        /// <summary>
        /// Processes resize events
        /// </summary>
        /// <param name="msg">The resize event message</param>
        private void processResizeChangeEvent(ResizeEventMessage msg)
        {
            ResizeEventModel eventModel = new ResizeEventModel(msg.WindowHeight, msg.WindowWidth, Util.Timestamp.GetMillisecondsUnixTimestamp());

            eventModel.URL = msg.URL;

            eventModel.EventTimestamp = msg.EventTimestamp;

            AddBrowserEvent(this, new AddBrowserEvent(eventModel, _connection.UID));
        }
    }
}
