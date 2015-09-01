using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Models.AnalysisModel;
using WebAnalyzer.Util;
using WebAnalyzer.Models.SettingsModel;

namespace WebAnalyzer.Models.DataModel
{
    public class WebpageModel
    {

        private String _url;
        private String _visitTimestamp;
        private List<GazeModel> _positionData = new List<GazeModel>();

        /* Fixations */
        private List<FixationModel> _leftFixationData = new List<FixationModel>();
        private List<FixationModel> _rightFixationData = new List<FixationModel>();

        public WebpageModel(String url, String visitTimestamp)
        {
            _url = url;
            _visitTimestamp = visitTimestamp;
        }

        public WebpageModel()
        {

        }

        #region GetterSetterFunctions

        public String Url
        {
            get { return _url; }
            set { _url = value;}
        }

        public String VisitTimestamp
        {
            get { return _visitTimestamp; }
            set { _visitTimestamp = value; }
        }

        public List<GazeModel> Gazes
        {
            get { return _positionData; }
        }

        public void AddGazeData(GazeModel data)
        {
            _positionData.Add(data);
        }

        #endregion

        #region XMLFunctions

        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode webpageNode = xmlDoc.CreateElement("webpage");

            XmlAttribute url = xmlDoc.CreateAttribute("url");

            url.Value = this.Url;

            webpageNode.Attributes.Append(url);

            XmlAttribute visited = xmlDoc.CreateAttribute("visited");

            visited.Value = this.VisitTimestamp;

            webpageNode.Attributes.Append(visited);

            foreach (GazeModel data in _positionData)
            {
                webpageNode.AppendChild(data.ToXML(xmlDoc));
            }

            return webpageNode;
        }

        public static WebpageModel LoadFromXML(XmlNode webpageNode)
        {
            WebpageModel page = new WebpageModel();

            foreach (XmlAttribute attr in webpageNode.Attributes)
            {
                switch (attr.Name)
                {
                    case "url":
                        page.Url = attr.Value;
                        break;
                    case "visited":
                        page.VisitTimestamp = attr.Value;
                        break;
                }
            }

            foreach (XmlNode child in webpageNode.ChildNodes)
            {
                GazeModel gaze = GazeModel.LoadFromXML(child);

                if (gaze != null)
                {
                    page.AddGazeData(gaze);
                }
            }

            return page;
        }

        public XmlNode GenerateStatisticsXML(XmlDocument xmlDoc)
        {
            XmlNode webpageNode = xmlDoc.CreateElement("webpage");

            XmlAttribute url = xmlDoc.CreateAttribute("url");

            url.Value = this.Url;

            webpageNode.Attributes.Append(url);

            XmlAttribute visited = xmlDoc.CreateAttribute("visited");

            visited.Value = this.VisitTimestamp;

            webpageNode.Attributes.Append(visited);

            XmlAttribute nrOfGazes = xmlDoc.CreateAttribute("number-of-gazes");

            nrOfGazes.Value = Gazes.Count.ToString();

            webpageNode.Attributes.Append(nrOfGazes);

            // create & insert statistics?

            webpageNode.AppendChild(CreateWebpageStatistics(xmlDoc));

            XmlNode gazesNode = xmlDoc.CreateElement("gazes");

            foreach (GazeModel data in _positionData)
            {
                gazesNode.AppendChild(data.GenerateStatisticsXML(xmlDoc));
            }

            webpageNode.AppendChild(gazesNode);


            return webpageNode;
        }

        private XmlNode CreateWebpageStatistics(XmlDocument xmlDoc)
        {
            // sum of statistics?
            XmlNode statsNode = xmlDoc.CreateElement("webpage-stats");

            XmlNode requestTillSent = xmlDoc.CreateElement("request-till-sent");

            InsertArrayStatistics(xmlDoc, requestTillSent, StatisticsAnalyser.ArrayOfDurationFromRequestTillSent(_positionData));

            statsNode.AppendChild(requestTillSent);

            XmlNode serverSentToReceived = xmlDoc.CreateElement("server-sent-to-received");

            InsertArrayStatistics(xmlDoc, serverSentToReceived, StatisticsAnalyser.ArrayOfDurationFromServerSentToReceived(_positionData));

            statsNode.AppendChild(serverSentToReceived);

            XmlNode clientReceivedToSent = xmlDoc.CreateElement("client-received-to-sent");

            InsertArrayStatistics(xmlDoc, clientReceivedToSent, StatisticsAnalyser.ArrayOfDurationFromClientReceivedToClientSent(_positionData));

            statsNode.AppendChild(clientReceivedToSent);


            return statsNode;
        }

