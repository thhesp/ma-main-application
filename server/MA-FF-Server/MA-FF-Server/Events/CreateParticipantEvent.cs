using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// CreateParticipantEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">CreateParticipantEvent which is sent</param>
    public delegate void CreateParticipantEventHandler(object sender, CreateParticipantEvent e);

    /// <summary>
    /// Used for saving the newly created participant
    /// </summary>
    public class CreateParticipantEvent : EventArgs
    {

        /// <summary>
        /// The new participant
        /// </summary>
        private ExperimentParticipant _participant;

        /// <summary>
        /// EventConstructor
        /// </summary>
        /// <param name="participant">The new participant</param>
        public CreateParticipantEvent(ExperimentParticipant participant)
        {
            _participant = participant;
        }

        /// <summary>
        /// Getter for the new participant
        /// </summary>
        public ExperimentParticipant Participant
        {
            get { return _participant; }
        }
    }
}
