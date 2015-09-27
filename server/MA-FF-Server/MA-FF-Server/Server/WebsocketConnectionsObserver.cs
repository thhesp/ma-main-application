using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

using WebAnalyzer.Server.MessageHandler;
using WebAnalyzer.Util;
using WebAnalyzer.Controller;
using WebAnalyzer.Models.MessageModel;


namespace WebAnalyzer.Server
{
    class WebsocketConnectionsObserver : IObserver<WebsocketConnection>
    {
        ConnectionManager _connectionManager;
        TestController _controller;

        public WebsocketConnectionsObserver(TestController controller, ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
            _controller = controller;
        }

        public void OnCompleted()
        {
            Logger.Log("Connection Observer completed");
        }

        public void OnError(Exception error)
        {
            //error on connection timeout from here?
            Logger.Log("Connection Observer error: " + error.Message);
        }

        public void OnNext(WebsocketConnection connection)
        {
            var published = connection.In.Publish().RefCount();

            ConnectionMessageHandler connectMsg = new ConnectionMessageHandler(connection);
            connectMsg.AddConnection += _connectionManager.On_AddConnection;
            connectMsg.AddWebpage += _controller.On_AddWebpage;

            published.Where(msgIn => msgIn != null && msgIn is ConnectionMessage)
               .Subscribe(connectMsg);

            ActivationMessageHandler activationMsg = new ActivationMessageHandler(connection);
            activationMsg.AddWebpage += _controller.On_AddWebpage;

            published.Where(msgIn => msgIn != null && msgIn is ActivationMessage)
                 .Subscribe(activationMsg);

            // data
            published.Where(msgIn => msgIn != null && msgIn is InDataMessage)
               .Subscribe(new DataMessageHandler(_controller, connection));

            //error
            published.Where(msgIn => msgIn != null && msgIn is ErrorMessage)
              .Subscribe(new ErrorMessageHandler(_controller, connection));

            EventMessageHandler eventMsg = new EventMessageHandler(connection);
            eventMsg.AddWebpage += _controller.On_AddWebpage;
            eventMsg.AddBrowserEvent += _controller.On_AddBrowserEvent;

            //event
            published.Where(msgIn => msgIn != null && msgIn is EventMessage)
              .Subscribe(eventMsg);
            
        }
    }
}
