using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Events;

namespace WebAnalyzer.Models.MessageModel
{
    public class DataMessage : Message
    {
        public event MessageSentEventHandler MessageSent;

        /* 
         * 
         * Message Data
         * 
         */


        private String _uniqueId;

        private double _leftX;
        private double _leftY;

        private double _rightX;
        private double _rightY;

        public DataMessage()
        {

        }

        public DataMessage(String _timestamp)
            : base(_timestamp)
        {

        }

        public Message SetMessageData(String uniqueId, double leftX, double leftY, double rightX, double rightY)
        {
            _uniqueId = uniqueId;
            _leftX = leftX;
            _leftY = leftY;

            _rightX = rightX;
            _rightY = rightY;

            return this;
        }

        public String UniqueID
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }

        public Double LeftX
        {
            get { return _leftX; }
            set { _leftX = value; }
        }

        public Double LeftY
        {
            get { return _leftY; }
            set { _leftY = value; }
        }


        public Double RightX
        {
            get { return _rightX; }
            set { _rightX = value; }
        }

        public Double RightY
        {
            get { return _rightY; }
            set { _rightY = value; }
        }

        public string ToJson()
        {
            StringWriter sw = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(sw);

            //{
            writer.WriteStartObject();

            // "command" : "request"
            writer.WritePropertyName("command");
            writer.WriteValue("request");

            // "uniqueid" : 
            writer.WritePropertyName("uniqueid");
            writer.WriteValue(_uniqueId);

            // "left": x,y
            writer.WritePropertyName("left");

            //{
            writer.WriteStartObject();

            // "x" : 
            writer.WritePropertyName("x");
            writer.WriteValue(_leftX);

            // "y" : 
            writer.WritePropertyName("y");
            writer.WriteValue(_leftY);

            // }
            writer.WriteEndObject();

            // "right": x,y
            writer.WritePropertyName("right");

            //{
            writer.WriteStartObject();

            // "x" : 
            writer.WritePropertyName("x");
            writer.WriteValue(_rightX);

            // "y" : 
            writer.WritePropertyName("y");
            writer.WriteValue(_rightY);

            // }
            writer.WriteEndObject();

            // }
            writer.WriteEndObject();

            MessageSent(this, new MessageSentEvent(this.UniqueID));

            return sw.ToString();
        }
    }
}
