using System;
using System.Threading.Tasks;
using System.Net;   
using System.Threading;
using vtortola.WebSockets;

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
            server.Start();
            acceptingTask = Task.Run(() => AcceptWebSocketClients(server, cancellation.Token));
            Logger.Log("WS Socket Server started");
        }

        public void stop()
        {
            server.Stop();
            cancellation.Cancel();
            acceptingTask.Wait();
            Logger.Log("Server stopped");
        }

        static async Task AcceptWebSocketClients(WebSocketListener server, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var ws = await server.AcceptWebSocketAsync(token).ConfigureAwait(false);
                    if (ws == null)
                        continue;
                    Task.Run(() => HandleConnectionAsync(ws, token));
                }
                catch (Exception aex)
                {
                    var ex = aex.GetBaseException();
                    Logger.Log("Error Accepting client: " + ex.GetType().Name + ": " + ex.Message);
                }
            }
        }


        static async Task HandleConnectionAsync(WebSocket ws, CancellationToken cancellation)
        {
            try
            {
                IWebSocketLatencyMeasure l = ws as IWebSocketLatencyMeasure;
                while (ws.IsConnected && !cancellation.IsCancellationRequested)
                {
                    String msg = await ws.ReadStringAsync(cancellation).ConfigureAwait(false);
                    if (msg == null)
                        continue;
                    ws.WriteString(msg + " --- Echo from Server");
                    Logger.Log("Message from Client: " + msg);
                }
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception aex)
            {
                Logger.Log("Error Handling connection: " + aex.GetBaseException().Message);
                try { ws.Close(); }
                catch { }
            }
            finally
            {
                ws.Dispose();
            }
        }
    }
}
