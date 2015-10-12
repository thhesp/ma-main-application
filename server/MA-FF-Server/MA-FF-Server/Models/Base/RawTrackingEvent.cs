using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebAnalyzer.Models.Base
{
    /// <summary>
    /// The raw event sent from the tracking-component
    /// </summary>
    public class RawTrackingEvent
    {
        /// <summary>
        /// The Event type (currently only F in use)
        /// </summary>
        private String _eventType;

        /// <summary>
        /// The eye for which the event occurred
        /// </summary>
        private String _eye;

        /// <summary>
        /// The start timestamp of the event
        /// </summary>
        private String _startTime;

        /// <summary>
        /// The end timestamp of the event
        /// </summary>
        private String _endTime;

        /// <summary>
        /// The duration of the event
        /// </summary>
        private String _duration;

        /// <summary>
        /// x coordinates of the event
        /// </summary>
        private double _x;

        /// <summary>
        /// y coordinates of the event
        /// </summary>
        private double _y;

        /// <summary>
        /// Getter/ Setter for the event type
        /// </summary>
        public String EventType
        {
            get { return _eventType; }
            set { _eventType = value; }
        }

        /// <summary>
        /// Getter/Setter for the eye
        /// </summary>
        public String Eye
        {
            get { return _eye; }
            set { _eye = value; }
        }

        /// <summary>
        /// Getter/ Setter for the starttime
        /// </summary>
        public String StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        /// <summary>
        /// Getter/Setter for the endtime
        /// </summary>
        public String EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        /// <summary>
        /// Getter/Setter for the duration
        /// </summary>
        public String Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        /// <summary>
        /// Getter/ Setter for the x coordinate
        /// </summary>
        public Double X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Getter/ Setter for the y coordinate
        /// </summary>
        public Double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Checks if the event is a fixation
        /// </summary>
        /// <returns></returns>
        public Boolean IsFixation()
        {
            return EventType == "F";
        }

        /// <summary>
        /// Checks if event belongs to eye
        /// </summary>
        /// <param name="eye">String for the eye</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="eventNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
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
