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
    /// Class for mousetracking data
    /// </summary>
    public class MouseTrackingData : BaseTrackingData
    {
        /// <summary>
        /// Constructor for loading from xml
        /// </summary>
        public MouseTrackingData() : base()
        {

        }

        /// <summary>
        /// Constructor with x and y coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public MouseTrackingData(double x, double y) : base(x,y)
        {
        }

        /// <summary>
        /// Constructor with x and y coordinates and the callback timestamp
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="callbackTimestamp">Callback timestamp</param>
        public MouseTrackingData(double x, double y, String callbackTimestamp)
            : base(x, y, callbackTimestamp)
        {
            
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public override XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode eyetrackingDataNode = xmlDoc.CreateElement("mousetracking-data");

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

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="etNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
        public static MouseTrackingData LoadFromXML(XmlNode etNode)
        {
            if (etNode.Name == "mousetracking-data")
            {
                MouseTrackingData mouseData = new MouseTrackingData();

                foreach (XmlAttribute attr in etNode.Attributes)
                {
                    switch (attr.Name)
                    {
                        case "x":
                            mouseData.X = Double.Parse(attr.Value);
                            break;
                        case "y":
                            mouseData.Y = Double.Parse(attr.Value);
                            break;
                        case "callback":
                            mouseData.CallbackTimestamp = attr.Value;
                            break;
                    }
                }

                return mouseData;

            }

            Logger.Log("Wrong node type given");

            return null;
        }
    }
}
