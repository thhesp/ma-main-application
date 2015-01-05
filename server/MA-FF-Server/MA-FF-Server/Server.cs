using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using vtortola.WebSockets.Deflate;
using vtortola.WebSockets;

namespace Server
{
    class Server
    {

        private static Server instance;

        public static Server getInstance()
        {
            if (instance == null)
            {
                instance = new Server();
            }

            return instance;
        }


        private WebSocketListener server;
        private CancellationTokenSource cancellation;
        private Task acceptingTask;

        private Server()
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
        }

        public void stop()
        {

            Log("Server stoping");
            server.Stop();
            cancellation.Cancel();
            acceptingTask.Wait();
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
                    Log("Error Accepting client: " + ex.GetType().Name + ": " + ex.Message);
                }
            }
            Log("Server Stop accepting clients");
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
                }
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception aex)
            {
                Log("Error Handling connection: " + aex.GetBaseException().Message);
                try { ws.Close(); }
                catch { }
            }
            finally
            {
                ws.Dispose();
            }
        }

        static void Log(String line)
        {
            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyy hh:mm:ss.fff ") + line);
        }
    }
}
