using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// AddTrackingEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">AddTrackingEvent which is sent</param>
    public delegate void AddTrackingEventHandler(object sender, AddTrackingEvent e);

    /// <summary>
    /// Trigger from the tracking component. Used for saving tracking events
    /// </summary>
    public class AddTrackingEvent : EventArgs
    {

        /// <summary>
        /// Data of the tracking event
        /// </summary>
        private RawTrackingEvent _event;

        /// <summary>
        /// EventConstructor
        /// </summary>
        /// <param name="rawEvent">TrackingEvent which happend</param>
        public AddTrackingEvent(RawTrackingEvent rawEvent)
        {
            _event = rawEvent;
        }

        /// <summary>
        /// Getter for the trackingevent data
        /// </summary>
        public RawTrackingEvent Event
        {
            get { return _event; }
        }
    }
}
