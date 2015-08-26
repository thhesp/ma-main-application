using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

using WebAnalyzer.Server.MessageHandler;
using WebAnalyzer.Util;
using WebAnalyzer.Controller;


namespace WebAnalyzer.Server
{
    class WebsocketConnectionsObserver : IObserver<WebsocketConnection>
    {
        ConnectionManager _connectionManager;
        TestController _controller;

        string[] _commands = {
            "connectionRequest",
            "connectionComplete",
            "data",
            "event",
            "error"
        };

        public WebsocketConnectionsObserver(TestController controller, ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
            _controller = controller;
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

            ConnectionMessageHandler connectMsg = new ConnectionMessageHandler(connection);
            connectMsg.AddConnection += _connectionManager.On_AddConnection;

            // connectiong
            published.Where(msgIn => msgIn.command != null && (msgIn.command == "connectRequest" || msgIn.command == "connectComplete"))
                .Subscribe(connectMsg);

            // data
           published.Where(msgIn => msgIn.command != null && msgIn.command == "data")
               .Subscribe(new DataMessageHandler(_controller, connection));

            //event
           published.Where(msgIn => msgIn.command != null && msgIn.command == "event")
              .Subscribe(new DataMessageHandler(_controller, connection));

            //error
           published.Where(msgIn => msgIn.command != null && msgIn.command == "error")
             .Subscribe(new DataMessageHandler(_controller, connection));

            // fallover ==> echo
           /*published.Where(msgIn => msgIn.command == null || Array.IndexOf(_commands, msgIn.command) == -1)
               .Subscribe(new EchoMessageHandler(_connectionManager, connection));
            */
        }
    }
}
