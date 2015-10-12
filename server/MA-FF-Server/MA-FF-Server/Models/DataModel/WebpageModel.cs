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
using WebAnalyzer.Models.EventModel;

namespace WebAnalyzer.Models.DataModel
{
    /// <summary>
    /// Model for representing a visit of a webpage
    /// </summary>
    public class WebpageModel
    {
        /// <summary>
        /// Url of th page
        /// </summary>
        private String _url;

        /// <summary>
        /// width of the window on page visit
        /// </summary>
        private int _windowWidth = 0;

        /// <summary>
        /// height of the window on page visit
        /// </summary>
        private int _windowHeight = 0;

        /// <summary>
        /// UID of the websocket connection on which the data was received
        /// </summary>
        private String _connectionUID;

        /// <summary>
        /// Timestamp on which the webpage was visited
        /// </summary>
        private String _visitTimestamp;

        /// <summary>
        /// Data about all gazes
        /// </summary>
        private List<GazeModel> _positionData = new List<GazeModel>();

        /// <summary>
        /// Data about all webpage events
        /// </summary>
        private List<BaseEventModel> _eventData = new List<BaseEventModel>();

        /* Fixations */
        /// <summary>
        /// Fixations for the left eye
        /// </summary>
        private List<FixationModel> _leftFixationData = new List<FixationModel>();

        /// <summary>
        /// Fixations for the right eye
        /// </summary>
        private List<FixationModel> _rightFixationData = new List<FixationModel>();

        /* Saccades */
        /// <summary>
        /// Saccades for the left eye
        /// </summary>
        private List<SaccadeModel> _leftSaccadesData = new List<SaccadeModel>();

        /// <summary>
        /// Saccades for the right eye
        /// </summary>
        private List<SaccadeModel> _rightSaccadesnData = new List<SaccadeModel>();

        /// <summary>
        /// Constructor for the webpage
        /// </summary>
        /// <param name="url">URL of the webpage</param>
        /// <param name="connectionUID">UID of the websocket connection which received the data</param>
        /// <param name="visitTimestamp">Timestamp on which the webpage was visited</param>
        public WebpageModel(String url, String connectionUID, String visitTimestamp)
        {
            _url = url;
            _visitTimestamp = visitTimestamp;
            _connectionUID = connectionUID;
        }

        /// <summary>
        /// Constructor used for loading from xml
        /// </summary>
        public WebpageModel()
        {

        }

        /// <summary>
        /// Getter/ Setter for the Url of the webpage
        /// </summary>
        public String Url
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// Getter/ Setter for the UID of the websocket connection which receives the data
        /// </summary>
        public String ConnectionUID
        {
            get { return _connectionUID; }
        }

        /// <summary>
        /// Getter/ Setter for the Timestamp on which the webpage was visited
        /// </summary>
        public String VisitTimestamp
        {
            get { return _visitTimestamp; }
            set { _visitTimestamp = value; }
        }

