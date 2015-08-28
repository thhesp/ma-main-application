using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel
{
    class InSmallDataMessage : SmallDataMessage
    {

        /* 
         * 
         * Message Data
         * 
         */


        public InSmallDataMessage(String _timestamp) : base(_timestamp)
        {

        }
    }
}
