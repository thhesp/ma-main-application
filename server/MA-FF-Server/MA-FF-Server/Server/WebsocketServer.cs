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
    class WebsocketServer
    {

        private ConnectionManager _connManager;
        private TestController _controller;

        private WebSocketListener server;
        private CancellationTokenSource cancellation;

        public WebsocketServer(TestController controller)
        {
            _connManager = new ConnectionManager();
            _connManager.MessageSent += controller.On_MessageSent;
            _connManager.UpdateWSConnectionCount += controller.On_UpdateConnectionCount;

            _controller = controller;
        }
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

            options.NegotiationTimeout = TimeSpan.FromMilliseconds(5000);
            options.WebSocketReceiveTimeout = TimeSpan.FromMilliseconds(100);
            options.WebSocketSendTimeout = TimeSpan.FromMilliseconds(100);

            options.PingTimeout = TimeSpan.FromMilliseconds(500);

            options.UseNagleAlgorithm = false;

            server = new WebSocketListener(new IPEndPoint(IPAddress.Any, port), options);
           
            
            var rfc6455 = new vtortola.WebSockets.Rfc6455.WebSocketFactoryRfc6455(server);
            
            rfc6455.MessageExtensions.RegisterExtension(new WebSocketDeflateExtension());
            
            server.Standards.RegisterStandard(rfc6455);
           
        }

        public void start(int port)
        {
            CreateServer(port);
            prepareServer();
        }

        private void prepareServer()
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

        public void stop()
        {
            cancellation.Cancel();

            server.Stop();

            _connManager.StopMessageThread();
            
            _connManager.ResetConnections();

            server.Dispose();

            Logger.Log("Server stopped");
        }

        public void RequestData(String uniqueId, double leftX, double leftY, double rightX, double rightY)
        {
            _connManager.RequestData(uniqueId, leftX, leftY, rightX, rightY);
        }

        public void RequestData(String uniqueId, double xPos, double yPos)
        {
            _connManager.RequestData(uniqueId, xPos, yPos);
        }
    }
}
