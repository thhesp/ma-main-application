using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using vtortola.WebSockets;
using WebAnalyzer.Models.MessageModel;
using WebAnalyzer.Util;
using WebAnalyzer.Events;

namespace WebAnalyzer.Server
{
    public class WebsocketConnection
    {
        public IObservable<Message> In { get; set; }
        public IObserver<String> Out { get; set; }

        readonly WebSocket _ws;

        private Boolean _established = false;

        private ConcurrentQueue<Message> _messageQueue;

        private Boolean _active = false;

        private Boolean _writing = false;

        public WebsocketConnection(WebSocket ws)
        {
            _ws = ws;
            _messageQueue = new ConcurrentQueue<Message>();
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

        public Boolean Writing
        {
            get { return _writing; }
            set { _writing = value; }
        }

        public Boolean Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public ConcurrentQueue<Message> MessageQueue
        {
            get { return _messageQueue; }
        }

        public void EnqueueMessage(Message message)
        {
            this.MessageQueue.Enqueue(message);
        }

        public void workMessageQueue()
        {
            //check if messages are too old and remove them
            removeOldMessages();

            if (Writing || !Active || !this.Established || !this.IsConnected || _messageQueue.Count == 0)
                return;

            //Logger.Log("Sent Message... current Message Count: " + _messageQueue.Count);

            //sent first message (later in try catch block)
            Message msg = _messageQueue.ElementAt(0);
            Writing = true;

            if (msg is SmallDataMessage)
            {
                this.Out.OnNext(((SmallDataMessage)msg).ToJson());
            }
            else if (msg is DataMessage)
            {
                this.Out.OnNext(((DataMessage)msg).ToJson());
            }
            else
            {
                Logger.Log("Message type unknown!");
                return;
            }

            Message outMessage;
            _messageQueue.TryDequeue(out outMessage);

            Writing = false;
        }

        private void removeOldMessages()
        {
            if (_messageQueue.Count == 0)
                return;

            Message msg = _messageQueue.ElementAt(0);
            String currentTimestamp = Timestamp.GetMillisecondsUnixTimestamp();
            while ((long.Parse(currentTimestamp) - long.Parse(msg.Timestamp)) > Properties.Settings.Default.DataTimeout)
            {
                // remove first element
                Message outMessage;
                _messageQueue.TryDequeue(out outMessage);
                if (_messageQueue.Count > 0)
                {
                    // get new first element
                    msg = _messageQueue.ElementAt(0);
                }
                else
                {
                    break;
                }

            }
        }
    }
}
