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
    /// <summary>
    /// Class which is processes all messages on all connections
    /// </summary>
    class WebsocketConnectionsObserver : IObserver<WebsocketConnection>
    {
        /// <summary>
        /// Reference to the connection manager
        /// </summary>
        ConnectionManager _connectionManager;

        /// <summary>
        /// Reference to the testcontroller
        /// </summary>
        TestController _controller;

        /// <summary>
        /// Constructor with reference to the testcontroller and the connection manager
        /// </summary>
        /// <param name="controller">Reference to the testcontroller</param>
        /// <param name="connectionManager">Reference to the connection manager</param>
        public WebsocketConnectionsObserver(TestController controller, ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
            _controller = controller;
        }

        /// <summary>
        /// Method which gets called once the processing of the message is completed.
        /// </summary>
        public void OnCompleted()
        {
            Logger.Log("Connection Observer completed");
        }

        /// <summary>
        /// Method which gets called if an error occurs while processing the message.
        /// </summary>
        /// <param name="error">Errorexception which occured</param>
        public void OnError(Exception error)
        {
            //error on connection timeout from here?
            Logger.Log("Connection Observer error: " + error.Message);
        }

        /// <summary>
        /// Method used for processing all messages on the connections
        /// </summary>
        /// <param name="connection">The connection which received the message</param>
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
