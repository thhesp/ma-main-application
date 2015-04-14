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

        private ConnectionManager()
        {

        }

        public void Broadcast(Object message)
        {
            foreach (var connection in this)
            {
                if(connection.Established)
                {
                    connection.Out.OnNext(message);
                }
            }
        }

    }
}
