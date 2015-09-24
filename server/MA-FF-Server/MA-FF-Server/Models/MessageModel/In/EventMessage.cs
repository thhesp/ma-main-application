using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Models.MessageModel.In.EventMessages;
using WebAnalyzer.Util;

namespace WebAnalyzer.Models.MessageModel
{
    public abstract class EventMessage : Message
    {

        /* 
         * 
         * Message Data
         * 
         */

        public static URLChangeEventMessage URLChangeEventMessageFromJson(JsonTextReader reader)
        {
            Logger.Log("URL Change message found!");

            String property = string.Empty;

            URLChangeEventMessage msg = new URLChangeEventMessage();

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                        property = reader.Value.ToString();

                    if (property == "url" && reader.TokenType == JsonToken.String)
                    {
                        msg.URL = reader.Value.ToString();
                    }
                    else if (property == "height" && reader.TokenType == JsonToken.Integer)
                    {
                        msg.WindowHeight = int.Parse(reader.Value.ToString());
                    }
                    else if (property == "width" && reader.TokenType == JsonToken.Integer)
                    {
                        msg.WindowWidth = int.Parse(reader.Value.ToString());
                    }
                }
            }

            return msg;
        }

        public static URLChangeEventMessage ScrollEventMessageFromJson(JsonTextReader reader)
        {
            //Logger.Log("Scroll event message found!");


            String property = string.Empty;

            URLChangeEventMessage msg = new URLChangeEventMessage();

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                        property = reader.Value.ToString();

                }
            }

            return msg;
        }

        public static URLChangeEventMessage ResizeEventMessageFromJson(JsonTextReader reader)
        {
            //Logger.Log("resize event message found!");


            String property = string.Empty;

            URLChangeEventMessage msg = new URLChangeEventMessage();

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                        property = reader.Value.ToString();


                }
            }

            return msg;
        }
    }
}
