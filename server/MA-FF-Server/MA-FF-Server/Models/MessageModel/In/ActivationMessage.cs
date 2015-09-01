using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel
{
    class ActivationMessage : Message
    {

        /* 
         * 
         * Message Data
         * 
         */

       public enum ACTIVATION_MESSAGE_TYPE { ACTIVATE = 0, DEACTIVATE = 1 };


       private ACTIVATION_MESSAGE_TYPE _type;

       public ActivationMessage(ACTIVATION_MESSAGE_TYPE type)
        {
            _type = type;
        }

       public ACTIVATION_MESSAGE_TYPE Type
        {
            get { return _type; }
        }
    }
}