        private void InsertArrayStatistics(XmlDocument xmlDoc, XmlNode node, long[] array)
        {
            XmlAttribute mean = xmlDoc.CreateAttribute("mean");

            mean.Value = Statistics.CalculateMean(array).ToString();

            node.Attributes.Append(mean);

            XmlAttribute median = xmlDoc.CreateAttribute("median");

            median.Value = Statistics.CalculateMedian(array).ToString();

            node.Attributes.Append(median);

            XmlAttribute min = xmlDoc.CreateAttribute("min");

            min.Value = array.Min().ToString();

            node.Attributes.Append(min);


            XmlAttribute max = xmlDoc.CreateAttribute("max");

            max.Value = array.Max().ToString();

            node.Attributes.Append(max);

        }

        public XmlNode GenerateFixationXML(XmlDocument xmlDoc)
        {
            ExtractFixations();

            XmlNode webpageNode = xmlDoc.CreateElement("webpage");

            XmlAttribute url = xmlDoc.CreateAttribute("url");

            url.Value = this.Url;

            webpageNode.Attributes.Append(url);

            XmlAttribute visited = xmlDoc.CreateAttribute("visited");

            visited.Value = this.VisitTimestamp;

            webpageNode.Attributes.Append(visited);


            // insert fixations

            XmlNode fixationsNode = xmlDoc.CreateElement("fixations");

            XmlNode leftEyeNode = xmlDoc.CreateElement("left-eye");

            XmlAttribute leftFixationsCount = xmlDoc.CreateAttribute("count-of-fixations");

            leftFixationsCount.Value = _leftFixationData.Count.ToString();

            leftEyeNode.Attributes.Append(leftFixationsCount);

            foreach (FixationModel data in _leftFixationData)
            {
                leftEyeNode.AppendChild(data.ToXML(xmlDoc));
            }

            fixationsNode.AppendChild(leftEyeNode);

            XmlNode rightEyeNode = xmlDoc.CreateElement("right-eye");

            XmlAttribute rightFixationsCount = xmlDoc.CreateAttribute("count-of-fixations");

            rightFixationsCount.Value = _rightFixationData.Count.ToString();

            rightEyeNode.Attributes.Append(rightFixationsCount);

            foreach (FixationModel data in _rightFixationData)
            {
                rightEyeNode.AppendChild(data.ToXML(xmlDoc));
            }

            fixationsNode.AppendChild(rightEyeNode);

            webpageNode.AppendChild(fixationsNode);


            return webpageNode;
        }

        public XmlNode GenerateAOIXML(ExperimentSettings settings, XmlDocument xmlDoc)
        {
            ExtractFixations();

            XmlNode webpageNode = xmlDoc.CreateElement("webpage");

            XmlAttribute url = xmlDoc.CreateAttribute("url");

            url.Value = this.Url;

            webpageNode.Attributes.Append(url);

            XmlAttribute visited = xmlDoc.CreateAttribute("visited");

            visited.Value = this.VisitTimestamp;

            webpageNode.Attributes.Append(visited);


            // insert fixations

            XmlNode aoisNode = xmlDoc.CreateElement("aois");

            XmlNode leftEyeNode = xmlDoc.CreateElement("left-eye");

            XmlAttribute leftFixationsCount = xmlDoc.CreateAttribute("count-of-fixations");

            leftFixationsCount.Value = _leftFixationData.Count.ToString();

            leftEyeNode.Attributes.Append(leftFixationsCount);

            foreach (FixationModel data in _leftFixationData)
            {
                XmlNode node = data.ToAOIXML(settings, this, "left", xmlDoc);

                if (node != null)
                {
                    leftEyeNode.AppendChild(node);
                }
            }

            aoisNode.AppendChild(leftEyeNode);

            XmlNode rightEyeNode = xmlDoc.CreateElement("right-eye");

            XmlAttribute rightFixationsCount = xmlDoc.CreateAttribute("count-of-fixations");

            rightFixationsCount.Value = _rightFixationData.Count.ToString();

            rightEyeNode.Attributes.Append(rightFixationsCount);

            foreach (FixationModel data in _rightFixationData)
            {

                XmlNode node = data.ToAOIXML(settings, this, "right", xmlDoc);

                if (node != null)
                {
                    leftEyeNode.AppendChild(node);
                }
            }

            aoisNode.AppendChild(rightEyeNode);

            webpageNode.AppendChild(aoisNode);


            return webpageNode;
        }

        #endregion


        #region FixationFunctions

        private void ExtractFixations(double acceptableDeviations)
        {
            _leftFixationData = FixationAnalyser.ExtractFixations(_positionData, "left", 100, acceptableDeviations);
            _rightFixationData = FixationAnalyser.ExtractFixations(_positionData, "right", 100, acceptableDeviations);

        }

        private void ExtractFixations()
        {
            //this.ExtractFixations(0);

            _leftFixationData = FixationAnalyser.ExtractFixations(_positionData, "left", 100, 0);
            _rightFixationData = FixationAnalyser.ExtractFixations(_positionData, "right", 100, 0);
        }

        #endregion
    }
}
