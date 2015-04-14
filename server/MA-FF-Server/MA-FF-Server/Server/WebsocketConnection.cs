using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using vtortola.WebSockets;

namespace WebAnalyzer.Server
{
    public class WebsocketConnection
    {

        public IObservable<dynamic> In { get; set; }
        public IObserver<dynamic> Out { get; set; }

        readonly WebSocket _ws;

        private Boolean _established = false;

        public WebsocketConnection(WebSocket ws)
        {
            _ws = ws;
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


    }
}
