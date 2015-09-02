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
using WebAnalyzer.Models.MessageModel;

namespace WebAnalyzer.Server
{
    public static class WsExtensions
    {

        public static async Task<Message> ReadDynamicAsync(this WebSocket ws, CancellationToken cancel)
        {
            var message = await ws.ReadMessageAsync(cancel);
            if (message != null)
            {
                using (var sr = new StreamReader(message, Encoding.UTF8))
                {
                    return Message.FromJson(new JsonTextReader(sr));
                }
            }
            else
            {
                return null;
            }
                

        }

        public static void WriteString(this WebSocket ws, String data)
        {
            if (ws.IsConnected)
            {
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
}
