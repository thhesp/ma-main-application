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
    /// Represents one position which was requested. Contains the tracking data as well as the element information
    /// </summary>
    public class PositionDataModel
    {
        /// <summary>
        /// Tracking data from the tracking component
        /// </summary>
        private BaseTrackingData _trackingData;

        /// <summary>
        /// Element data
        /// </summary>
        private DOMElementModel _element;

        /// <summary>
        /// Constructor for loading from xml
        /// </summary>
        public PositionDataModel()
        {
        }

        /// <summary>
        /// Getter/ Setter for the tracking data
        /// </summary>
        public BaseTrackingData TrackingData
        {
            get { return _trackingData; }
            set { _trackingData = value; }
        }

        /// <summary>
        /// Getter / Setter for the element
        /// </summary>
        public DOMElementModel Element
        {
            get { return _element; }
            set { _element = value; }
        }


        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode positionNode = xmlDoc.CreateElement("position");

            //eyetracking data

            positionNode.AppendChild(this.TrackingData.ToXML(xmlDoc));

            //element data

            positionNode.AppendChild(this.Element.ToXML(xmlDoc));

            return positionNode;
        }

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="posNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
        public static PositionDataModel LoadFromXML(XmlNode posNode)
        {
            if (posNode.Name == "position")
            {
                PositionDataModel posModel = new PositionDataModel();

                foreach (XmlNode child in posNode.ChildNodes)
                {
                    if (child.Name == "eyetracking-data" || child.Name == "mousetracking-data")
                    {
                        BaseTrackingData data = BaseTrackingData.LoadFromXML(child);

                        if (data != null)
                        {
                            posModel.TrackingData = data;
                        }
                    }
                    else if (child.Name == "element")
                    {
                        DOMElementModel domModel = DOMElementModel.LoadFromXML(child);

                        if (domModel != null)
                        {
                            posModel.Element = domModel;
                        }
                    }
                }

                return posModel;
            }

            Logger.Log("Wrong node type given");

            return null;
        }
    }
}
