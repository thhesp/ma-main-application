using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;
using System.Reactive.Subjects;
using vtortola.WebSockets.Deflate;

using System.Security.Cryptography.X509Certificates;
using WebAnalyzer.Util;
using WebAnalyzer.Controller;
using WebAnalyzer.Models.MessageModel;
using WebAnalyzer.Events;

namespace WebAnalyzer.Server
{
    /// <summary>
    /// The interface to the websocket server
    /// </summary>
    class WebsocketServer
    {

        /// <summary>
        /// Reference to the connection manager
        /// </summary>
        private ConnectionManager _connManager;

        /// <summary>
        /// Reference to the testcontroller
        /// </summary>
        private TestController _controller;

        /// <summary>
        /// The "real" websocket server
        /// </summary>
        private WebSocketListener server;

        /// <summary>
        /// Cancellation token to stop the server
        /// </summary>
        private CancellationTokenSource cancellation;

        /// <summary>
        /// Constructor which creates the connection manager with necessary event listeners
        /// </summary>
        /// <param name="controller">Reference to the testcontroller</param>
        public WebsocketServer(TestController controller)
        {
            _connManager = new ConnectionManager();
            _connManager.MessageSent += controller.On_MessageSent;
            _connManager.UpdateWSConnectionCount += controller.On_UpdateConnectionCount;

            _controller = controller;
        }

        /// <summary>
        /// Creates a reference to a websocket server which runs on the given port
        /// </summary>
        /// <param name="port">Port for the websocket server</param>
        private void CreateServer(int port)
        {
            if (server != null)
            {
                stop();
            }

            Logger.Log("Creating Websocket-Server on Port :" + port);

            cancellation = new CancellationTokenSource();


            // https://github.com/vtortola/WebSocketListener/wiki/WebSocketListener-options
            
              WebSocketListenerOptions options = new WebSocketListenerOptions();

            options.NegotiationTimeout = TimeSpan.FromMilliseconds(Properties.Settings.Default.WebsocketNegotiationTimeout);
            options.WebSocketReceiveTimeout = TimeSpan.FromMilliseconds(Properties.Settings.Default.WebsocketReceiveTimeout);
            options.WebSocketSendTimeout = TimeSpan.FromMilliseconds(Properties.Settings.Default.WebsocketSentTimeout);

            options.PingTimeout = TimeSpan.FromMilliseconds(Properties.Settings.Default.WebsocketPingTimeout);

            options.UseNagleAlgorithm = Properties.Settings.Default.WebsocketUseNagle;

            server = new WebSocketListener(new IPEndPoint(IPAddress.Any, port), options);
           
            
            var rfc6455 = new vtortola.WebSockets.Rfc6455.WebSocketFactoryRfc6455(server);
            
            rfc6455.MessageExtensions.RegisterExtension(new WebSocketDeflateExtension());
            
            server.Standards.RegisterStandard(rfc6455);
           
        }

        /// <summary>
        /// Starts the websocket server with the given port
        /// </summary>
        /// <param name="port">The port to run the websocket server</param>
        public void start(int port)
        {
            CreateServer(port);
            StartServer();
        }

        /// <summary>
        /// Starts the websocket server and the message working thread
        /// </summary>
        private void StartServer()
        {
            WebsocketConnectionsObserver messagesObserver = new WebsocketConnectionsObserver(_controller, _connManager);

            server.Start();

            Observable.FromAsync(server.AcceptWebSocketAsync)
                .Select(ws => new WebsocketConnection(ws)
                {
                    In = Observable.FromAsync<Message>(ws.ReadDynamicAsync)
                                                .DoWhile(() => ws.IsConnected)
                                                .Where(msg => msg != null),

                    Out = Observer.Create<String>(ws.WriteString)
                })
                .DoWhile(() => server.IsStarted && !cancellation.IsCancellationRequested)
                .Subscribe(messagesObserver);

            _connManager.StartMessageThread();

            Logger.Log("WS Socket Server started");
        }

        /// <summary>
        /// Stops the websocket server and the message thread
        /// </summary>
        public void stop()
        {
            cancellation.Cancel();

            server.Stop();

            _connManager.StopMessageThread();
            
            _connManager.ResetConnections();

            server.Dispose();

            Logger.Log("Server stopped");
        }

        /// <summary>
        /// Used for requesting data
        /// </summary>
        /// <param name="uniqueId">UniqueID of the gaze</param>
        /// <param name="leftX">X coordinate for the left eye</param>
        /// <param name="leftY">Y coordinate for the left eye</param>
        /// <param name="rightX">X coordinate for the right eye</param>
        /// <param name="rightY">Y coordinate for the right eye</param>
        public void RequestData(String uniqueId, double leftX, double leftY, double rightX, double rightY)
        {
            _connManager.RequestData(uniqueId, leftX, leftY, rightX, rightY);
        }

        /// <summary>
        /// Used for requesting data
        /// </summary>
        /// <param name="uniqueId">UniqueID of the gaze</param>
        /// <param name="xPos">X coordinate</param>
        /// <param name="yPos">Y coordinate</param>
        public void RequestData(String uniqueId, double xPos, double yPos)
        {
            _connManager.RequestData(uniqueId, xPos, yPos);
        }
    }
}
