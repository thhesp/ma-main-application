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
    /// Outgoing data message for two eyes with the same coordinates
    /// </summary>
    class SmallDataMessage : Message
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
        /// X Coordinate of the message
        /// </summary>
        private double _x;

        /// <summary>
        /// Y Coordinate of the message
        /// </summary>
        private double _y;

        /// <summary>
        /// Constructor with request timestamp
        /// </summary>
        /// <param name="_timestamp">Request timestamp of the data</param>
        public SmallDataMessage(String _timestamp) : base(_timestamp)
        {

        }

        /// <summary>
        /// Constructor with uniqueID, X and Y coordinates
        /// </summary>
        /// <param name="uniqueId">UniqueID of the message/ gaze</param>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <returns>The current Message</returns>
        public Message SetMessageData(String uniqueId, double x, double y)
        {
            _uniqueId = uniqueId;
            _x = x;
            _y = y;

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
        /// Getter/ Setter for the X Coordinate
        /// </summary>
        public Double X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Getter/ Setter for the Y Coordinate
        /// </summary>
        public Double Y
        {
            get { return _y; }
            set { _y = value; }
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
