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
using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Server
{
    public class WebsocketConnection : UIDBase
    {
        public IObservable<Message> In { get; set; }
        public IObserver<String> Out { get; set; }

        readonly WebSocket _ws;

        private List<Message> _messageQueue;

        private String _uid;

        private Boolean _active = false;

        private Boolean _writing = false;

        public WebsocketConnection(WebSocket ws) : base()
        {
            _ws = ws;
            _messageQueue = new List<Message>();
        }

        ~WebsocketConnection() {
            _active = false;
            _messageQueue.Clear();

            _ws.Close();
            _ws.Dispose();
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

        public List<Message> MessageQueue
        {
            get { return _messageQueue; }
        }

        public void EnqueueMessage(Message message)
        {
            lock (_messageQueue)
            {
                this.MessageQueue.Add(message);
            }
            
        }

        public void workMessageQueue()
        {
            if (_messageQueue.Count == 0)
                return;

            lock (MessageQueue)
            {
                //check if messages are too old and remove them
                removeOldMessages();

                if (_messageQueue.Count == 0 || Writing|| !this.IsConnected)
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

                _messageQueue.Remove(msg);

                Writing = false;
            }
            
        }

        private void removeOldMessages()
        {
            String currentTimestamp = Timestamp.GetMillisecondsUnixTimestamp();
            _messageQueue.RemoveAll(msg => ((long.Parse(msg.Timestamp) - long.Parse(currentTimestamp)) > Properties.Settings.Default.DataTimeout));
        }
    }
}
