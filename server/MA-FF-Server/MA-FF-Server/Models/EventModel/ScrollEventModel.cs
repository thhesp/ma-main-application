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
    public class ScrollEventModel : BaseEventModel
    {
        private int _scrollX;
        private int _scrollY;

        public ScrollEventModel(int scrollX, int scrollY, String serverReceivedTimestamp)
        {
            _scrollX = scrollX;
            _scrollY = scrollY;
            _serverReceivedTimestamp = serverReceivedTimestamp;
        }

        public ScrollEventModel()
        {

        }

        public int ScrollX
        {
            get { return _scrollX; }
            set { _scrollX = value; }
        }

        public int ScrollY
        {
            get { return _scrollY; }
            set { _scrollY = value; }
        }


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
