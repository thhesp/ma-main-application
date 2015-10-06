using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// TriggerSaveEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">TriggerSaveEvent which is sent</param>
    public delegate void TriggerSaveEventHandler(object sender, TriggerSaveEvent e);

    /// <summary>
    /// Event for triggering saves of data (to xml)
    /// </summary>
    public class TriggerSaveEvent : EventArgs
    {

        /// <summary>
        /// Different save types
        /// </summary>
        public enum SAVE_TYPES { ALL = 0, PARTICIPANTS = 1, SETTINGS = 2};

        /// <summary>
        /// Save Type of the event
        /// </summary>
        private SAVE_TYPES _type;

        /// <summary>
        /// EventConstructor
        /// </summary>
        /// <param name="type">Save type of the event</param>
        public TriggerSaveEvent(SAVE_TYPES type)
        {
            _type = type;
        }

        /// <summary>
        /// Getter for the save type
        /// </summary>
        public SAVE_TYPES Type
        {
            get { return _type; }
        }
    }
}