        /// <summary>
        /// Getter/ Setter for the Width of the browser window
        /// </summary>
        public int WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; }
        }


        /// <summary>
        /// Getter/ Setter for the Height of the browser window
        /// </summary>
        public int WindowHeight
        {
            get { return _windowHeight; }
            set { _windowHeight = value; }
        }

        /// <summary>
        /// Getter/ Setter for the gazes of this webpage
        /// </summary>
        public List<GazeModel> Gazes
        {
            get { return _positionData; }
        }

        /// <summary>
        ///Getter/ Setter for the events of this webpage
        /// </summary>
        public List<BaseEventModel> Events
        {
            get { return _eventData; }
        }

        /// <summary>
        /// Add gaze to this webpage
        /// </summary>
        /// <param name="data">Gaze to add</param>
        public void AddGazeData(GazeModel data)
        {
            _positionData.Add(data);
        }

        /// <summary>
        /// Getter/ Setter for the fixations of the left eye
        /// </summary>
        public List<FixationModel> LeftFixationData
        {
            get { return _leftFixationData; }
            set { _leftFixationData = value; }
        }

        /// <summary>
        /// Getter/ Setter for the fixations of the right eye
        /// </summary>
        public List<FixationModel> RightFixationData
        {
            get { return _rightFixationData; }
            set { _rightFixationData = value; }
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode webpageNode = xmlDoc.CreateElement("webpage");

            XmlAttribute url = xmlDoc.CreateAttribute("url");

            url.Value = this.Url;

            webpageNode.Attributes.Append(url);

            XmlAttribute visited = xmlDoc.CreateAttribute("visited");

            visited.Value = this.VisitTimestamp;

            webpageNode.Attributes.Append(visited);

            XmlAttribute windowWidth = xmlDoc.CreateAttribute("window-width");

            windowWidth.Value = this.WindowWidth.ToString();

            webpageNode.Attributes.Append(windowWidth);

            XmlAttribute windowHeight = xmlDoc.CreateAttribute("window-height");

            windowHeight.Value = this.WindowHeight.ToString();

            webpageNode.Attributes.Append(windowHeight);

            XmlAttribute numberOfGazes = xmlDoc.CreateAttribute("number-of-gazes");

            numberOfGazes.Value = _positionData.Count.ToString();

            webpageNode.Attributes.Append(numberOfGazes);

            XmlNode gazesNode = xmlDoc.CreateElement("gazes");

            foreach (GazeModel data in _positionData)
            {
                gazesNode.AppendChild(data.ToXML(xmlDoc));
            }

            webpageNode.AppendChild(gazesNode);

            XmlNode eventsNode = xmlDoc.CreateElement("events");

            foreach (BaseEventModel data in _eventData)
            {
                eventsNode.AppendChild(data.ToXML(xmlDoc));
            }

            webpageNode.AppendChild(eventsNode);

            return webpageNode;
        }

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="webpageNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
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
                        case "window-width":
                            page.WindowWidth = int.Parse(attr.Value);
                            break;
                        case "window-height":
                            page.WindowHeight = int.Parse(attr.Value);
                            break;
                    }
                }

                foreach (XmlNode child in webpageNode.ChildNodes)
                {
                    if (child.Name == "gazes")
                    {
                        foreach (XmlNode gazeNode in child)
                        {
                            GazeModel gaze = GazeModel.LoadFromXML(gazeNode);

                            if (gaze != null)
                            {
                                page.AddGazeData(gaze);
                            }
                        }
                    }
                    else if (child.Name == "events")
                    {
                        foreach (XmlNode eventNode in child)
                        {
                            BaseEventModel eventModel = BaseEventModel.LoadFromXML(eventNode);

                            if (eventModel != null)
                            {
                                page.Events.Add(eventModel);
                            }
                        }
                    }

                }

                return page;
            }

            Logger.Log("Wrong node type given");

            return null;
        }

        /// <summary>
        /// Generates a statistics xml for the webpage
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <param name="includeSingleGazeData">Should the xml contain data about each gaze</param>
        /// <returns></returns>
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

        /// <summary>
        /// Generates the statistics on a webpage level
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
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

        /// <summary>
        /// Generates the statistics for the given array
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <param name="node">Node in which data shall be inserted</param>
        /// <param name="array">Array with data which shall be analysed</param>
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

        /// <summary>
        /// Generates fixation xml
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <param name="includeSingleGazeData">Shall the xml contain data about each single gaze</param>
        /// <returns></returns>
        public XmlNode GenerateFixationXML(XmlDocument xmlDoc, Boolean includeSingleGazeData)
        {
            XmlNode webpageNode = xmlDoc.CreateElement("webpage");

            XmlAttribute url = xmlDoc.CreateAttribute("url");

            url.Value = this.Url;

            webpageNode.Attributes.Append(url);

            XmlAttribute visited = xmlDoc.CreateAttribute("visited");

            visited.Value = this.VisitTimestamp;

            webpageNode.Attributes.Append(visited);

            XmlAttribute windowWidth = xmlDoc.CreateAttribute("window-width");

            windowWidth.Value = this.WindowWidth.ToString();

            webpageNode.Attributes.Append(windowWidth);

            XmlAttribute windowHeight = xmlDoc.CreateAttribute("window-height");

            windowHeight.Value = this.WindowHeight.ToString();

            webpageNode.Attributes.Append(windowHeight);


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

        /// <summary>
        /// Generates aoi xml
        /// </summary>
        /// <param name="settings">Settings to be used for the generation</param>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <param name="includeSingleGazeData">Shall the xml contain data about each single gaze</param>
        /// <returns></returns>
        public XmlNode GenerateAOIXML(ExperimentSettings settings, XmlDocument xmlDoc, Boolean includeSingleGazeData)
        {
            XmlNode webpageNode = xmlDoc.CreateElement("webpage");

            XmlAttribute url = xmlDoc.CreateAttribute("url");

            url.Value = this.Url;

            webpageNode.Attributes.Append(url);

            XmlAttribute visited = xmlDoc.CreateAttribute("visited");

            visited.Value = this.VisitTimestamp;

            webpageNode.Attributes.Append(visited);

            XmlAttribute windowWidth = xmlDoc.CreateAttribute("window-width");

            windowWidth.Value = this.WindowWidth.ToString();

            webpageNode.Attributes.Append(windowWidth);

            XmlAttribute windowHeight = xmlDoc.CreateAttribute("window-height");

            windowHeight.Value = this.WindowHeight.ToString();

            webpageNode.Attributes.Append(windowHeight);


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

        /// <summary>
        /// Generates saccades xml
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <param name="includeSingleGazeData">Shall the xml contain data about each single gaze</param>
        /// <returns></returns>
        public XmlNode GenerateSaccadesXML(XmlDocument xmlDoc, Boolean includeSingleGazeData)
        {
            XmlNode webpageNode = xmlDoc.CreateElement("webpage");

            XmlAttribute url = xmlDoc.CreateAttribute("url");

            url.Value = this.Url;

            webpageNode.Attributes.Append(url);

            XmlAttribute visited = xmlDoc.CreateAttribute("visited");

            visited.Value = this.VisitTimestamp;

            webpageNode.Attributes.Append(visited);

            XmlAttribute windowWidth = xmlDoc.CreateAttribute("window-width");

            windowWidth.Value = this.WindowWidth.ToString();

            webpageNode.Attributes.Append(windowWidth);

            XmlAttribute windowHeight = xmlDoc.CreateAttribute("window-height");

            windowHeight.Value = this.WindowHeight.ToString();

            webpageNode.Attributes.Append(windowHeight);


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

        /// <summary>
        /// Method for extracting fixations and saccades
        /// </summary>
        /// <param name="algorithm">The algorithm to use for the extraction</param>
        public void ExtractFixationsAndSaccades(Algorithm algorithm)
        {
            try
            {
                _leftFixationData = algorithm.ExtractFixation(_positionData, "left");
                _rightFixationData = algorithm.ExtractFixation(_positionData, "right");

                Logger.Log("Count of left eye fixations: " + _leftFixationData.Count);
                Logger.Log("Count of right eye fixations: " + _rightFixationData.Count);
            }
            catch (Exception e)
            {
                Logger.Log("Exception while extracting fixations: " + e.Message);
            }

            try
            {
                _leftSaccadesData = algorithm.ExtractSaccades(_positionData, "left");
                _rightSaccadesnData = algorithm.ExtractSaccades(_positionData, "right");

                Logger.Log("Count of left eye saccades: " + _leftSaccadesData.Count);
                Logger.Log("Count of right eye saccades: " + _rightSaccadesnData.Count);
            }
            catch (Exception e)
            {
                Logger.Log("Exception while extracting saccades: " + e.Message);
            }

        }
    }
}
