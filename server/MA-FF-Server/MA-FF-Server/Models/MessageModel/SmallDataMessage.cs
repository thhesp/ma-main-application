using System;
using System.Collections.Generic;
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
                x = _x, 
                y = _y
            };

        }
    }
}
