using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.Base
{
    public class RawTrackingData
    {

        private List<RawTrackingGaze> _gazes = new List<RawTrackingGaze>();

        private List<RawTrackingEvent> _events = new List<RawTrackingEvent>();

        public List<RawTrackingGaze> Gazes
        {
            get { return _gazes; }
            set { _gazes = value; }
        }

        public List<RawTrackingEvent> Events
        {
            get { return _events; }
            set { _events = value; }
        }

        public List<RawTrackingEvent> GetEventsForEye(String eye)
        {
            List<RawTrackingEvent> eyeEvents = new List<RawTrackingEvent>();

            foreach(RawTrackingEvent rawEvent in Events)
            {
                if (rawEvent.BelongsToEye(eye))
                {
                    eyeEvents.Add(rawEvent);
                }
            }

            return eyeEvents;
        }


        public List<RawTrackingEvent> GetFixationsForEye(String eye)
        {
            List<RawTrackingEvent> eyeEvents = new List<RawTrackingEvent>();

            foreach (RawTrackingEvent rawEvent in Events)
            {
                if (rawEvent.IsFixation() && rawEvent.BelongsToEye(eye))
                {
                    eyeEvents.Add(rawEvent);
                }
            }

            return eyeEvents;
        }

        public void AddEvent(RawTrackingEvent rawEvent)
        {
            _events.Add(rawEvent);
        }

        public void AddRawGaze(EyeTrackingData leftEye, EyeTrackingData rightEye)
        {
            _gazes.Add(new RawTrackingGaze(leftEye, rightEye));
        }

        public XmlNode ToXML(ExperimentParticipant participant, XmlDocument xmlDoc)
        {
            XmlNode experimentNode = xmlDoc.CreateElement("experiment");

            XmlAttribute numberOfGazes = xmlDoc.CreateAttribute("number-of-gazes");

            numberOfGazes.Value = _gazes.Count.ToString();

            experimentNode.Attributes.Append(numberOfGazes);

            XmlAttribute numberOfEvents = xmlDoc.CreateAttribute("number-of-events");

            numberOfEvents.Value = _events.Count.ToString();

            experimentNode.Attributes.Append(numberOfEvents);

            // add participant data
            experimentNode.AppendChild(participant.ToXML(xmlDoc));

            // add experiment data
            XmlNode dataNode = xmlDoc.CreateElement("raw-gazes");

            foreach (RawTrackingGaze gaze in _gazes)
            {
                dataNode.AppendChild(gaze.ToXML(xmlDoc));
            }

            experimentNode.AppendChild(dataNode);

            // add experiment data
            XmlNode eventsNode = xmlDoc.CreateElement("raw-events");

            foreach (RawTrackingEvent rawEvent in _events)
            {
                eventsNode.AppendChild(rawEvent.ToXML(xmlDoc));
            }

            experimentNode.AppendChild(eventsNode);

            return experimentNode;
        }

        public static RawTrackingData LoadFromXML(XmlDocument doc)
        {
            RawTrackingData data = new RawTrackingData();

            XmlNode experimentNode = doc.DocumentElement.SelectSingleNode("/experiment");

            if (experimentNode == null)
            {
                return null;
            }

            foreach (XmlNode childs in experimentNode.ChildNodes)
            {
                if (childs.Name == "raw-gazes")
                {
                    foreach (XmlNode gazeNode in childs)
                    {
                        RawTrackingGaze gaze = RawTrackingGaze.LoadFromXML(gazeNode);

                        if (gaze != null)
                        {
                            data._gazes.Add(gaze);
                        }
                    }
                }
                else if (childs.Name == "raw-events")
                {
                    foreach (XmlNode eventNode in childs)
                    {
                        RawTrackingEvent rawEvent = RawTrackingEvent.LoadFromXML(eventNode);

                        if (rawEvent != null)
                        {
                            data._events.Add(rawEvent);
                        }
                    }
                }
            }

            return data;
        }
    }
}
