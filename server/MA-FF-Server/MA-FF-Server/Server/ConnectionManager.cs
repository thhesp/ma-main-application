using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Linq;

using WebAnalyzer.Util;
using WebAnalyzer.Models.MessageModel;

namespace WebAnalyzer.Server
{
    class ConnectionManager : List<WebsocketConnection>
    {

        private static ConnectionManager instance;

        public static ConnectionManager getInstance()
        {
            if (instance == null)
            {
                instance = new ConnectionManager();
            }

            return instance;
        }

        private static int WORK_DELAY = 10;

        private Boolean _workMessages = false;

        private List<WebsocketConnection> _toAdd = new List<WebsocketConnection>();

        private List<WebsocketConnection> _toRemove = new List<WebsocketConnection>();

        private ConnectionManager()
        {
            
        }

        public void ResetConnections()
        {
            this.Clear();
            _toAdd.Clear();
            _toRemove.Clear();
        }

        public void StartMessageThread()
        {
            _workMessages = true;
            var period = TimeSpan.FromMilliseconds(WORK_DELAY);
            var observable = Observable.Interval(period);

            observable.Subscribe(i => {
                if(_workMessages){
                    WorkConnectionMessageQueues();
                }
            });
        }

        public void StopMessageThread()
        {
            _workMessages = false;
        }
        public void AddWebsocketConnection(WebsocketConnection connection)
        {
            _toAdd.Add(connection);
        }

        public void RemoveWebsocketConnection(WebsocketConnection connection)
        {
            _toRemove.Add(connection);
        }

        public void RequestData(String uniqueId, double leftX, double leftY, double rightX, double rightY)
        {
            DataMessage message = new DataMessage(Timestamp.GetMillisecondsUnixTimestamp());

            message.SetMessageData(uniqueId, leftX, leftY, rightX, rightY);

            this.Broadcast(message);
        }

        public void RequestData(String uniqueId, double xPos, double yPos)
        {
            SmallDataMessage message = new SmallDataMessage(Timestamp.GetMillisecondsUnixTimestamp());

            message.SetMessageData(uniqueId, xPos, yPos);

            this.Broadcast(message);
        }

        private void Broadcast(Message message)
        {
            checkConnectionQueues();

            foreach (var connection in this)
            {
                if(checkConnection(connection))
                {
                    //connection.Out.OnNext(message);

                    connection.EnqueueMessage(message);
                }
            }
        }

        private void checkConnectionQueues()
        {
            foreach(var connection in _toAdd)
            {
                this.Add(connection);
            }

            foreach(var connection in _toRemove)
            {
                this.Remove(connection);
            }

            _toAdd.Clear();
            _toRemove.Clear();
        }

        private Boolean checkConnection(WebsocketConnection connection)
        {
            if (!connection.IsConnected)
            {
                this.RemoveWebsocketConnection(connection);
                return false;
            }

            return true;
        }

        private void WorkConnectionMessageQueues()
        {
            //Logger.Log("Working through queues... ");
            foreach (var connection in this)
            {
                connection.workMessageQueue();
            }
        }
    }
}
