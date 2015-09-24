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
using WebAnalyzer.Events;

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

                        //Logger.Log("MessageType: " + messageType);

                        switch (messageType)
                        {
                            case "connectRequest":
                                return ConnectionMessageFromJson(ConnectionMessage.CONNECTION_MESSAGE_TYPE.REQUEST, reader);
                            case "connectComplete":
                                return ConnectionMessageFromJson(ConnectionMessage.CONNECTION_MESSAGE_TYPE.COMPLETE, reader);
                            case "activate":
                                return ActivationMessageFromJson(ActivationMessage.ACTIVATION_MESSAGE_TYPE.ACTIVATE, reader);
                            case "deactivate":
                                return ActivationMessageFromJson(ActivationMessage.ACTIVATION_MESSAGE_TYPE.DEACTIVATE, reader);
                            case "data":
                                return DataMessageFromJson(reader);
                            case "event":
                                return EventMessageFromJson(reader);
                            case "error":
                                return ErrorMessageFromJson(reader);
                            default:
                                return null;
                        }
                    }
                }
            }


            return null;
        }

        private static ConnectionMessage ConnectionMessageFromJson(ConnectionMessage.CONNECTION_MESSAGE_TYPE type, JsonTextReader reader){
            //Logger.Log("connection message found!");

            if (type == ConnectionMessage.CONNECTION_MESSAGE_TYPE.REQUEST)
            {
                return new ConnectionMessage(type);
            }
            

            String property = string.Empty;

            ConnectionMessage msg = new ConnectionMessage(type);

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

        private static ActivationMessage ActivationMessageFromJson(ActivationMessage.ACTIVATION_MESSAGE_TYPE type, JsonTextReader reader)
        {
            //Logger.Log("Activation message found!");

            if (type == ActivationMessage.ACTIVATION_MESSAGE_TYPE.DEACTIVATE)
            {
                return new ActivationMessage(ActivationMessage.ACTIVATION_MESSAGE_TYPE.DEACTIVATE);
            }

            String property = string.Empty;

            ActivationMessage msg = new ActivationMessage(type);

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

        private static ErrorMessage ErrorMessageFromJson(JsonTextReader reader)
        {
            //Logger.Log("Error message found!");

            String property = string.Empty;

            ErrorMessage msg = new ErrorMessage();

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                        property = reader.Value.ToString();

                    if (property == "uniqueid" && reader.TokenType == JsonToken.String)
                    {
                        msg.UniqueID = reader.Value.ToString();

                        return msg;
                    }
                }
            }

            return null;
        }

        private static InDataMessage DataMessageFromJson(JsonTextReader reader)
        {
            String property = string.Empty;

            Boolean element = false;

            DOMElementModel leftElement = new DOMElementModel();
            DOMElementModel rightElement = null;

            InDataMessage msg = new InDataMessage();

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    //comment console writeline in for debugging
                    //Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
                    //get property ==> name of field
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        property = reader.Value.ToString();

                        if (property == "attributes")
                        {
                            ExtractAttributes(reader, leftElement);
                        }

                        if (!element && property == "left")
                        {
                            ExtractElementData(reader, leftElement);
                        }

                        if (!element && property == "right")
                        {
                            rightElement = new DOMElementModel();
                            ExtractElementData(reader, rightElement);
                        }
                        continue;
                    }

                    if (element && (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float))
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
                            case "url":
                                msg.URL = value;
                                break;
                            case "tag":
                                leftElement.Tag = value;
                                break;
                            case "path":
                                leftElement.Path = value;
                                break;
                            case "selector":
                                leftElement.Selector = value;
                                break;

                        }
                    }
                    else if ((reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float))
                    {
                        Double value = Double.Parse(reader.Value.ToString());

                        switch (property)
                        {
                            case "clientreceived":
                                msg.ClientReceived = value.ToString();
                                break;
                            case "clientsent":
                                msg.ClientSent = value.ToString();
                                break;
                            case "htmlX":
                                leftElement.HTMLX = value;
                                break;
                            case "htmlY":
                                leftElement.HTMLY = value;
                                break;
                        }
                    }
                }
                else
                {
                    // Process tracking the current nested element
                    if (property == "element" && reader.TokenType == JsonToken.StartObject)
                    {
                        element = true;
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
            else
            {
                msg.Type = InDataMessage.MESSAGE_TYPE.NORMAL;
            }

            msg.LeftElement = leftElement;
            msg.RightElement = rightElement;

            return msg;
        }

        private static void ExtractAttributes(JsonTextReader reader, DOMElementModel element)
        {

            String property = "";

            String name = "";
            String value = "";

            Boolean endObj = false;

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    endObj = false;
                    //Console.WriteLine("In Attributes... Token: {0}, Value: {1}", reader.TokenType, reader.Value);

                    if (reader.TokenType == JsonToken.PropertyName)
                        property = reader.Value.ToString();

                    if (property == "name" && reader.TokenType == JsonToken.String)
                    {
                        name = reader.Value.ToString();
                    }


                    if (property == "value" && reader.TokenType == JsonToken.String)
                    {
                        value = reader.Value.ToString();
                    }

                    if (name != "" && value != "")
                    {
                        //Console.WriteLine("Name: {0}, Value: {1}", name, value);

                        element.AddAttribute(name, value);

                        if (name == "id")
                        {
                            element.ID = value;
                        }
                        else if (name == "title")
                        {
                            element.Title = value;
                        }
                        else if (name == "class")
                        {
                            element.AddClasses(value);
                        }

                        name = "";
                        value = "";
                    }
                }
                else
                {
                    if (reader.TokenType == JsonToken.EndObject)
                    {
                        endObj = true;
                    }

                    //attributes should end here
                    if (endObj && reader.TokenType == JsonToken.EndArray)
                    {
                        return;
                    }
                }
            }

        }

        private static void ExtractElementData(JsonTextReader reader, DOMElementModel element)
        {
            //@TODO: ERROR checking in complex message?

            String property = string.Empty;

            Boolean elementDataFound = false;

            Boolean elementDataComplete = false;

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    //Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
                    //get property ==> name of field
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        property = reader.Value.ToString();

                        if (property == "attributes")
                        {
                            ExtractAttributes(reader, element);
                        }
                        continue;
                    }

                    if (elementDataFound && (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float))
                    {
                        Double value = Double.Parse(reader.Value.ToString());

                        switch (property)
                        {
                            case "top":
                                element.Top = value;
                                break;
                            case "left":
                                element.Left = value;
                                break;
                            case "width":
                                element.Width = value;
                                break;
                            case "height":
                                element.Height = value;
                                break;
                            case "outerWidth":
                                element.OuterWidth = value;
                                break;
                            case "outerHeight":
                                element.OuterHeight = value;
                                break;
                        }
                    }
                    else if (reader.TokenType == JsonToken.String && property == "tag")
                    {
                        element.Tag = reader.Value.ToString();
                    } else if (reader.TokenType == JsonToken.String && property == "path")
                    {
                        element.Path = reader.Value.ToString();
                    }
                    else if (reader.TokenType == JsonToken.String && property == "selector")
                    {
                        element.Selector = reader.Value.ToString();
                    }
                    else if ((reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Integer) && property == "htmlX")
                    {
                        element.HTMLX = Double.Parse(reader.Value.ToString());
                    }
                    else if ((reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Integer) && property == "htmlY")
                    {
                        element.HTMLY = Double.Parse(reader.Value.ToString());
                    }
                }
                else
                {
                    // Process tracking the current nested element
                    if (property == "element" && reader.TokenType == JsonToken.StartObject)
                    {
                        elementDataFound = true;
                    }

                    if (elementDataFound && reader.TokenType == JsonToken.EndObject)
                    {
                        elementDataFound = false;
                        elementDataComplete = true;
                    }

                    if (elementDataComplete && reader.TokenType == JsonToken.EndObject)
                    {
                        return;
                    }
                }
            }
        }

        private static EventMessage EventMessageFromJson(JsonTextReader reader)
        {
            //Logger.Log("event message found!");

            String property = string.Empty;

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                        property = reader.Value.ToString();

                    if (reader.TokenType == JsonToken.String && property == "eventType")
                    {
                        String eventType = reader.Value.ToString();

                        //Logger.Log("EventType: " + eventType);

                        switch (eventType)
                        {
                            case "url-change":
                                return EventMessage.URLChangeEventMessageFromJson(reader);
                            case "resize":
                                return EventMessage.ResizeEventMessageFromJson(reader);
                            case "scroll":
                                return EventMessage.ScrollEventMessageFromJson(reader);
                            default:
                                return null;
                        }
                    }
                }
            }

            return null;
        }
    }

}
