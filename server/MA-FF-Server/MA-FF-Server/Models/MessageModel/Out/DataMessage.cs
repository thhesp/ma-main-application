﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel
{
    public class DataMessage : Message
    {

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

        private String _requestTimestamp;

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

            _requestTimestamp = Util.Timestamp.GetMillisecondsUnixTimestamp();

            return this;
        }

        public String UniqueID
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }


        public String RequestTimestamp
        {
            get { return _requestTimestamp; }
            set { _requestTimestamp = value; }
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

            // "datarequest" : 
            writer.WritePropertyName("datarequest");
            writer.WriteValue(_requestTimestamp);

            // "serversent" : 
            writer.WritePropertyName("serversent");
            writer.WriteValue(Util.Timestamp.GetMillisecondsUnixTimestamp());

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

            return sw.ToString();
        }
    }
}
