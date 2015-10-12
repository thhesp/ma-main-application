using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Models.Base;
using WebAnalyzer.Util;

namespace WebAnalyzer.Models.EventModel
{
    /// <summary>
    /// Class for representing browser scroll events
    /// </summary>
    public class ScrollEventModel : BaseEventModel
    {
        /// <summary>
        /// X value of the scroll event
        /// </summary>
        private int _scrollX;

        /// <summary>
        /// Y value of the scroll event
        /// </summary>
        private int _scrollY;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="scrollX">X Value</param>
        /// <param name="scrollY">Y Value</param>
        /// <param name="serverReceivedTimestamp">Timestamp when the server received the data</param>
        public ScrollEventModel(int scrollX, int scrollY, String serverReceivedTimestamp)
        {
            _scrollX = scrollX;
            _scrollY = scrollY;
            _serverReceivedTimestamp = serverReceivedTimestamp;
        }

        /// <summary>
        /// Empty constructor for loading from the xml
        /// </summary>
        public ScrollEventModel()
        {

        }

        /// <summary>
        /// Getter/ Setter for the X Value of the scroll event
        /// </summary>
        public int ScrollX
        {
            get { return _scrollX; }
            set { _scrollX = value; }
        }

        /// <summary>
        /// Getter/ Setter for the Y Value of the scroll event
        /// </summary>
        public int ScrollY
        {
            get { return _scrollY; }
            set { _scrollY = value; }
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public override XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode resizeEventNode = xmlDoc.CreateElement("scroll-event");

            //scrollX

            XmlAttribute scrollX = xmlDoc.CreateAttribute("scroll-x");

            scrollX.Value = this.ScrollX.ToString();

            resizeEventNode.Attributes.Append(scrollX);

            //scrollY

            XmlAttribute scrollY = xmlDoc.CreateAttribute("scroll-y");

            scrollY.Value = this.ScrollY.ToString();

            resizeEventNode.Attributes.Append(scrollY);

            //serverReceived

            XmlAttribute serverReceived = xmlDoc.CreateAttribute("server-received-timestamp");

            serverReceived.Value = this.ServerReceivedTimestamp;

            resizeEventNode.Attributes.Append(serverReceived);

            //eventTimesatmp

            XmlAttribute eventTimestamp = xmlDoc.CreateAttribute("event-timestamp");

            eventTimestamp.Value = this.EventTimestamp;

            resizeEventNode.Attributes.Append(eventTimestamp);

            return resizeEventNode;
        }

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="eventNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
        public static ScrollEventModel LoadFromXML(XmlNode eventNode)
        {
            if (eventNode.Name == "scroll-event")
            {
                ScrollEventModel scrollModel = new ScrollEventModel();

                foreach (XmlAttribute attr in eventNode.Attributes)
                {
                    switch (attr.Name)
                    {
                        case "scroll-x":
                            scrollModel.ScrollX = int.Parse(attr.Value);
                            break;
                        case "scroll-y":
                            scrollModel.ScrollY = int.Parse(attr.Value);
                            break;
                        case "server-received-timestamp":
                            scrollModel.ServerReceivedTimestamp = attr.Value;
                            break;
                        case "event-timestamp":
                            scrollModel.EventTimestamp = attr.Value;
                            break;
                    }
                }

                return scrollModel;
            }

            Logger.Log("Wrong node type given");

            return null;
        }
    }
}
