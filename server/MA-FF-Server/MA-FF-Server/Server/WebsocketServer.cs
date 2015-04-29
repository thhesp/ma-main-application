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

using WebAnalyzer.Util;

namespace WebAnalyzer.Server
{
    class WebsocketServer
    {

        private static WebsocketServer instance;

        public static WebsocketServer getInstance()
        {
            if (instance == null)
            {
                instance = new WebsocketServer();
            }

            return instance;
        }


        private WebSocketListener server;
        private CancellationTokenSource cancellation;
        private Task acceptingTask;

        private WebsocketServer()
        {
            cancellation = new CancellationTokenSource();

            server = new WebSocketListener(new IPEndPoint(IPAddress.Any, 8888));
            var rfc6455 = new vtortola.WebSockets.Rfc6455.WebSocketFactoryRfc6455(server);
            server.Standards.RegisterStandard(rfc6455);

            
        }

        public void start()
        {
            WebsocketConnectionsObserver messagesObserver = new WebsocketConnectionsObserver(ConnectionManager.getInstance());

            server.Start();

            //acceptingTask = Task.Run(() => AcceptWebSocketClients(server, cancellation.Token));

            Observable.FromAsync(server.AcceptWebSocketAsync)
                .Select(ws => new WebsocketConnection(ws)
                {
                    In = Observable.FromAsync<dynamic>(ws.ReadDynamicAsync)
                                                .DoWhile(() => ws.IsConnected)
                                                .Where(msg => msg != null),

                    Out = Observer.Create<dynamic>(ws.WriteDynamic) 
                })
                .DoWhile(() => server.IsStarted && !cancellation.IsCancellationRequested)
                .Subscribe(messagesObserver);

            ConnectionManager.getInstance().StartMessageThread();

            Logger.Log("WS Socket Server started");
        }

        public void stop()
        {
            ConnectionManager.getInstance().StopMessageThread();
            ConnectionManager.getInstance().ResetConnections();

            server.Stop();
            cancellation.Cancel();

            Logger.Log("Server stopped");
        }
    }
}
