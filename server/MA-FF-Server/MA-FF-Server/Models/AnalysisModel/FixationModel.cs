using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Models.SettingsModel;
using WebAnalyzer.Util;

namespace WebAnalyzer.Models.AnalysisModel
{
    /// <summary>
    /// Model for fixations
    /// </summary>
    public class FixationModel
    {


        /// <summary>
        /// Start timestamp of the fixation
        /// </summary>
        private String _startTimestamp;

        /// <summary>
        /// End timestamp of the fixation
        /// </summary>
        private String _endTimestamp;

        /// <summary>
        /// duration of the fixation
        /// </summary>
        private long _duration = 0;

        /// <summary>
        /// Eye to which the fixation belongs
        /// </summary>
        private String _eye;

        /// <summary>
        /// Coordinates in which the fixation is contained
        /// </summary>
        private double _startX, _endX, _startY, _endY;


        /// <summary>
        /// related gazes of the fixation
        /// </summary>
        private List<GazeModel> _relatedGazes = new List<GazeModel>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startTimestamp">Start timestamp of the fixation</param>
        /// <param name="endTimestamp">End timestamp of the fixation</param>
        /// <param name="duration">Duration of the fixation</param>
        /// <param name="eye">Eye to which the fixation belongs</param>
        public FixationModel(String startTimestamp, String endTimestamp, long duration, String eye)
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
        /// Start for the fixation coordinates
        /// </summary>
        /// <param name="startX">start x coordinate</param>
        /// <param name="startY">start y coordinate</param>
        public void From(double startX, double startY)
        {
            _startX = startX;
            _startY = startY;
        }

        /// <summary>
        /// End for the fixation coordinates
        /// </summary>
        /// <param name="endX">end x coordinate</param>
        /// <param name="endY">end y coordinate</param>
        public void To(double endX, double endY)
        {
            _endX = endX;
            _endY = endY;
        }

        /// <summary>
        /// Getter for the duration of the fixation
        /// </summary>
        public long Duration
        {
            get { return _duration; }
        }

        /// <summary>
        /// Getter for the start x coordinate of the fixation
        /// </summary>
        public double StartX
        {
            get { return _startX; }
        }

        /// <summary>
        /// Getter for the start y coordinate of the fixation
        /// </summary>
        public double StartY
        {
            get { return _startY; }
        }

        /// <summary>
        /// Getter for the end x coordinate of the fixation
        /// </summary>
        public double EndX
        {
            get { return _endX; }
        }

        /// <summary>
        /// Getter for the end y coordinate of the fixation
        /// </summary>
        public double EndY
        {
            get { return _endY; }
        }

        /// <summary>
        /// Getter for the related gazes
        /// </summary>
        /// <returns>List of Gazes</returns>
        public List<GazeModel> GetRelatedGazes()
        {
            return _relatedGazes;
        }

        /// <summary>
        /// Creates an XML Representation of the fixation
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <param name="includeSingleGazeData">Shall data about the related gazes be included</param>
        /// <returns>XMLNode which contains the representation of the model</returns>
        public XmlNode ToXML(XmlDocument xmlDoc, Boolean includeSingleGazeData)
        {
            XmlNode fixationNode = xmlDoc.CreateElement("fixation");

            XmlAttribute startTimestamp = xmlDoc.CreateAttribute("start-timestamp");

            startTimestamp.Value = _startTimestamp;

            fixationNode.Attributes.Append(startTimestamp);

            XmlAttribute endTimestamp = xmlDoc.CreateAttribute("end-timestamp");

            endTimestamp.Value = _endTimestamp;

            fixationNode.Attributes.Append(endTimestamp);

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

            XmlAttribute countOfGazes = xmlDoc.CreateAttribute("count-of-gazes");

            countOfGazes.Value = this.RelatedGazes.Count.ToString();

            fixationNode.Attributes.Append(countOfGazes);

            if (includeSingleGazeData)
            {
                XmlNode gazesNode = xmlDoc.CreateElement("related-gazes");

                foreach (GazeModel gaze in _relatedGazes)
                {
                    gazesNode.AppendChild(gaze.ToXML(xmlDoc));
                }

                fixationNode.AppendChild(gazesNode);
            }

            return fixationNode;
        }

        /// <summary>
        /// Creates an XML Representation of the aois
        /// </summary>
        /// <param name="settings">Experiment settings which are used for the aois</param>
        /// <param name="page">Webpage to which the gazes belong</param>
        /// <param name="eye">Eye to which the gazes belong</param>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <param name="includeSingleGazeData">Shall data about the related gazes be included</param>
        /// <returns>XMLNode which contains the representation of the aois for these fixations</returns>
        public XmlNode ToAOIXML(ExperimentSettings settings, WebpageModel page, String eye, XmlDocument xmlDoc, Boolean includeSingleGazeData)
        {

            String[] aois = new String[0];

            if (GetRelatedGazes().Count == 0)
            {
                return null;
            }

            if(eye == "left"){
                 aois = settings.GetAOIs(page, GetRelatedGazes()[0].LeftEye.Element);
            }else if(eye == "right"){
                 aois = settings.GetAOIs(page, GetRelatedGazes()[0].RightEye.Element);
            }

            if(aois.Length == 0){
                Logger.Log("No aois found?");
                return null;
            }

            XmlNode aoisNode = xmlDoc.CreateElement("aois");

            XmlAttribute identifier = xmlDoc.CreateAttribute("identifiers");

            identifier.Value = string.Join(",", aois);

            aoisNode.Attributes.Append(identifier);

            XmlAttribute startTimestamp = xmlDoc.CreateAttribute("start-timestamp");

            startTimestamp.Value = _startTimestamp;

            aoisNode.Attributes.Append(startTimestamp);

            XmlAttribute endTimestamp = xmlDoc.CreateAttribute("end-timestamp");

            endTimestamp.Value = _endTimestamp;

            aoisNode.Attributes.Append(endTimestamp);

            XmlAttribute duration = xmlDoc.CreateAttribute("duration");

            duration.Value = this.Duration.ToString();

            aoisNode.Attributes.Append(duration);


            XmlAttribute startX = xmlDoc.CreateAttribute("startX");

            startX.Value = this.StartX.ToString();

            aoisNode.Attributes.Append(startX);

            XmlAttribute startY = xmlDoc.CreateAttribute("startY");

            startY.Value = this.StartY.ToString();

            aoisNode.Attributes.Append(startY);

            XmlAttribute endX = xmlDoc.CreateAttribute("endX");

            endX.Value = this.EndX.ToString();

            aoisNode.Attributes.Append(endX);

            XmlAttribute endY = xmlDoc.CreateAttribute("endY");

            endY.Value = this.EndY.ToString();

            aoisNode.Attributes.Append(endY);

            XmlAttribute countOfGazes = xmlDoc.CreateAttribute("count-of-gazes");

            countOfGazes.Value = this.RelatedGazes.Count.ToString();

            aoisNode.Attributes.Append(countOfGazes);

            if (includeSingleGazeData)
            {
                XmlNode gazesNode = xmlDoc.CreateElement("related-gazes");

                foreach (GazeModel gaze in _relatedGazes)
                {
                    gazesNode.AppendChild(gaze.ToXML(xmlDoc));
                }

                aoisNode.AppendChild(gazesNode);
            }

            return aoisNode;
        }
    }
}
