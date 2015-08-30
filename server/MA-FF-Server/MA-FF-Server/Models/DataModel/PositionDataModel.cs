using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Models.DataModel
{
    public class PositionDataModel
    {


        private EyeTrackingData _eyeTrackingData;

        private DOMElementModel _element;

        public PositionDataModel()
        {
        }

        public PositionDataModel(double x, double y) : this()
        {
            _eyeTrackingData = new EyeTrackingData(x, y);
        }

        #region GetterSetterFunctions


        public EyeTrackingData EyeTrackingData
        {
            get { return _eyeTrackingData; }
            set { _eyeTrackingData = value; }
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

            positionNode.AppendChild(this.EyeTrackingData.ToXML(xmlDoc));

            //element data

            positionNode.AppendChild(this.Element.ToXML(xmlDoc));

            return positionNode;
        }

        public static PositionDataModel LoadFromXML(XmlNode posNode)
        {
            PositionDataModel posModel = new PositionDataModel();

            foreach (XmlNode child in posNode.ChildNodes)
            {
                if (child.Name == "eyetracking-data")
                {
                    EyeTrackingData etData = EyeTrackingData.LoadFromXML(child);

                    if (etData != null)
                    {
                        posModel.EyeTrackingData = etData;
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

        #endregion
    }
}
