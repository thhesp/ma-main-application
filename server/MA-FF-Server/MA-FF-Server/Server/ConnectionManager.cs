using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Linq;

using WebAnalyzer.Events;

using WebAnalyzer.Util;
using WebAnalyzer.Models.MessageModel;

namespace WebAnalyzer.Server
{
    public class ConnectionManager
    {
        private Boolean _workMessages = false;

        private List<WebsocketConnection> _connections = new List<WebsocketConnection>();

        public ConnectionManager()
        {
            
        }

        public void ResetConnections()
        {
            _connections.Clear();
        }

        public void StartMessageThread()
        {
            _workMessages = true;
            var period = TimeSpan.FromMilliseconds(Properties.Settings.Default.WSMessageDelay);
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
            lock (_connections)
            {
                _connections.RemoveAll(connection => connection.IsConnected == false);
                foreach (var connection in _connections)
                {
                    connection.EnqueueMessage(message);
                }
            }

        }

        private void WorkConnectionMessageQueues()
        {
            lock (_connections)
            {
                _connections.RemoveAll(connection => connection.IsConnected == false);
                //Logger.Log("Working through queues... ");
                foreach (var connection in _connections)
                {
                    connection.workMessageQueue();
                }
            }
        }

        public void On_AddConnection(object source, AddConnectionEvent e)
        {
            lock (_connections)
            {
                _connections.Add(e.Connection);
            }

            Logger.Log("Current connection count: " + _connections.Count);
        }
    }
}
