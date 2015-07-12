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

        private String _identifier;
        private Boolean _createNew;

        public EditParticipantEvent(String identifier)
        {
            _identifier = identifier;
        }

        public EditParticipantEvent(Boolean createNew)
        {
            _createNew = createNew;
        }

        public String Identifier
        {   
            get {return _identifier; }
        }

        public Boolean CreateNew
        {
            get { return _createNew; }
        }
    }
}
