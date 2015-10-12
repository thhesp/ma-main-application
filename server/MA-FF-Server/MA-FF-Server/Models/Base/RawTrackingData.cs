using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.Base
{
    /// <summary>
    /// Class for all raw data
    /// </summary>
    public class RawTrackingData
    {
        /// <summary>
        /// List of raw gazes
        /// </summary>
        private List<RawTrackingGaze> _gazes = new List<RawTrackingGaze>();

        /// <summary>
        /// List of raw events
        /// </summary>
        private List<RawTrackingEvent> _events = new List<RawTrackingEvent>();

        /// <summary>
        /// Getter/ Setter for the raw gazes
        /// </summary>
        public List<RawTrackingGaze> Gazes
        {
            get { return _gazes; }
            set { _gazes = value; }
        }

        /// <summary>
        /// Getter/ Setter for the raw events
        /// </summary>
        public List<RawTrackingEvent> Events
        {
            get { return _events; }
            set { _events = value; }
        }

        /// <summary>
        /// Returns all events for the eye
        /// </summary>
        /// <param name="eye">String for the eye</param>
        /// <returns>List of raw events</returns>
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

        /// <summary>
        /// Returns all fixations for the given eye
        /// </summary>
        /// <param name="eye">String representation of the eye</param>
        /// <returns>List of raw events</returns>
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

        /// <summary>
        /// Adds raw event to the list of raw events
        /// </summary>
        /// <param name="rawEvent">Event to add</param>
        public void AddEvent(RawTrackingEvent rawEvent)
        {
            _events.Add(rawEvent);
        }

        /// <summary>
        /// Adds raw gaze to the list of raw gazes
        /// </summary>
        /// <param name="leftEye">Data for the left eye</param>
        /// <param name="rightEye">Data for the right eye</param>
        public void AddRawGaze(BaseTrackingData leftEye, BaseTrackingData rightEye)
        {
            _gazes.Add(new RawTrackingGaze(leftEye, rightEye));
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="participant">Participant to which the rwa tracking data belongs</param>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="doc">XML Document which contains the data</param>
        /// <returns>The loaded object</returns>
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
