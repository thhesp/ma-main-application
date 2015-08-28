using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Util;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using WebAnalyzer.Models.DataModel;

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

                                return DataMessageFromJson(reader);
                            default:
                                return null;
                        }
                    }
                }
            }


            return null;
        }

        private static Message DataMessageFromJson(JsonTextReader reader)
        {
            //@TODO: ERROR checking + complex messages
            String property = string.Empty;

            Boolean attributes = false;

            List<String> attributeList = new List<String>();

            Boolean element = false;

            DOMElementModel leftElement = new DOMElementModel();
            DOMElementModel rightElement = null;

            //complex messages

            Boolean left = false;
            Boolean right = false;


            InDataMessage msg = new InDataMessage();

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    //get property ==> name of field
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        property = reader.Value.ToString();
                        continue;
                    }

                    if (attributes && reader.TokenType == JsonToken.String)
                    {
                        leftElement.AddAttribute(property, reader.Value.ToString());
                    }
                    else if (element && (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float))
                    {
                        Double value = Double.Parse(reader.Value.ToString());

                        switch (property)
                        {
                            case "top":
                                leftElement.Top = value;
                                break;
                            case "left":
                                leftElement.Left = value;
                                break;
                            case "width":
                                leftElement.Width = value;
                                break;
                            case "height":
                                leftElement.Height = value;
                                break;
                            case "outerWidth":
                                leftElement.OuterWidth = value;
                                break;
                            case "outerHeight":
                                leftElement.OuterHeight = value;
                                break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.String)
                    {

                        String value = reader.Value.ToString();
                        switch (property)
                        {
                            case "uniqueid":
                                msg.UniqueID = value;
                                break;
                            case "datarequest":
                                msg.RequestTimestamp = value;
                                break;
                            case "serversent":
                                msg.ServerSent = value;
                                break;
                            case "clientreceived":
                                msg.ClientReceived = value;
                                break;
                            case "clientsent":
                                msg.ClientSent = value;
                                break;
                            case "url":
                                msg.URL = value;
                                break;
                            case "tag":
                                leftElement.Tag = value;
                                break;

                        }
                    }
                    else if ((reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float))
                    {
                        Double value = Double.Parse(reader.Value.ToString());

                        switch (property)
                        {
                            case "x":
                                msg.LeftX = value;
                                break;
                            case "y":
                                msg.LeftY = value;
                                break;
                            case "clientreceived":
                                msg.ClientReceived = value.ToString();
                                break;
                            case "clientsent":
                                msg.ClientSent = value.ToString();
                                break;
                        }
                    }
                }
                else
                {
                    // Process tracking the current nested element

                    if (property == "attributes" && reader.TokenType == JsonToken.StartArray)
                    {
                        attributes = true;
                    }

                    if (property == "element" && reader.TokenType == JsonToken.StartObject)
                    {
                        element = true;
                    }


                    if (attributes && reader.TokenType == JsonToken.EndArray)
                    {
                        attributes = false;
                    }

                    if (element && reader.TokenType == JsonToken.EndObject)
                    {
                        element = false;
                    }

                }
            }

            if (rightElement == null)
            {
                rightElement = leftElement;
            }

            msg.LeftElement = leftElement;
            msg.RightElement = rightElement;

            return msg;
        }
    }

}
