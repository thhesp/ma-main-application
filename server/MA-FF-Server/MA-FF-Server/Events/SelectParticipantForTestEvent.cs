using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    public delegate void SelectParticipantForTestEventHandler(object sender, SelectParticipantForTestEvent e);

    public class SelectParticipantForTestEvent : EventArgs
    {
        private String _uid;

        public SelectParticipantForTestEvent(String uid)
        {
            _uid = uid;
        }

        public String UID
        {   
            get {return _uid; }
        }
    }
}
