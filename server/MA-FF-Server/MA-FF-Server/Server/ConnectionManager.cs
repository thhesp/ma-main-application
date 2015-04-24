using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Linq;

using WebAnalyzer.Util;
using WebAnalyzer.Models.Base;

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

        private static int WORK_DELAY = 15;

        private Boolean _workMessages = false;

        private List<WebsocketConnection> _toAdd = new List<WebsocketConnection>();

        private List<WebsocketConnection> _toRemove = new List<WebsocketConnection>();

        private ConnectionManager()
        {
            
        }

        public void StartMessageThread()
        {
            _workMessages = true;
            var o = Observable.Start(() =>
            {
                //This starts on a background thread.
                WorkConnectionMessageQueues();
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

        public void RequestData(double xPos, double yPos)
        {
            Object msg = new { command = "request", x = xPos, y = yPos, serversent = Timestamp.GetMillisecondsUnixTimestamp() };

            Message message = new Message(Timestamp.GetMillisecondsUnixTimestamp(), msg);

            this.Broadcast(message);
        }

        public void RequestData(int xPos, int yPos)
        {
            this.RequestData((double)xPos, (double)yPos);
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
            while (_workMessages)
            {
                Logger.Log("Working through the queues...");
                foreach (var connection in this)
                {
                    connection.workMessageQueue();
                }
                Thread.Sleep(ConnectionManager.WORK_DELAY);
            }
        }
    }
}
