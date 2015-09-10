using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Models.SettingsModel;

namespace WebAnalyzer.Models.AnalysisModel
{
    public class SaccadeModel
    {

        private String _startTimestamp;
        private String _endTimestamp;

        private long _duration = 0;

        private String _eye;

        private List<GazeModel> _relatedGazes = new List<GazeModel>();

        public SaccadeModel(String startTimestamp, String endTimestamp, long duration, String eye)
        {
            _startTimestamp = startTimestamp;
            _endTimestamp = endTimestamp;
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

        #endregion

        #region XMLFunctions

        public XmlNode ToXML(XmlDocument xmlDoc, Boolean includeSingleGazeData)
        {
            XmlNode saccadeNode = xmlDoc.CreateElement("saccade");

            XmlAttribute startTimestamp = xmlDoc.CreateAttribute("start-timestamp");

            startTimestamp.Value = _startTimestamp;

            saccadeNode.Attributes.Append(startTimestamp);

            XmlAttribute endTimestamp = xmlDoc.CreateAttribute("end-timestamp");

            endTimestamp.Value = _endTimestamp;

            saccadeNode.Attributes.Append(endTimestamp);

            XmlAttribute duration = xmlDoc.CreateAttribute("duration");

            duration.Value = _duration.ToString();

            saccadeNode.Attributes.Append(duration);

            if (includeSingleGazeData)
            {
                XmlNode gazesNode = xmlDoc.CreateElement("related-gazes");

                foreach (GazeModel gaze in _relatedGazes)
                {
                    gazesNode.AppendChild(gaze.ToXML(xmlDoc));
                }

                saccadeNode.AppendChild(gazesNode);
            }


            return saccadeNode;
        }


        #endregion
    }
}
