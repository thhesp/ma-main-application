using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.MessageModel
{
    class EchoMessage : Message
    {

        dynamic _origMsg;

        public EchoMessage(String _timestamp, dynamic origMsg) : base(_timestamp)
        {
            _origMsg = origMsg;
        }

        public dynamic MessageObj
        {
            get { return _origMsg; }
        }
    }
}
