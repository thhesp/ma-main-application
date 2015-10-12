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
    /// <summary>
    /// Connection Manager which holds a list of all current connections.
    /// </summary>
    /// <remarks>
    /// Used for requesting data and managing all connections.
    /// </remarks>
    public class ConnectionManager
    {
        /// <summary>
        /// Handler which is used when a message was sent
        /// </summary>
        public event MessageSentEventHandler MessageSent;

        /// <summary>
        /// Handler used for triggering an update of the connection count
        /// </summary>
        public event UpdateWSConnectionCountEventHandler UpdateWSConnectionCount;

        /// <summary>
        /// Used for indicating if messages need to be sent
        /// </summary>
        private Boolean _workMessages = false;

        /// <summary>
        /// List of all open websocket connections
        /// </summary>
        private List<WebsocketConnection> _connections = new List<WebsocketConnection>();

        /// <summary>
        /// Timer for the message sending thread.
        /// </summary>
        private StopwatchTimer timer;

        /// <summary>
        /// Count of the last connection count
        /// </summary>
        /// <remarks>
        /// Used for checking if an update in the ui is necessary or not.
        /// </remarks>
        private int _lastConnectionCount = 0;

        /// <summary>
        /// Checks if the connection count needs to be updated and if triggers the update event.
        /// </summary>
        private void UpdateConnectionCount()
        {
            if (_lastConnectionCount != _connections.Count)
            {
                _lastConnectionCount = _connections.Count;
                UpdateWSConnectionCount(this, new UpdateWSConnectionCountEvent(_lastConnectionCount));
            }

        }

        /// <summary>
        /// clears the list of connections
        /// </summary>
        public void ResetConnections()
        {
            _connections.Clear();
        }

        /// <summary>
        /// starts the message sending thread
        /// </summary>
        public void StartMessageThread()
        {
            _workMessages = true;
            timer = new StopwatchTimer(Properties.Settings.Default.WSMessageDelay, WorkConnectionMessageQueues);
            timer.Start();
        }

        /// <summary>
        /// stops the message sending thread
        /// </summary>
        public void StopMessageThread()
        {
            _workMessages = false;
            if (timer != null)
            {
                timer.Stop();
            }

        }

        /// <summary>
        /// Creates an message for requesting the given data
        /// </summary>
        /// <param name="uniqueId">UniqueID of the gaze</param>
        /// <param name="leftX">X coordinate for the left eye</param>
        /// <param name="leftY">Y coordinate for the left eye</param>
        /// <param name="rightX">X coordinate for the right eye</param>
        /// <param name="rightY">Y coordinate for the right eye</param>
        public void RequestData(String uniqueId, double leftX, double leftY, double rightX, double rightY)
        {
            DataMessage message = new DataMessage(Timestamp.GetMillisecondsUnixTimestamp());

            message.SetMessageData(uniqueId, leftX, leftY, rightX, rightY);

            message.MessageSent += MessageSent;

            this.Broadcast(message);
        }

        /// <summary>
        /// Creates an small message for requesting the given data
        /// </summary>
        /// <param name="uniqueId">UniqueID of the gaze</param>
        /// <param name="leftX">X coordinate</param>
        /// <param name="leftY">Y coordinate</param>
        public void RequestData(String uniqueId, double xPos, double yPos)
        {
            SmallDataMessage message = new SmallDataMessage(Timestamp.GetMillisecondsUnixTimestamp());

            message.SetMessageData(uniqueId, xPos, yPos);

            message.MessageSent += MessageSent;

            this.Broadcast(message);
        }

        /// <summary>
        /// Enqueues the data for all actives connections (should only be one)
        /// </summary>
        /// <param name="message">The message to broadcast</param>
        private void Broadcast(Message message)
        {
            RemoveOldConnections();
            lock (_connections)
            {
                //encqueue only to active message
                Parallel.ForEach(_connections.Where(connection => connection.Active), connection => connection.EnqueueMessage(message));
            }

        }

        /// <summary>
        /// Triggers the sending of a message for all connections
        /// </summary>
        private void WorkConnectionMessageQueues()
        {
            if (_workMessages)
            {
                RemoveOldConnections();
                lock (_connections)
                {
                    Parallel.ForEach(_connections, connection => connection.workMessageQueue());
                }
            }
        }

        /// <summary>
        /// removes all connections which aren't connected anymore
        /// </summary>
        private void RemoveOldConnections()
        {
            IEnumerable<WebsocketConnection> oldConnections;
            lock (_connections)
            {
                 oldConnections = _connections.Where(connection => !connection.IsConnected);

                if(oldConnections.Count() == 0)
                    return;
                _connections.RemoveAll(connection => oldConnections.Contains(connection));
            }

            UpdateConnectionCount();
        }

        /// <summary>
        /// Adds the given connection to the list of connections
        /// </summary>
        /// <param name="source">Event sender</param>
        /// <param name="e">Event data with the connection</param>
        public void On_AddConnection(object source, AddConnectionEvent e)
        {
            lock (_connections)
            {
                _connections.Add(e.Connection);
            }
            e.Connection.Active = true;
            UpdateConnectionCount();

            Logger.Log("Current connection count: " + _connections.Count);
        }
    }
}
