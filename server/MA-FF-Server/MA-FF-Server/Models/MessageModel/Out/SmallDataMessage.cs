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
    class SmallDataMessage : Message
    {
        public event MessageSentEventHandler MessageSent;

        /* 
         * 
         * Message Data
         * 
         */


        private String _uniqueId;

        private double _x;
        private double _y;

        public SmallDataMessage(String _timestamp) : base(_timestamp)
        {

        }

        public Message SetMessageData(String uniqueId, double x, double y)
        {
            _uniqueId = uniqueId;
            _x = x;
            _y = y;

            return this;
        }

        public String UniqueID
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }

        public Double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public Double Y
        {
            get { return _y; }
            set { _y = value; }
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

            // "x" : 
            writer.WritePropertyName("x");
            writer.WriteValue(_x);

            // "y" : 
            writer.WritePropertyName("y");
            writer.WriteValue(_y);


            // }
            writer.WriteEndObject();

            MessageSent(this, new MessageSentEvent(this.UniqueID));

            return sw.ToString();
        }
    }
}
