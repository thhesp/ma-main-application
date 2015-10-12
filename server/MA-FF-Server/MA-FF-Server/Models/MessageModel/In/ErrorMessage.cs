using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel
{
    /// <summary>
    /// Model for the error messages
    /// </summary>
    class ErrorMessage : Message
    {

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
        /// Getter / Setter for the unique id
        /// </summary>
        public String UniqueID
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }
    }
}
