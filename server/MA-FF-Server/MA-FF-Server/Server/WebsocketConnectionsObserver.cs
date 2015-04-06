using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;

namespace WebAnalyzer.Server
{
    class WebsocketConnectionsObserver : IObserver<WebsocketConnection>
    {

        public WebsocketConnectionsObserver()
        {

        }

        public void OnCompleted()
        {
            Logger.Log("Connection completed");
        }

        public void OnError(Exception error)
        {
            Logger.Log("Connection error: " + error.Message);
        }

        public void OnNext(WebsocketConnection message)
        {

        }
    }
}
