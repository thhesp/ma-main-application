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
    /// Class for representing browser resize events
    /// </summary>
    public class ResizeEventModel : BaseEventModel
    {
        /// <summary>
        /// Window width
        /// </summary>
        private int _windowWidth = 0;
        /// <summary>
        /// Window height
        /// </summary>
        private int _windowHeight = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="windowHeight">window height</param>
        /// <param name="windowWidth">window width</param>
        /// <param name="serverReceivedTimestamp">Timestamp when the server received the event</param>
        public ResizeEventModel(int windowHeight, int windowWidth, String serverReceivedTimestamp)
        {
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
            _serverReceivedTimestamp = serverReceivedTimestamp;
        }

        /// <summary>
        /// Constructor for loading from the xml
        /// </summary>
        public ResizeEventModel()
        {

        }

        /// <summary>
        /// Getter/ Setter for the window width
        /// </summary>
        public int WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; }
        }

        /// <summary>
        /// Getter/ Setter for the window height
        /// </summary>
        public int WindowHeight
        {
            get { return _windowHeight; }
            set { _windowHeight = value; }
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public override XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode resizeEventNode = xmlDoc.CreateElement("resize-event");

            //windowWidth

            XmlAttribute windowWidth = xmlDoc.CreateAttribute("window-width");

            windowWidth.Value = this.WindowWidth.ToString();

            resizeEventNode.Attributes.Append(windowWidth);

            //windowHeight

            XmlAttribute windowHeight = xmlDoc.CreateAttribute("window-height");

            windowHeight.Value = this.WindowHeight.ToString();

            resizeEventNode.Attributes.Append(windowHeight);

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
        public static ResizeEventModel LoadFromXML(XmlNode eventNode)
        {
            if (eventNode.Name == "resize-event")
            {
                ResizeEventModel resizeModel = new ResizeEventModel();

                foreach (XmlAttribute attr in eventNode.Attributes)
                {
                    switch (attr.Name)
                    {
                        case "window-width":
                            resizeModel.WindowWidth = int.Parse(attr.Value);
                            break;
                        case "window-height":
                            resizeModel.WindowHeight = int.Parse(attr.Value);
                            break;
                        case "server-received-timestamp":
                            resizeModel.ServerReceivedTimestamp = attr.Value;
                            break;
                        case "event-timestamp":
                            resizeModel.EventTimestamp = attr.Value;
                            break;
                    }
                }

                return resizeModel;

            }

            Logger.Log("Wrong node type given");

            return null;
        }
    }
}
