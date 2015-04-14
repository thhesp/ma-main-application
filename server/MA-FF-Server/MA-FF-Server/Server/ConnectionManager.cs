using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private List<WebsocketConnection> _toAdd = new List<WebsocketConnection>();

        private List<WebsocketConnection> _toRemove = new List<WebsocketConnection>();

        private ConnectionManager()
        {

        }

        public void AddWebsocketConnection(WebsocketConnection connection)
        {
            _toAdd.Add(connection);
        }

        public void RemoveWebsocketConnection(WebsocketConnection connection)
        {
            _toRemove.Add(connection);
        }

        public void Broadcast(Object message)
        {
            checkConnectionQueues();

            foreach (var connection in this)
            {
                if(checkConnection(connection))
                {
                    connection.Out.OnNext(message);
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

            return connection.Established;
        }

       

    }
}
