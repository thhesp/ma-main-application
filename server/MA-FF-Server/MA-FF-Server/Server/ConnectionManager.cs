﻿using System;
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
        public event MessageSentEventHandler MessageSent;
        public event UpdateWSConnectionCountEventHandler UpdateWSConnectionCount;

        private Boolean _workMessages = false;

        private List<WebsocketConnection> _connections = new List<WebsocketConnection>();

        private StopwatchTimer timer;

        private int _lastConnectionCount = 0;

        public ConnectionManager()
        {
        }

        private void UpdateConnectionCount()
        {
            if (_lastConnectionCount != _connections.Count)
            {
                _lastConnectionCount = _connections.Count;
                UpdateWSConnectionCount(this, new UpdateWSConnectionCountEvent(_lastConnectionCount));
            }
            
        }

        public void ResetConnections()
        {
            _connections.Clear();
        }

        public void StartMessageThread()
        {
            _workMessages = true;
            timer = new StopwatchTimer(Properties.Settings.Default.WSMessageDelay, WorkConnectionMessageQueues);
            timer.Start();
        }

        public void StopMessageThread()
        {
            _workMessages = false;
            if (timer != null)
            {
                timer.Stop();
            }
           
        }

        public void RequestData(String uniqueId, double leftX, double leftY, double rightX, double rightY)
        {
            DataMessage message = new DataMessage(Timestamp.GetMillisecondsUnixTimestamp());

            message.SetMessageData(uniqueId, leftX, leftY, rightX, rightY);

            message.MessageSent += MessageSent;

            this.Broadcast(message);
        }

        public void RequestData(String uniqueId, double xPos, double yPos)
        {
            SmallDataMessage message = new SmallDataMessage(Timestamp.GetMillisecondsUnixTimestamp());

            message.SetMessageData(uniqueId, xPos, yPos);

            message.MessageSent += MessageSent;

            this.Broadcast(message);
        }

        private void Broadcast(Message message)
        {
            lock (_connections)
            {
                _connections.RemoveAll(connection => connection.IsConnected == false);
                UpdateConnectionCount();
                Parallel.ForEach(_connections, connection => connection.EnqueueMessage(message));
            }

        }

        private void WorkConnectionMessageQueues()
        {
            if (_workMessages)
            {
                lock (_connections)
                {
                    _connections.RemoveAll(connection => connection.IsConnected == false);
                    UpdateConnectionCount();
                    Parallel.ForEach(_connections, connection => connection.workMessageQueue());
                }
            }
        }

        public void On_AddConnection(object source, AddConnectionEvent e)
        {
            lock (_connections)
            {
                _connections.Add(e.Connection);
                e.Connection.Active = true;
                UpdateConnectionCount();
            }

            Logger.Log("Current connection count: " + _connections.Count);
        }
    }
}
