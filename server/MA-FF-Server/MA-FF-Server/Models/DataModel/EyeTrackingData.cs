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
    /// <summary>
    /// Class for the eyetracking data
    /// </summary>
    public class EyeTrackingData : BaseTrackingData
    {
        /// <summary>
        /// Diameter of the eye
        /// </summary>
        private double _diameter;

        /// <summary>
        /// eye position x
        /// </summary>
        private double _eyePostionX;

        /// <summary>
        /// eye position y
        /// </summary>
        private double _eyePostionY;

        /// <summary>
        /// eye position z
        /// </summary>
        private double _eyePostionZ;

        /// <summary>
        /// Constructor for loading from xml
        /// </summary>
        public EyeTrackingData() : base()
        {

        }

        /// <summary>
        /// Constructor with x and y coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public EyeTrackingData(double x, double y) : base(x,y)
        {
        }

        /// <summary>
        /// Constructor with x and y coordinates and the callback timestamp
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="callbackTimestamp">Callback timestamp</param>
        public EyeTrackingData(double x, double y, String callbackTimestamp)
            : base(x, y, callbackTimestamp)
        {
            
        }

        /// <summary>
        /// Getter/ Setter for the eye diameter
        /// </summary>
        public Double Diameter
        {
            get { return _diameter; }
            set { _diameter = value; }
        }

        /// <summary>
        /// Getter/ Setter for the x position of the eye
        /// </summary>
        public Double EyePositionX
        {
            get { return _eyePostionX; }
            set { _eyePostionX = value; }
        }

        /// <summary>
        /// Getter/ Setter for the y position of the eye
        /// </summary>
        public Double EyePositionY
        {
            get { return _eyePostionY; }
            set { _eyePostionY = value; }
        }

        /// <summary>
        /// Getter/ Setter for the z position of the eye
        /// </summary>
        public Double EyePositionZ
        {
            get { return _eyePostionZ; }
            set { _eyePostionZ = value; }
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="etNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
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
    }
}
