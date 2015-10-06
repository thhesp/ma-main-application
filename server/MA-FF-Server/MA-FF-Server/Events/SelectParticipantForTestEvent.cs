using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// SelectParticipantForTestEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">SelectParticipantForTestEvent which is sent</param>
    public delegate void SelectParticipantForTestEventHandler(object sender, SelectParticipantForTestEvent e);

    /// <summary>
    /// Used for selecting an participant for an testrun
    /// </summary>
    public class SelectParticipantForTestEvent : EventArgs
    {
        /// <summary>
        /// UID of the participant
        /// </summary>
        private String _uid;

        /// <summary>
        /// Event-Constructor
        /// </summary>
        /// <param name="uid">UID of the selected participant</param>
        public SelectParticipantForTestEvent(String uid)
        {
            _uid = uid;
        }

        /// <summary>
        /// Getter for the participant UID
        /// </summary>
        public String UID
        {   
            get {return _uid; }
        }
    }
}
