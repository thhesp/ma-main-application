using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Util;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace WebAnalyzer.Models.MessageModel
{

    abstract public class Message
    {

        private String _timestamp;

        public Message()
        {
            
        }

        public Message(String timestamp)
        {
            _timestamp = timestamp;
        }


        public String Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        public static Message FromJson(JsonTextReader reader)
        {
            String property = string.Empty;

            Message msg;

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                        property = reader.Value.ToString();

                    if (reader.TokenType == JsonToken.String && property == "command")
                    {
                        String messageType = reader.Value.ToString();

                        Logger.Log("MessageType: " + messageType);

                        switch (messageType)
                        {
                            case "connectRequest":
                                return new ConnectionMessage(ConnectionMessage.CONNECTION_MESSAGE_TYPE.REQUEST);
                            case "connectComplete":
                                return new ConnectionMessage(ConnectionMessage.CONNECTION_MESSAGE_TYPE.COMPLETE);
                            case "data":
                                return null;
                            default:
                                return null;
                        }
                    }
                     /*   response.Message = reader.Value.ToString();

                    if (reader.TokenType == JsonToken.Integer && currentProperty == "command")
                        response.Acknowledge = (AcknowledgeType)Int32.Parse(reader.Value.ToString());

                    if (reader.TokenType == JsonToken.Integer && currentProperty == "Code")
                        response.Code = Int32.Parse(reader.Value.ToString());

                    if (reader.TokenType == JsonToken.String && currentProperty == "Message")
                        response.Message = reader.Value.ToString();

                    if (reader.TokenType == JsonToken.String && currentProperty == "Exception")
                        response.Exception = reader.Value.ToString();*/

                    // Process Rooms and other stuff
                }
                else
                {
                    // Process tracking the current nested element
                }
            }


            return (dynamic)JObject.Load(reader); ;
        }
    }
}
