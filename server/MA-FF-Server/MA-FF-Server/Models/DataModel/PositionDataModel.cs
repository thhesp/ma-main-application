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
    public class PositionDataModel
    {

        private BaseTrackingData _trackingData;

        private DOMElementModel _element;

        public PositionDataModel()
        {
        }

        #region GetterSetterFunctions


        public BaseTrackingData TrackingData
        {
            get { return _trackingData; }
            set { _trackingData = value; }
        }

        public DOMElementModel Element
        {
            get { return _element; }
            set { _element = value; }
        }

        #endregion

        #region XMLFunctions

        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode positionNode = xmlDoc.CreateElement("position");

            //eyetracking data

            positionNode.AppendChild(this.TrackingData.ToXML(xmlDoc));

            //element data

            positionNode.AppendChild(this.Element.ToXML(xmlDoc));

            return positionNode;
        }

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

        #endregion
    }
}
