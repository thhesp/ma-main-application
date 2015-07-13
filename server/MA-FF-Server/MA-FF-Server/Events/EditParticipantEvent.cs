using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    public delegate void EditParticipantEventHandler(object sender, EditParticipantEvent e);

    public class EditParticipantEvent : EventArgs
    {

        public enum EDIT_TYPES { Create = 0, Edit = 1, Copy = 2, Delete = 3 };

        private String _uid;

        private EDIT_TYPES _type;

        public EditParticipantEvent(EDIT_TYPES type, String uid)
        {
            _uid = uid;
            _type = type;
        }

        public EditParticipantEvent(EDIT_TYPES type)
        {
            _type = type;
        }

        public String UID
        {   
            get {return _uid; }
        }

        public EDIT_TYPES Type
        {
            get { return _type; }
        }
    }
}
