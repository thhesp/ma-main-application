using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.Base;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class ExperimentObject : BaseInteractionObject
    {

        private ExperimentModel _exp;


        public ExperimentObject(){
        }

        public ExperimentModel Experiment
        {
            get { return _exp; }
            set { _exp = value; }
        }

        public String getName(){
            if(_exp != null)
                return _exp.ExperimentName;

            return null;
        }

        public String[] participantArray()
        {
            if (_exp != null)
                return _exp.GetParticipantArray();

            return null;
        }

        public String[] participantUIDs()
        {
            if (_exp != null)
                return _exp.GetParticipantUIDs();

            return null;
        }

        public String[] domainSettingsArray()
        {
            if (_exp != null)
                return _exp.GetDomainSettingArray();

            return null;
        }

        public String[] domainSettingUIDs()
        {
            if (_exp != null)
                return _exp.GetDomainSettingUIDs();

            return null;
        }
    }
}
