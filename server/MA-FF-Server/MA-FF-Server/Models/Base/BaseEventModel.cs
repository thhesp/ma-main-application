using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Models.EventModel
{
    /// <summary>
    /// Abstract class for all event models
    /// </summary>
    public abstract class BaseEventModel : BasicData
    {
        /// <summary>
        /// Event timestamp
        /// </summary>
        protected String _eventTimestamp;

        /// <summary>
        /// Getter/ Setter for the event timestamp
        /// </summary>
        public String EventTimestamp
        {
            get { return _eventTimestamp; }
            set { _eventTimestamp = value; }
        }

        /// <summary>
        /// Url of the event
        /// </summary>
        protected String _url;

        /// <summary>
        /// Getter/ Setter for the event url
        /// </summary>
        public String URL
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public abstract XmlNode ToXML(XmlDocument xmlDoc);

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="eventNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
        /// <remarks>Checks for the implemented child classes and calls their method</remarks>
        public static BaseEventModel LoadFromXML(XmlNode eventNode)
        {
            if (eventNode.Name == "scroll-event")
            {
                return ScrollEventModel.LoadFromXML(eventNode);
            }
            else if (eventNode.Name == "resize-event")
            {
                return ResizeEventModel.LoadFromXML(eventNode);
            }

            return null;
        }
    }
}
