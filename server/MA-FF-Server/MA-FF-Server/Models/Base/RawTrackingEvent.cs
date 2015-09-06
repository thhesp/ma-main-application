using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.Base
{
    public class RawTrackingEvent
    {
        private String _eventType;
        private String _eye;
        private String _startTime;
        private String _endTime;
        private String _duration;
        private double _x;
        private double _y;

        public String EventType
        {
            get { return _eventType; }
            set { _eventType = value; }
        }

        public String Eye
        {
            get { return _eye; }
            set { _eye = value; }
        }

        public String StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        public String EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        public String Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        public Double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public Double Y
        {
            get { return _y; }
            set { _y = value; }
        }
    }
}
