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
    /// <summary>
    /// The websocket connection class
    /// </summary>
    public class WebsocketConnection : UIDBase
    {
        /// <summary>
        /// Messages which are received on this connection
        /// </summary>
        public IObservable<Message> In { get; set; }

        /// <summary>
        /// Messages which are sent over this connection
        /// </summary>
        public IObserver<String> Out { get; set; }

        /// <summary>
        /// The websocket
        /// </summary>
        readonly WebSocket _ws;

        /// <summary>
        /// The list of messages which need to be sent
        /// </summary>
        private List<Message> _messageQueue;

        /// <summary>
        /// Flag if the connection is active
        /// </summary>
        private Boolean _active = false;

        /// <summary>
        /// Flag if currently a message is being sent
        /// </summary>
        private Boolean _writing = false;


        /// <summary>
        /// Constructor for the connection
        /// </summary>
        /// <param name="ws">The websocket</param>
        public WebsocketConnection(WebSocket ws) : base()
        {
            Logger.Log("Creating new WS Conncetion");
            _ws = ws;
            _messageQueue = new List<Message>();
        }

        /// <summary>
        /// Destructor 
        /// </summary>
        ~WebsocketConnection() {
            _active = false;
            _messageQueue.Clear();

            _ws.Close();
            _ws.Dispose();
        }

        /// <summary>
        /// Checks if the websocket is connected
        /// </summary>
        public Boolean IsConnected
        {
            get 
            {
                return _ws.IsConnected;
            }
        }

        /// <summary>
        /// Getter/Setter if the connection is currently sending a message
        /// </summary>
        public Boolean Writing
        {
            get { return _writing; }
            set { _writing = value; }
        }

        /// <summary>
        /// Getter/Setter if the connection is currently active
        /// </summary>
        public Boolean Active
        {
            get { return _active; }
            set { _active = value; }
        }

        /// <summary>
        /// The list of messages which still need to be sent
        /// </summary>
        public List<Message> MessageQueue
        {
            get { return _messageQueue; }
        }

        /// <summary>
        /// Adds the given message to the list
        /// </summary>
        /// <param name="message">Message to be added</param>
        public void EnqueueMessage(Message message)
        {
            lock (_messageQueue)
            {
                this.MessageQueue.Add(message);
            }
            
        }

        /// <summary>
        /// Tries to send the next message from the list
        /// </summary>
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

        /// <summary>
        /// Removes all messages which are to old.
        /// </summary>
        private void removeOldMessages()
        {
            String currentTimestamp = Timestamp.GetMillisecondsUnixTimestamp();
            _messageQueue.RemoveAll(msg => ((long.Parse(msg.Timestamp) - long.Parse(currentTimestamp)) > Properties.Settings.Default.DataTimeout));
        }
    }
}
