using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.UI.InteractionObjects;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// TestrunEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">TestrunEvent which is sent</param>
    public delegate void TestrunEventHandler(BaseInteractionObject sender, TestrunEvent e);

    /// <summary>
    /// Event which controls the creating, starting and stopping of testruns
    /// </summary>
    public class TestrunEvent : EventArgs
    {

        /// <summary>
        /// Different event types for creating, starting and stopping
        /// </summary>
        public enum EVENT_TYPE { Create = 0,Start = 1, Stop = 2 };

        /// <summary>
        /// Type of the event
        /// </summary>
        private EVENT_TYPE _type;

        /// <summary>
        /// Event Constructor
        /// </summary>
        /// <param name="type">Type of the Event</param>
        public TestrunEvent(EVENT_TYPE type)
        {
            _type = type;
        }

        /// <summary>
        /// Getter for the event type
        /// </summary>
        public EVENT_TYPE Type
        {
            get { return _type; }
        }
    }
}
