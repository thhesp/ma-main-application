using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Events
{
    public delegate void AddTrackingEventHandler(object sender, AddTrackingEvent e);

    public class AddTrackingEvent : EventArgs
    {

        private RawTrackingEvent _event;

        public AddTrackingEvent(RawTrackingEvent rawEvent)
        {
            _event = rawEvent;
        }

        public RawTrackingEvent Event
        {
            get { return _event; }
        }
    }
}
