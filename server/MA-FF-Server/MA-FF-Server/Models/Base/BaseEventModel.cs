using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Models.EventModel
{
    public abstract class BaseEventModel : BasicData
    {

        protected String _eventTimestamp;

        public String EventTimestamp
        {
            get { return _eventTimestamp; }
            set { _eventTimestamp = value; }
        }


        protected String _url;

        public String URL
        {
            get { return _url; }
            set { _url = value; }
        }


        public abstract XmlNode ToXML(XmlDocument xmlDoc);

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
