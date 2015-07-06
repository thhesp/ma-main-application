using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebAnalyzer.Models.DataModel
{
    public class EyeTrackingData
    {

        private double _x;
        private double _y;

        private String _callbackTimestamp;

        public EyeTrackingData(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public EyeTrackingData(double x, double y, String callbackTimestamp) : this(x,y)
        {
            _callbackTimestamp = callbackTimestamp;
        }

        #region GetterSetterFunctions

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

        #endregion

        #region XMLFunctions

        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode eyetrackingDataNode = xmlDoc.CreateElement("eyetracking-data");

            //x

            XmlAttribute xPosition = xmlDoc.CreateAttribute("x");

            xPosition.Value = this.X.ToString();

            eyetrackingDataNode.Attributes.Append(xPosition);

            //y

            XmlAttribute yPosition = xmlDoc.CreateAttribute("y");

            yPosition.Value = this.Y.ToString();

            eyetrackingDataNode.Attributes.Append(yPosition);

            //callback  timestamp

            XmlAttribute callbackTimestamp = xmlDoc.CreateAttribute("callback");

            callbackTimestamp.Value = this.CallbackTimestamp;

            eyetrackingDataNode.Attributes.Append(callbackTimestamp);

            return eyetrackingDataNode;
        }

        #endregion
    }
}
