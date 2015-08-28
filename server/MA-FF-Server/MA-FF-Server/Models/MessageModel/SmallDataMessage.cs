using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel
{
    class SmallDataMessage : Message
    {

                /* 
         * 
         * Message Data
         * 
         */


        String _uniqueId;

        double _x;
        double _y;

        String _requestTimestamp;

        public SmallDataMessage(String _timestamp) : base(_timestamp)
        {

        }

        public Message SetMessageData(String uniqueId, double x, double y)
        {
            _uniqueId = uniqueId;
            _x = x;
            _y = y;

            _requestTimestamp = Util.Timestamp.GetMillisecondsUnixTimestamp();

            return this;
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

            // "datarequest" : 
            writer.WritePropertyName("datarequest");
            writer.WriteValue(_requestTimestamp);

            // "serversent" : 
            writer.WritePropertyName("serversent");
            writer.WriteValue(Util.Timestamp.GetMillisecondsUnixTimestamp());


            // "x" : 
            writer.WritePropertyName("x");
            writer.WriteValue(_x);

            // "y" : 
            writer.WritePropertyName("y");
            writer.WriteValue(_y);


            // }
            writer.WriteEndObject();

            return sw.ToString();
        }
    }
}
