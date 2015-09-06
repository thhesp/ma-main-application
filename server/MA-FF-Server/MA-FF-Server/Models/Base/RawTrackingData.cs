using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.Base
{
    public class RawTrackingData
    {

        private List<RawTrackingGaze> _gazes = new List<RawTrackingGaze>();

        private List<RawTrackingEvent> _events = new List<RawTrackingEvent>();

        public void AddEvent(RawTrackingEvent rawEvent){
            _events.Add(rawEvent);
        }

        public void AddRawGaze(String callbackTimestamp, double leftX, double leftY, double rightX, double rightY)
        {
            _gazes.Add(new RawTrackingGaze(callbackTimestamp, leftX, leftY, rightX, rightY));
        }

        public void AddRawGaze(String callbackTimestamp, double x, double y)
        {
            _gazes.Add(new RawTrackingGaze(callbackTimestamp, x, y));
        }
    }
}
