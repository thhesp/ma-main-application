using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel
{
    class DataMessage : Message
    {

        /* 
         * 
         * Message Data
         * 
         */


        String _uniqueId;

        double _leftX;
        double _leftY;

        double _rightX;
        double _rightY;

        String _requestTimestamp;

        public DataMessage(String _timestamp) : base(_timestamp)
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

        public dynamic MessageObj
        {
            get { return CreateMessageObject(); }
        }


        /*
         * 
         * Create Message Object for the transfer
         * 
         */

        private dynamic CreateMessageObject()
        {

            return new { 
                command = "request", 
                uniqueid = _uniqueId, 
                datarequest = _requestTimestamp, 
                serversent = Util.Timestamp.GetMillisecondsUnixTimestamp(), 
                left = new { x = _leftX, y = _leftY }, 
                right = new { x = _rightX, y = _leftY } 
            };

        }
    }
}
