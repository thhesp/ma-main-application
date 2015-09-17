using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Util;

namespace WebAnalyzer.Models.Base
{
    public abstract class BaseTrackingData
    {

        protected double _x;
        protected double _y;

        protected String _callbackTimestamp;

        public BaseTrackingData()
        {

        }

        public BaseTrackingData(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public BaseTrackingData(double x, double y, String callbackTimestamp)
            : this(x, y)
        {
            _callbackTimestamp = callbackTimestamp;
        }

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }


        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public String CallbackTimestamp
        {
            get { return _callbackTimestamp; }
            set { _callbackTimestamp = value; }
        }

        public abstract XmlNode ToXML(XmlDocument xmlDoc);

        public static BaseTrackingData LoadFromXML(XmlNode trackingNode)
        {
            if (trackingNode.Name == "eyetracking-data")
            {
                return EyeTrackingData.LoadFromXML(trackingNode);
            }else if(trackingNode.Name == "mousetracking-data"){
                return MouseTrackingData.LoadFromXML(trackingNode);
            }
            else
            {
                Logger.Log("Wrong node type given");
            }

            return null;
        }

    }
}
