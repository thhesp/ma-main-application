using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class ExperimentObject : BaseInteractionObject
    {

        private String _name;

        private String[] _participants;

        private String[] _domainSettings;

        public ExperimentObject(String name){
            _name = name;
        }

        public String getName(){
            return _name;
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public String[] participantArray()
        {
            return Participants;
        }

        public String[] domainSettingsArray()
        {
            return DomainSettings;
        }

        public String[] Participants
        {
            get { return _participants; }
            set { _participants = value; }
        }

        public String[] DomainSettings
        {
            get { return _domainSettings; }
            set { _domainSettings = value; }
        }
    }
}
