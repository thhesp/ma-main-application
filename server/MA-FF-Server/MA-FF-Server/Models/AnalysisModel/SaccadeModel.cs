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
    /// <summary>
    /// Model for saccades
    /// </summary>
    public class SaccadeModel
    {

        /// <summary>
        /// Start timestamp of the saccade
        /// </summary>
        private String _startTimestamp;

        /// <summary>
        /// End timestamp of the saccade
        /// </summary>
        private String _endTimestamp;

        /// <summary>
        /// duration of the saccade
        /// </summary>
        private long _duration = 0;

        /// <summary>
        /// Eye to which the saccade belongs
        /// </summary>
        private String _eye;

        /// <summary>
        /// related gazes of the saccade
        /// </summary>
        private List<GazeModel> _relatedGazes = new List<GazeModel>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startTimestamp">Start timestamp of the saccade</param>
        /// <param name="endTimestamp">End timestamp of the saccade</param>
        /// <param name="duration">Duration of the saccade</param>
        /// <param name="eye">Eye to which the saccade belongs</param>
        public SaccadeModel(String startTimestamp, String endTimestamp, long duration, String eye)
        {
            _startTimestamp = startTimestamp;
            _endTimestamp = endTimestamp;
            _duration = duration;
            _eye = eye;
        }

        /// <summary>
        /// Getter/Setter for the related gazes
        /// </summary>
        public List<GazeModel> RelatedGazes
        {
            get { return _relatedGazes; }
            set { _relatedGazes = value; }
        }

        /// <summary>
        /// Method for adding a gaze to the list of the related gazes
        /// </summary>
        /// <param name="model">The gaze to add</param>
        public void AddRelatedGaze(GazeModel model)
        {
            _relatedGazes.Add(model);
        }

        /// <summary>
        /// Creates an XML Representation of the saccade
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <param name="includeSingleGazeData">Shall data about the related gazes be included</param>
        /// <returns>XMLNode which contains the representation of the model</returns>
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

            XmlAttribute countOfGazes = xmlDoc.CreateAttribute("count-of-gazes");

            countOfGazes.Value = this.RelatedGazes.Count.ToString();

            saccadeNode.Attributes.Append(countOfGazes);

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
    }
}
