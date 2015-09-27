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

        protected String _url;

        protected String _eventTimestamp;

        public String URL
        {
            get { return _url; }
            set { _url = value; }
        }

        public String EventTimestamp
        {
            get { return _eventTimestamp; }
            set { _eventTimestamp = value; }
        }

        /* 
         * 
         * Message Data
         * 
         */

        public static URLChangeEventMessage URLChangeEventMessageFromJson(JsonTextReader reader)
        {
            //Logger.Log("URL Change message found!");

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

        public static ScrollEventMessage ScrollEventMessageFromJson(JsonTextReader reader)
        {
            //Logger.Log("Scroll event message found!");

            String property = string.Empty;

            ScrollEventMessage msg = new ScrollEventMessage();

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
                    else if (property == "scrollx" && reader.TokenType == JsonToken.Integer)
                    {
                        msg.ScrollX = int.Parse(reader.Value.ToString());
                    }
                    else if (property == "scrolly" && reader.TokenType == JsonToken.Integer)
                    {
                        msg.ScrollY = int.Parse(reader.Value.ToString());
                    }
                    else if (property == "eventtimestamp" && reader.TokenType == JsonToken.Integer)
                    {
                        msg.EventTimestamp = reader.Value.ToString();
                    }
                }
            }

            return msg;
        }

        public static ResizeEventMessage ResizeEventMessageFromJson(JsonTextReader reader)
        {
            //Logger.Log("resize event message found!");

            String property = string.Empty;

            ResizeEventMessage msg = new ResizeEventMessage();

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
                    else if (property == "eventtimestamp" && reader.TokenType == JsonToken.Integer)
                    {
                        msg.EventTimestamp = reader.Value.ToString();
                    }
                }
            }

            return msg;
        }
    }
}
