using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets.Deflate;
using vtortola.WebSockets;

namespace Server
{


    // Testing the WebSocket Server

    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/

            CancellationTokenSource cancellation = new CancellationTokenSource();

            var server = new WebSocketListener(new IPEndPoint(IPAddress.Any, 8888));
            var rfc6455 = new vtortola.WebSockets.Rfc6455.WebSocketFactoryRfc6455(server);
            server.Standards.RegisterStandard(rfc6455);
            server.Start();


            var acceptingTask = Task.Run(() => AcceptWebSocketClients(server, cancellation.Token));
            while(19 != Console.Read());
            Log("Server stoping");
            server.Stop();
            cancellation.Cancel();
            acceptingTask.Wait();
            while (20 != Console.Read()) ;
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
                    ws.WriteString(msg+" --- Echo from Server");
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
