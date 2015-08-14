using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Server;

namespace WebAnalyzer.Events
{
    public delegate void AddConnectionEventHandler(object sender, AddConnectionEvent e);

    public class AddConnectionEvent : EventArgs
    {

        private WebsocketConnection _connection;

        public AddConnectionEvent(WebsocketConnection connection)
        {
            _connection = connection;
        }

        public WebsocketConnection Connection
        {
            get { return _connection; }
        }
    }
}
