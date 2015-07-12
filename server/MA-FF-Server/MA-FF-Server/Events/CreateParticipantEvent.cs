using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Events
{
    public delegate void CreateParticipantEventHandler(object sender, CreateParticipantEvent e);

    public class CreateParticipantEvent : EventArgs
    {

        private ExperimentParticipant _participant;

        public CreateParticipantEvent(ExperimentParticipant participant)
        {
            _participant = participant;
        }

        public ExperimentParticipant Participant
        {
            get { return _participant; }
        }
    }
}
