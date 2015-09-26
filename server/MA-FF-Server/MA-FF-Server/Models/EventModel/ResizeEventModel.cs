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
    class ResizeEventModel : BaseEventModel
    {
        private int _windowWidth = 0;
        private int _windowHeight = 0;

        public ResizeEventModel(int windowHeight, int windowWidth, String serverReceivedTimestamp)
        {
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
            _serverReceivedTimestamp = serverReceivedTimestamp;
        }

        public ResizeEventModel()
        {

        }

        public int WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; }
        }

        public int WindowHeight
        {
            get { return _windowHeight; }
            set { _windowHeight = value; }
        }

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



            return resizeEventNode;
        }

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
