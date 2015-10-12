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
    /// <summary>
    /// Outgoing data message for two eyes with different coordinates
    /// </summary>
    public class DataMessage : Message
    {
        /// <summary>
        /// Handler for the message sent event
        /// </summary>
        public event MessageSentEventHandler MessageSent;

        /* 
         * 
         * Message Data
         * 
         */

        /// <summary>
        /// Uniqueid of the message
        /// </summary>
        private String _uniqueId;

        /// <summary>
        /// X Coordinate for the left eye
        /// </summary>
        private double _leftX;

        /// <summary>
        /// Y Coordinate for the left eye
        /// </summary>
        private double _leftY;

        /// <summary>
        /// X Coordinate for the right eye
        /// </summary>
        private double _rightX;

        /// <summary>
        /// Y Coordinate for the right eye
        /// </summary>
        private double _rightY;


        /// <summary>
        /// Constructor
        /// </summary>
        public DataMessage()
        {

        }

        /// <summary>
        /// Constructor with request timestamp
        /// </summary>
        /// <param name="_timestamp">Request timestamp of the data</param>
        public DataMessage(String _timestamp)
            : base(_timestamp)
        {

        }

        /// <summary>
        /// Constructor with uniqueid and coordinates about the eyes
        /// </summary>
        /// <param name="uniqueId">UniqueID of the message/ gaze</param>
        /// <param name="leftX">X Coordinate for the left eye</param>
        /// <param name="leftY">Y Coordinate for the left eye</param>
        /// <param name="rightX">X Coordinate for the right eye</param>
        /// <param name="rightY">Y Coordinate for the right eye</param>
        /// <returns></returns>
        public Message SetMessageData(String uniqueId, double leftX, double leftY, double rightX, double rightY)
        {
            _uniqueId = uniqueId;
            _leftX = leftX;
            _leftY = leftY;

            _rightX = rightX;
            _rightY = rightY;

            return this;
        }

        /// <summary>
        /// Getter / Setter for the uniqueid of the message / gaze
        /// </summary>
        public String UniqueID
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }

        /// <summary>
        /// Getter/ Setter for the X Coordinate for the left eye
        /// </summary>
        public Double LeftX
        {
            get { return _leftX; }
            set { _leftX = value; }
        }

        /// <summary>
        /// Getter/ Setter for the Y Coordinate for the left eye
        /// </summary>
        public Double LeftY
        {
            get { return _leftY; }
            set { _leftY = value; }
        }

        /// <summary>
        /// Getter/ Setter for the X Coordinate for the right eye
        /// </summary>
        public Double RightX
        {
            get { return _rightX; }
            set { _rightX = value; }
        }

        /// <summary>
        /// Getter/ Setter for the Y Coordinate for the right eye
        /// </summary>
        public Double RightY
        {
            get { return _rightY; }
            set { _rightY = value; }
        }

        /// <summary>
        /// The methods generates a JSON representation of the message
        /// </summary>
        /// <returns>JSON representation of the message</returns>
        /// <remarks>
        /// Used for sending the message
        /// </remarks>
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
