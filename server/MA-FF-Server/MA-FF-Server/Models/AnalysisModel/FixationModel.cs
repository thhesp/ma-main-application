using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.AnalysisModel
{
    class FixationModel
    {

        private long _duration = 0;

        private String _eye;

        private double _startX, _endX, _startY, _endY;

        private List<GazeModel> _relatedGazes = new List<GazeModel>();


        public FixationModel(long duration, String eye)
        {
            _duration = duration;
            _eye = eye;
        }

        #region GetterSetter

        public List<GazeModel> RelatedGazes
        {
            get { return _relatedGazes; }
            set { _relatedGazes = value; }
        }

        public void AddRelatedGaze(GazeModel model)
        {
            _relatedGazes.Add(model);
        }

        public void From(double startX, double startY)
        {
            _startX = startX;
            _startY = startY;
        }

        public void To(double endX, double endY)
        {
            _endX = endX;
            _endY = endY;
        }

        public long Duration
        {
            get { return _duration; }
        }

        public double StartX
        {
            get { return _startX; }
        }

        public double StartY
        {
            get { return _startY; }
        }

        public double EndX
        {
            get { return _endX; }
        }

        public double EndY
        {
            get { return _endY; }
        }

        public List<GazeModel> GetRelatedGazes()
        {
            return _relatedGazes;
        }

        #endregion

        #region XMLFunctions

        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode fixationNode = xmlDoc.CreateElement("fixation");


            XmlAttribute duration = xmlDoc.CreateAttribute("duration");

            duration.Value = this.Duration.ToString();

            fixationNode.Attributes.Append(duration);



            XmlAttribute startX = xmlDoc.CreateAttribute("startX");

            startX.Value = this.StartX.ToString();

            fixationNode.Attributes.Append(startX);

            XmlAttribute startY = xmlDoc.CreateAttribute("startY");

            startY.Value = this.StartY.ToString();

            fixationNode.Attributes.Append(startY);

            XmlAttribute endX = xmlDoc.CreateAttribute("endX");

            endX.Value = this.EndX.ToString();

            fixationNode.Attributes.Append(endX);

            XmlAttribute endY = xmlDoc.CreateAttribute("endY");

            endY.Value = this.EndY.ToString();

            fixationNode.Attributes.Append(endY);


            XmlNode gazesNode = xmlDoc.CreateElement("related-gazes");

            foreach (GazeModel gaze in _relatedGazes)
            {
                gazesNode.AppendChild(gaze.ToXML(xmlDoc));
            }

            fixationNode.AppendChild(gazesNode);

            return fixationNode;
        }

        #endregion
    }
}
