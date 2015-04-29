using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebAnalyzer.Models.DataModel
{
    class EyeTrackingData
    {

        private double _x;
        private double _y;

        private String _callbackTimestamp;

        private String _startTime;
        private String _endTime;
        private String _duration;

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

        public String StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        public String EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        public String Duration
        {
            get { return _duration; }
            set { _duration = value; }
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

            //eyetracking data start timestamp

            XmlAttribute startTimestamp = xmlDoc.CreateAttribute("start");

            startTimestamp.Value = this.StartTime;

            eyetrackingDataNode.Attributes.Append(startTimestamp);

            //eyetracking end timestamp

            XmlAttribute endTimestamp = xmlDoc.CreateAttribute("end");

            endTimestamp.Value = this.EndTime;

            eyetrackingDataNode.Attributes.Append(endTimestamp);

            //eyetracking duration

            XmlAttribute duration = xmlDoc.CreateAttribute("duration");

            duration.Value = this.Duration;

            eyetrackingDataNode.Attributes.Append(duration);

            return eyetrackingDataNode;
        }

        #endregion
    }
}
