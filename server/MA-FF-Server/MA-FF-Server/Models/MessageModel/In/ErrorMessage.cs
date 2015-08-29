using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel
{
    class ErrorMessage : Message
    {

        /* 
         * 
         * Message Data
         * 
         */

        private String _uniqueId;

        public ErrorMessage()
        {
        }

        public String UniqueID
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }
    }
}
