using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        public Boolean IsFixation()
        {
            return EventType == "F";
        }

        public Boolean BelongsToEye(String eye)
        {
            if (this.Eye == "l" && eye == "left")
            {
                return true;
            }
            else if (this.Eye == "r" && eye == "right")
            {
                return true;
            }

            return false;
        }

        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode eventNode = xmlDoc.CreateElement("event");

            //eventtype

            XmlAttribute eventType = xmlDoc.CreateAttribute("event-type");

            eventType.Value = this.EventType;

            eventNode.Attributes.Append(eventType);

            //eye

            XmlAttribute eye = xmlDoc.CreateAttribute("eye");

            eye.Value = this.Eye;

            eventNode.Attributes.Append(eye);

            //starttime

            XmlAttribute startTime = xmlDoc.CreateAttribute("start-time");

            startTime.Value = this.StartTime;

            eventNode.Attributes.Append(startTime);

            //endtime

            XmlAttribute endTime = xmlDoc.CreateAttribute("end-time");

            endTime.Value = this.EndTime;

            eventNode.Attributes.Append(endTime);

            //duration

            XmlAttribute duration = xmlDoc.CreateAttribute("duration");

            duration.Value = this.Duration;

            eventNode.Attributes.Append(duration);

            //x

            XmlAttribute x = xmlDoc.CreateAttribute("x");

            x.Value = this.X.ToString();

            eventNode.Attributes.Append(x);

            //y

            XmlAttribute y = xmlDoc.CreateAttribute("y");

            y.Value = this.Y.ToString();

            eventNode.Attributes.Append(y);



            return eventNode;
        }

        public static RawTrackingEvent LoadFromXML(XmlNode eventNode)
        {
            if (eventNode.Name == "event")
            {
                RawTrackingEvent rawEvent = new RawTrackingEvent();

                foreach (XmlAttribute attr in eventNode.Attributes)
                {
                    switch (attr.Name)
                    {
                        case "event-type":
                            rawEvent.EventType = attr.Value;
                            break;
                        case "eye":
                            rawEvent.Eye = attr.Value;
                            break;
                        case "start-time":
                            rawEvent.StartTime = attr.Value;
                            break;
                        case "end-time":
                            rawEvent.EndTime = attr.Value;
                            break;
                        case "duration":
                            rawEvent.Duration = attr.Value;
                            break;
                        case "x":
                            rawEvent.X = Double.Parse(attr.Value);
                            break;
                        case "y":
                            rawEvent.Y = Double.Parse(attr.Value);
                            break;
                    }
                }

                return rawEvent;
            }

            return null;
        }
    }
}
