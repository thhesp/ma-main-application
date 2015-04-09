using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

using WebAnalyzer.Server.MessageHandler;
using WebAnalyzer.Util;


namespace WebAnalyzer.Server
{
    class WebsocketConnectionsObserver : IObserver<WebsocketConnection>
    {
        ConnectionManager _connectionManager;

        public WebsocketConnectionsObserver(ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public void OnCompleted()
        {
            Logger.Log("Connection completed");
        }

        public void OnError(Exception error)
        {
            Logger.Log("Connection error: " + error.Message);
        }

        public void OnNext(WebsocketConnection connection)
        {
            var published = connection.In.Publish().RefCount();


            Logger.Log("Message received?");

            // connectiong
            published.Where(msgIn => msgIn.command != null && msgIn.command == "connect")
                .Subscribe(new ConnectionMessageHandler(_connectionManager, connection));

            // data
           published.Where(msgIn => msgIn.command != null && msgIn.command == "data")
               .Subscribe(new DataMessageHandler(_connectionManager, connection));

            // fallover
           published.Where(msgIn => msgIn.command == null || (msgIn.command != "connect" && msgIn.commadn != "data"))
               .Subscribe(new EchoMessageHandler(_connectionManager, connection));
        }
    }
}
