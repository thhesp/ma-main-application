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
    /// <summary>
    /// Base for all tracking data
    /// </summary>
    public abstract class BaseTrackingData
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        protected double _x;

        /// <summary>
        /// Y coordinate
        /// </summary>
        protected double _y;

        /// <summary>
        /// Callback timestamp
        /// </summary>
        protected String _callbackTimestamp;

        /// <summary>
        /// Empty constructor used for loading
        /// </summary>
        public BaseTrackingData()
        {

        }

        /// <summary>
        /// Constructor with only coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public BaseTrackingData(double x, double y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Constructor with coordinates and timestamp
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="callbackTimestamp">Callback Timestamp</param>
        public BaseTrackingData(double x, double y, String callbackTimestamp)
            : this(x, y)
        {
            _callbackTimestamp = callbackTimestamp;
        }

        /// <summary>
        /// Getter/ Setter for the X Coordinate
        /// </summary>
        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Getter/ Setter for the Y Coordinate
        /// </summary>
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Getter/ Setter for the timestamp
        /// </summary>
        public String CallbackTimestamp
        {
            get { return _callbackTimestamp; }
            set { _callbackTimestamp = value; }
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public abstract XmlNode ToXML(XmlDocument xmlDoc);

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="trackingNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
        /// <remarks>Checks for the implemented child classes and calls their method</remarks>
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
