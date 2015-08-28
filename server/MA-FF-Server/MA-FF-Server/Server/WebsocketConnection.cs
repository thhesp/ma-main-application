using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using vtortola.WebSockets;
using WebAnalyzer.Models.MessageModel;
using WebAnalyzer.Util;

namespace WebAnalyzer.Server
{
    public class WebsocketConnection
    {

        private static int TIMEOUT = 500;

        public IObservable<dynamic> In { get; set; }
        public IObserver<String> Out { get; set; }

        readonly WebSocket _ws;

        private Boolean _established = false;

        private ConcurrentQueue<Message> _messageQueue;

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

            if (!this.Established || !this.IsConnected || _messageQueue.Count == 0)
                return;

            Logger.Log("Sent Message... current Message Count: " + _messageQueue.Count);

            //sent first message (later in try catch block)
            Message msg = _messageQueue.ElementAt(0);

            try
            {

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
            }
            catch (WebSocketException e)
            {
                Logger.Log("Tried writing, while there still was a message been written: " + e.Message);
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
