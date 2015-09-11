using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Models.AnalysisModel;
using WebAnalyzer.Util;
using WebAnalyzer.Models.SettingsModel;
using WebAnalyzer.Models.AlgorithmModel;

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

        /* Saccades */
        private List<SaccadeModel> _leftSaccadesData = new List<SaccadeModel>();
        private List<SaccadeModel> _rightSaccadesnData = new List<SaccadeModel>();

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

        public List<FixationModel> LeftFixationData
        {
            get { return _leftFixationData; }
            set { _leftFixationData = value; }
        }

        public List<FixationModel> RightFixationData
        {
            get { return _rightFixationData; }
            set { _rightFixationData = value; }
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

            XmlAttribute numberOfGazes = xmlDoc.CreateAttribute("number-of-gazes");

            numberOfGazes.Value = _positionData.Count.ToString();

            webpageNode.Attributes.Append(numberOfGazes);

            foreach (GazeModel data in _positionData)
            {
                webpageNode.AppendChild(data.ToXML(xmlDoc));
            }

            return webpageNode;
        }

        public static WebpageModel LoadFromXML(XmlNode webpageNode)
        {
            if (webpageNode.Name == "webpage")
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

            Logger.Log("Wrong node type given");

            return null;
        }

        public XmlNode GenerateStatisticsXML(XmlDocument xmlDoc, Boolean includeSingleGazeData)
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

            if (includeSingleGazeData)
            {
                XmlNode gazesNode = xmlDoc.CreateElement("gazes");

                foreach (GazeModel data in _positionData)
                {
                    gazesNode.AppendChild(data.GenerateStatisticsXML(xmlDoc));
                }

                webpageNode.AppendChild(gazesNode);
            }

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

        public XmlNode GenerateFixationXML(XmlDocument xmlDoc, Boolean includeSingleGazeData)
        {
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
                leftEyeNode.AppendChild(data.ToXML(xmlDoc, includeSingleGazeData));
            }

            fixationsNode.AppendChild(leftEyeNode);

            XmlNode rightEyeNode = xmlDoc.CreateElement("right-eye");

            XmlAttribute rightFixationsCount = xmlDoc.CreateAttribute("count-of-fixations");

            rightFixationsCount.Value = _rightFixationData.Count.ToString();

            rightEyeNode.Attributes.Append(rightFixationsCount);

            foreach (FixationModel data in _rightFixationData)
            {
                rightEyeNode.AppendChild(data.ToXML(xmlDoc, includeSingleGazeData));
            }

            fixationsNode.AppendChild(rightEyeNode);

            webpageNode.AppendChild(fixationsNode);


            return webpageNode;
        }

        public XmlNode GenerateAOIXML(ExperimentSettings settings, XmlDocument xmlDoc, Boolean includeSingleGazeData)
        {
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

            int countLeftAOI = 0;

            foreach (FixationModel data in _leftFixationData)
            {
                XmlNode node = data.ToAOIXML(settings, this, "left", xmlDoc, includeSingleGazeData);

                if (node != null)
                {
                    leftEyeNode.AppendChild(node);
                    countLeftAOI++;
                }
            }

            aoisNode.AppendChild(leftEyeNode);

            XmlAttribute leftAOICount = xmlDoc.CreateAttribute("count-of-aois");

            leftAOICount.Value = countLeftAOI.ToString();

            leftEyeNode.Attributes.Append(leftAOICount);

            XmlNode rightEyeNode = xmlDoc.CreateElement("right-eye");

            int countRightAOI = 0;

            foreach (FixationModel data in _rightFixationData)
            {

                XmlNode node = data.ToAOIXML(settings, this, "right", xmlDoc, includeSingleGazeData);

                if (node != null)
                {
                    rightEyeNode.AppendChild(node);
                    countRightAOI++;
                }
            }

            aoisNode.AppendChild(rightEyeNode);


            XmlAttribute rightAOICount = xmlDoc.CreateAttribute("count-of-aoi");

            rightAOICount.Value = countRightAOI.ToString();

            rightEyeNode.Attributes.Append(rightAOICount);

            webpageNode.AppendChild(aoisNode);


            return webpageNode;
        }

        public XmlNode GenerateSaccadesXML(XmlDocument xmlDoc, Boolean includeSingleGazeData)
        {
            XmlNode webpageNode = xmlDoc.CreateElement("webpage");

            XmlAttribute url = xmlDoc.CreateAttribute("url");

            url.Value = this.Url;

            webpageNode.Attributes.Append(url);

            XmlAttribute visited = xmlDoc.CreateAttribute("visited");

            visited.Value = this.VisitTimestamp;

            webpageNode.Attributes.Append(visited);


            // insert fixations

            XmlNode saccadesNode = xmlDoc.CreateElement("saccades");

            XmlNode leftEyeNode = xmlDoc.CreateElement("left-eye");

            XmlAttribute leftSaccadesCount = xmlDoc.CreateAttribute("count-of-saccades");

            leftSaccadesCount.Value = _leftSaccadesData.Count.ToString();

            leftEyeNode.Attributes.Append(leftSaccadesCount);

            foreach (SaccadeModel data in _leftSaccadesData)
            {
                leftEyeNode.AppendChild(data.ToXML(xmlDoc, includeSingleGazeData));
            }

            saccadesNode.AppendChild(leftEyeNode);

            XmlNode rightEyeNode = xmlDoc.CreateElement("right-eye");

            XmlAttribute rightSaccadesCount = xmlDoc.CreateAttribute("count-of-saccades");

            rightSaccadesCount.Value = _rightSaccadesnData.Count.ToString();

            rightEyeNode.Attributes.Append(rightSaccadesCount);

            foreach (SaccadeModel data in _rightSaccadesnData)
            {
                rightEyeNode.AppendChild(data.ToXML(xmlDoc, includeSingleGazeData));
            }

            saccadesNode.AppendChild(rightEyeNode);

            webpageNode.AppendChild(saccadesNode);


            return webpageNode;
        }

        #endregion


        #region FixationFunctions

        public void ExtractFixationsAndSaccades(Algorithm algorithm)
        {
            _leftFixationData = algorithm.ExtractFixation(_positionData, "left");
            _rightFixationData = algorithm.ExtractFixation(_positionData, "right");

            _leftSaccadesData = algorithm.ExtractSaccades(_positionData, "left");
            _rightSaccadesnData = algorithm.ExtractSaccades(_positionData, "right");
        }

        #endregion
    }
}
