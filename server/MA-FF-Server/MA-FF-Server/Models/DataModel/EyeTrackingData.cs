using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Util;
using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Models.DataModel
{
    public class EyeTrackingData : BaseTrackingData
    {

        private double _diameter;

        private double _eyePostionX;
        private double _eyePostionY;
        private double _eyePostionZ;

        public EyeTrackingData() : base()
        {

        }

        public EyeTrackingData(double x, double y) : base(x,y)
        {
        }

        public EyeTrackingData(double x, double y, String callbackTimestamp)
            : base(x, y, callbackTimestamp)
        {
            
        }

        public Double Diameter
        {
            get { return _diameter; }
            set { _diameter = value; }
        }

        public Double EyePositionX
        {
            get { return _eyePostionX; }
            set { _eyePostionX = value; }
        }

        public Double EyePositionY
        {
            get { return _eyePostionY; }
            set { _eyePostionY = value; }
        }

        public Double EyePositionZ
        {
            get { return _eyePostionZ; }
            set { _eyePostionZ = value; }
        }

        #region XMLFunctions

        public override XmlNode ToXML(XmlDocument xmlDoc)
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

            //diameter
            XmlAttribute diameter = xmlDoc.CreateAttribute("diameter");

            diameter.Value = this.Diameter.ToString();

            eyetrackingDataNode.Attributes.Append(diameter);

            //eyePositionX
            XmlAttribute positionX = xmlDoc.CreateAttribute("eye-position-x");

            positionX.Value = this.EyePositionX.ToString();

            eyetrackingDataNode.Attributes.Append(positionX);

            //eyePositionY
            XmlAttribute positionY = xmlDoc.CreateAttribute("eye-position-y");

            positionY.Value = this.EyePositionY.ToString();

            eyetrackingDataNode.Attributes.Append(positionY);

            //eyePositionZ
            XmlAttribute positionZ = xmlDoc.CreateAttribute("eye-position-z");

            positionZ.Value = this.EyePositionZ.ToString();

            eyetrackingDataNode.Attributes.Append(positionZ);

            return eyetrackingDataNode;
        }

        public static EyeTrackingData LoadFromXML(XmlNode etNode)
        {
            if (etNode.Name == "eyetracking-data")
            {
                EyeTrackingData etData = new EyeTrackingData();

                foreach (XmlAttribute attr in etNode.Attributes)
                {
                    switch (attr.Name)
                    {
                        case "x":
                            etData.X = Double.Parse(attr.Value);
                            break;
                        case "y":
                            etData.Y = Double.Parse(attr.Value);
                            break;
                        case "callback":
                            etData.CallbackTimestamp = attr.Value;
                            break;
                        case "diameter":
                            etData.Diameter = Double.Parse(attr.Value);
                            break;
                        case "eye-position-x":
                            etData.EyePositionX = Double.Parse(attr.Value);
                            break;
                        case "eye-position-y":
                            etData.EyePositionY = Double.Parse(attr.Value);
                            break;
                        case "eye-position-z":
                            etData.EyePositionZ = Double.Parse(attr.Value);
                            break;
                    }
                }

                return etData;

            }

            Logger.Log("Wrong node type given");

            return null;
        }
        #endregion
    }
}
