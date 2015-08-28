using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;

using WebAnalyzer.Util;

namespace WebAnalyzer.Server
{
    public static class WsExtensions
    {

        public static async Task<dynamic> ReadDynamicAsync(this WebSocket ws, CancellationToken cancel)
        {
            var message = await ws.ReadMessageAsync(cancel);
            if (message != null)
            {
                using (var sr = new StreamReader(message, Encoding.UTF8))
                {
                    return (dynamic)JObject.Load(new JsonTextReader(sr));
                }
            }
            else
                return null;

        }

        public static void WriteDynamic(this WebSocket ws, String data)
        {
            //JsonSerializer serializer = new JsonSerializer();
            using (var writer = ws.CreateMessageWriter(WebSocketMessageType.Text))
            {
                using (var sw = new StreamWriter(writer, Encoding.UTF8))
                {
                    sw.WriteAsync(data);
                }
            }       
        }

    }
}
