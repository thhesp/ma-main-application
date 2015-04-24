using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using vtortola.WebSockets;
using WebAnalyzer.Models.Base;
using WebAnalyzer.Util;

namespace WebAnalyzer.Server
{
    public class WebsocketConnection
    {

        private static int TIMEOUT = 2000;

        public IObservable<dynamic> In { get; set; }
        public IObserver<dynamic> Out { get; set; }

        readonly WebSocket _ws;

        private Boolean _established = false;

        private Queue<Message> _messageQueue;

        public WebsocketConnection(WebSocket ws)
        {
            _ws = ws;
            _messageQueue = new Queue<Message>();
        }

        public Boolean Established
        {
            get
            {

                return _established;
            }

            set
            {
                _established = value;
            }
        }

        public Boolean IsConnected
        {
            get 
            {
                return _ws.IsConnected;
            }
        }

        public Queue<Message> MessageQueue
        {
            get { return _messageQueue; }
        }

        public void EnqueueMessage(Message message)
        {
            this.MessageQueue.Enqueue(message);
        }

        public void workMessageQueue()
        {
            if(!this.Established || !this.IsConnected || _messageQueue.Count == 0)
                return;

            //check if messages are too old and remove them
            removeOldMessages();

            //sent first message (later in try catch block)
            Message msg = _messageQueue.ElementAt(0);
            try
            {
                this.Out.OnNext(msg.MessageObj);
                _messageQueue.Dequeue();
            }
            catch (WebSocketException e)
            {
                Logger.Log("Tried writing, while there still was a message been written.");
            }
            
        }

        private void removeOldMessages()
        {
            if (_messageQueue.Count == 0)
                return;

            Message msg = _messageQueue.ElementAt(0);
            String currentTimestamp = Timestamp.GetMillisecondsUnixTimestamp();
            while ((long.Parse(currentTimestamp) - long.Parse(msg.Timestamp)) > WebsocketConnection.TIMEOUT)
            {
                // remove first element
                _messageQueue.Dequeue();

                // get new first element
                msg = _messageQueue.ElementAt(0);
            }
        }
    }
}
