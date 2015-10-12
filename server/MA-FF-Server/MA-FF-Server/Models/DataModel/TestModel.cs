using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Util;
using WebAnalyzer.Models.SettingsModel;
using WebAnalyzer.Models.AnalysisModel;
using WebAnalyzer.Models.AlgorithmModel;
using WebAnalyzer.Events;
using WebAnalyzer.Models.Base;
using WebAnalyzer.Models.EventModel;

namespace WebAnalyzer.Models.DataModel
{
    /// <summary>
    /// Class which represents a testrun
    /// </summary>
    public class TestModel
    {
        /// <summary>
        /// Timestamp when the testrun was started
        /// </summary>
        private String _started;

        /// <summary>
        /// Timestamp when the testrun was stopped
        /// </summary>
        private String _stopped;

        /// <summary>
        /// Label of the testrun
        /// </summary>
        private String _label;

        /// <summary>
        /// Protocol of the testrun
        /// </summary>
        private String _protocol;

        /// <summary>
        /// List of visited Pages during the test
        /// </summary>
        private List<WebpageModel> _visitedPages = new List<WebpageModel>();

        /// <summary>
        /// Unassigned gaze positions
        /// </summary>
        private Dictionary<String, GazeModel> _unassignedPositions = new Dictionary<String, GazeModel>();

        /// <summary>
        /// Timestamp of last action relating to a gaze
        /// </summary>
        /// <remarks>
        /// Used for checking if saving is already possible or not
        /// </remarks>
        private String _lastGazeActionTimestamp = "";

        /// <summary>
        /// tracking interval of the tracking component
        /// </summary>
        private double _trackingInterval = 0;

        /// <summary>
        /// interval in which messages are sent
        /// </summary>
        private int _messageSentInterval = 0;

        /// <summary>
        /// Getter for the testrun filename
        /// </summary>
        public String Filename
        {
            get
            {
                return Timestamp.GetDateTime(Started) + ".xml";
            }
        }

        /// <summary>
        /// Getter for the testrun filename without fileextension
        /// </summary>
        public String FilenameWithoutExtension
        {
            get
            {
                return Timestamp.GetDateTime(Started);
            }
        }

        /// <summary>
        /// Getter/ Setter for the started timestamp
        /// </summary>
        public String Started
        {
            get { return _started; }
            set { _started = value; }
        }

        /// <summary>
        /// Getter/ Setter for the stopped timestamp
        /// </summary>
        public String Stopped
        {
            get { return _stopped; }
            set { _stopped = value; }
        }

        /// <summary>
        /// Getter/ Setter for the label
        /// </summary>
        public String Label
        {
            get { return _label; }
            set { _label = value; }
        }

        /// <summary>
        /// Getter/ Setter for the protocol
        /// </summary>
        public String Protocol
        {
            get { return _protocol; }
            set { _protocol = value; }
        }

        /// <summary>
        /// Getter/ Setter for the tracking interval
        /// </summary>
        public double TrackingInterval
        {
            get { return _trackingInterval; }
            set { _trackingInterval = value; }
        }

        /// <summary>
        /// Getter/ Setter for the message interval
        /// </summary>
        public int MessageSentInterval
        {
            get { return _messageSentInterval; }
            set { _messageSentInterval = value; }
        }
        
        /// <summary>
        /// Method which checks if data was colleted and if its reasonable to export the data
        /// </summary>
        /// <returns></returns>
        public Boolean Exportable()
        {
            if(_visitedPages.Count > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="participant">Participant to which the testrun belongs</param>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public XmlNode ToXML(ExperimentParticipant participant, XmlDocument xmlDoc)
        {
            XmlNode experimentNode = xmlDoc.CreateElement("experiment");

            XmlAttribute label = xmlDoc.CreateAttribute("label");

            label.Value = this.Label;

            experimentNode.Attributes.Append(label);

            XmlAttribute started = xmlDoc.CreateAttribute("started");

            started.Value = this.Started;

            experimentNode.Attributes.Append(started);

            XmlAttribute stopped = xmlDoc.CreateAttribute("stopped");

            stopped.Value = this.Stopped;

            experimentNode.Attributes.Append(stopped);

            XmlAttribute duration = xmlDoc.CreateAttribute("duration");

            TimeSpan calcDuration = DateTime.Parse(Stopped) - DateTime.Parse(Started);

            duration.Value = calcDuration.ToString();

            experimentNode.Attributes.Append(duration);

            XmlAttribute visitedWebpagesCount = xmlDoc.CreateAttribute("count-of-visited-pages");

            visitedWebpagesCount.Value = this._visitedPages.Count.ToString();

            experimentNode.Attributes.Append(visitedWebpagesCount);

            XmlAttribute messageSentIntervall = xmlDoc.CreateAttribute("ws-message-sent-interval");

            messageSentIntervall.Value = MessageSentInterval.ToString();

            experimentNode.Attributes.Append(messageSentIntervall);

            XmlAttribute trackingIntervall = xmlDoc.CreateAttribute("tracking-interval");

            trackingIntervall.Value = this.TrackingInterval.ToString();

            experimentNode.Attributes.Append(trackingIntervall);

            //add protocol
            XmlNode protocolNode = xmlDoc.CreateElement("protocol");

            protocolNode.InnerText = this.Protocol;

            experimentNode.AppendChild(protocolNode);

            // add participant data
            experimentNode.AppendChild(participant.ToXML(xmlDoc));

            // add experiment data
            XmlNode webpagesNode = xmlDoc.CreateElement("webpages");

            foreach(WebpageModel page in _visitedPages)
            {
                webpagesNode.AppendChild(page.ToXML(xmlDoc));
            }

            experimentNode.AppendChild(webpagesNode);

            return experimentNode;
        }

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="doc">Document which contains the data</param>
        /// <returns>The loaded object</returns>
        public static TestModel LoadFromXML(XmlDocument doc)
        {
            TestModel test = new TestModel();

            XmlNode experimentNode = doc.DocumentElement.SelectSingleNode("/experiment");

            if (experimentNode == null)
            {
                return null;
            }

            foreach (XmlAttribute attr in experimentNode.Attributes)
            {
                switch (attr.Name)
                {
                    case "started":
                        test.Started = attr.Value;
                        break;
                    case "stopped":
                        test.Stopped = attr.Value;
                        break;
                    case "label":
                        test.Label = attr.Value;
                        break;
                    case "ws-message-sent-interval":
                        test.MessageSentInterval = int.Parse(attr.Value);
                        break;
                    case "tracking-interval":
                        test.TrackingInterval = double.Parse(attr.Value);
                        break;
                }
            }

            foreach (XmlNode child in experimentNode.ChildNodes)
            {
                if (child.Name == "webpages")
                {
                    foreach (XmlNode webpage in child)
                    {
                        WebpageModel page = WebpageModel.LoadFromXML(webpage);

                        if (page != null)
                        {
                            test._visitedPages.Add(page);
                        }
                    }
                }
                else if (child.Name == "protocol")
                {
                    test.Protocol = child.InnerText;
                }
            }

            return test;
        }

        /// <summary>
        /// Generates a statistics xml for the testrun
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public XmlNode GenerateStatisticsXML(XmlDocument xmlDoc)
        {
            return GenerateStatisticsXML(xmlDoc, true);
        }

        /// <summary>
        /// Generates a statistics xml for the testrun
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <param name="includeSingleGazeData">Should the xml contain data about each gaze</param>
        /// <returns></returns>
        public XmlNode GenerateStatisticsXML(XmlDocument xmlDoc, Boolean includeSingleGazeData)
        {
            XmlNode statisticsNode = xmlDoc.CreateElement("statistics");

            XmlAttribute started = xmlDoc.CreateAttribute("started");

            started.Value = this.Started;

            statisticsNode.Attributes.Append(started);

            XmlAttribute stopped = xmlDoc.CreateAttribute("stopped");

            stopped.Value = this.Stopped;

            statisticsNode.Attributes.Append(stopped);

            XmlAttribute duration = xmlDoc.CreateAttribute("duration");

            TimeSpan calcDuration = DateTime.Parse(Stopped) - DateTime.Parse(Started);

            duration.Value = calcDuration.ToString();

            statisticsNode.Attributes.Append(duration);

            // create & insert statistics for whole experiment

            XmlAttribute visitedWebpagesCount = xmlDoc.CreateAttribute("count-of-visited-pages");

            visitedWebpagesCount.Value = this._visitedPages.Count.ToString();

            statisticsNode.Attributes.Append(visitedWebpagesCount);

            XmlAttribute nrOfGazes = xmlDoc.CreateAttribute("number-of-gazes");

            int sum = 0;

            foreach (WebpageModel page in _visitedPages)
            {
                sum += page.Gazes.Count;
            }

            nrOfGazes.Value = sum.ToString();

            statisticsNode.Attributes.Append(nrOfGazes);

            //???
            statisticsNode.AppendChild(CreateExperimentStatistics(xmlDoc));

            XmlNode webpagesNode = xmlDoc.CreateElement("webpages");

            foreach (WebpageModel page in _visitedPages)
            {
                webpagesNode.AppendChild(page.GenerateStatisticsXML(xmlDoc, includeSingleGazeData));
            }

            statisticsNode.AppendChild(webpagesNode);

            return statisticsNode;
        }

        /// <summary>
        /// Generates the statistics on a testrun level
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        private XmlNode CreateExperimentStatistics(XmlDocument xmlDoc) 
        { 
            // number of revisited pages?
            
            // sum of statistics?
            XmlNode statsNode = xmlDoc.CreateElement("global-stats");

            XmlNode requestTillSent = xmlDoc.CreateElement("request-till-sent");

            InsertArrayStatistics(xmlDoc, requestTillSent, ArrayOfDurationFromRequestTillSent());

            statsNode.AppendChild(requestTillSent);

            XmlNode serverSentToReceived = xmlDoc.CreateElement("server-sent-to-received");

            InsertArrayStatistics(xmlDoc, serverSentToReceived, ArrayOfDurationFromServerSentToReceived());

            statsNode.AppendChild(serverSentToReceived);

            XmlNode clientReceivedToSent = xmlDoc.CreateElement("client-received-to-sent");

            InsertArrayStatistics(xmlDoc, clientReceivedToSent, ArrayOfDurationFromClientReceivedToClientSent());

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
        /// Generates an array of all durations from request till sent for all gazes in all webpages
        /// </summary>
        /// <returns></returns>
        public long[] ArrayOfDurationFromRequestTillSent()
        {
            long[] durations = new long[0];
            foreach (WebpageModel page in _visitedPages)
            {


                durations = durations.Concat(StatisticsAnalyser.ArrayOfDurationFromRequestTillSent(page.Gazes)).ToArray();
            }

            return durations;
        }

        /// <summary>
        /// Generates an array of all durations from server sent to received for all gazes in all webpages
        /// </summary>
        /// <returns></returns>
        public long[] ArrayOfDurationFromServerSentToReceived()
        {
            long[] durations = new long[0];
            foreach (WebpageModel page in _visitedPages)
            {
                durations = durations.Concat(StatisticsAnalyser.ArrayOfDurationFromServerSentToReceived(page.Gazes)).ToArray();
            }

            return durations;
        }

        /// <summary>
        /// Generates an array of all durations from client received to sent for all gazes in all webpages
        /// </summary>
        /// <returns></returns>
        public long[] ArrayOfDurationFromClientReceivedToClientSent()
        {
            long[] durations = new long[0];
            foreach (WebpageModel page in _visitedPages)
            {
                durations = durations.Concat(StatisticsAnalyser.ArrayOfDurationFromClientReceivedToClientSent(page.Gazes)).ToArray();
            }

            return durations;
        }

        /// <summary>
        /// Generates fixation xml
        /// </summary>
        /// <param name="participant">Participant to which the data belongs</param>
        /// <param name="algorithm">Algorithm which was used for the fixation extraction</param>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <param name="includeSingleGazeData">Shall the xml contain data about each single gaze</param>
        /// <returns></returns>
        public XmlNode GenerateFixationXML(ExperimentParticipant participant, Algorithm algorithm, XmlDocument xmlDoc, Boolean includeSingleGazeData)
        {
            XmlNode experimentNode = xmlDoc.CreateElement("experiment");


            XmlAttribute started = xmlDoc.CreateAttribute("started");

            started.Value = this.Started;

            experimentNode.Attributes.Append(started);

            XmlAttribute stopped = xmlDoc.CreateAttribute("stopped");

            stopped.Value = this.Stopped;

            experimentNode.Attributes.Append(stopped);

            XmlAttribute duration = xmlDoc.CreateAttribute("duration");

            TimeSpan calcDuration = DateTime.Parse(Stopped) - DateTime.Parse(Started);

            duration.Value = calcDuration.ToString();

            experimentNode.Attributes.Append(duration);

            XmlAttribute visitedWebpagesCount = xmlDoc.CreateAttribute("count-of-visited-pages");

            visitedWebpagesCount.Value = this._visitedPages.Count.ToString();

            experimentNode.Attributes.Append(visitedWebpagesCount);

            //add participant data
            experimentNode.AppendChild(participant.ToXML(xmlDoc));

            //add algorithm data
            XmlNode algorithmNode = xmlDoc.CreateElement("algorithm");

            algorithmNode.AppendChild(algorithm.ToXML(xmlDoc));

            experimentNode.AppendChild(algorithmNode);

            //add experiment data
            XmlNode webpagesNode = xmlDoc.CreateElement("webpages");

            foreach (WebpageModel page in _visitedPages)
            {
                webpagesNode.AppendChild(page.GenerateFixationXML(xmlDoc, includeSingleGazeData));
            }

            experimentNode.AppendChild(webpagesNode);

            return experimentNode;
        }

        /// <summary>
        /// Generates aoi xml
        /// </summary>
        /// <param name="settings">Settings to be used for the generation</param>
        /// <param name="participant">Participant to which the data belongs</param>
        /// <param name="algorithm">Algorithm which was used for the fixation extraction</param>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <param name="includeSingleGazeData">Shall the xml contain data about each single gaze</param>
        /// <returns></returns>
        public XmlNode GenerateAOIXML(ExperimentSettings settings, ExperimentParticipant participant, Algorithm algorithm, XmlDocument xmlDoc, Boolean includeSingleGazeData)
        {
            XmlNode experimentNode = xmlDoc.CreateElement("experiment");

            XmlAttribute started = xmlDoc.CreateAttribute("started");

            started.Value = this.Started;

            experimentNode.Attributes.Append(started);

            XmlAttribute stopped = xmlDoc.CreateAttribute("stopped");

            stopped.Value = this.Stopped;

            experimentNode.Attributes.Append(stopped);

            XmlAttribute duration = xmlDoc.CreateAttribute("duration");

            TimeSpan calcDuration = DateTime.Parse(Stopped) - DateTime.Parse(Started);

            duration.Value = calcDuration.ToString();

            experimentNode.Attributes.Append(duration);

            XmlAttribute visitedWebpagesCount = xmlDoc.CreateAttribute("count-of-visited-pages");

            visitedWebpagesCount.Value = this._visitedPages.Count.ToString();

            experimentNode.Attributes.Append(visitedWebpagesCount);

            //add participant data
            experimentNode.AppendChild(participant.ToXML(xmlDoc));

            //add algorithm data
            XmlNode algorithmNode = xmlDoc.CreateElement("algorithm");

            algorithmNode.AppendChild(algorithm.ToXML(xmlDoc));

            experimentNode.AppendChild(algorithmNode);

            //add aoi rules
            experimentNode.AppendChild(settings.ToXML(xmlDoc));

            //add experiment data
            XmlNode webpagesNode = xmlDoc.CreateElement("webpages");

            foreach (WebpageModel page in _visitedPages)
            {
                webpagesNode.AppendChild(page.GenerateAOIXML(settings, xmlDoc, includeSingleGazeData));
            }

            experimentNode.AppendChild(webpagesNode);

            return experimentNode;
        }

        /// <summary>
        /// Generates saccades xml
        /// </summary>
        /// <param name="participant">Participant to which the data belongs</param>
        /// <param name="algorithm">Algorithm which was used for the fixation extraction</param>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <param name="includeSingleGazeData">Shall the xml contain data about each single gaze</param>
        /// <returns></returns>
        public XmlNode GenerateSaccadesXML(ExperimentParticipant participant, Algorithm algorithm, XmlDocument xmlDoc, Boolean includeSingleGazeData)
        {
            XmlNode experimentNode = xmlDoc.CreateElement("experiment");
            

            XmlAttribute started = xmlDoc.CreateAttribute("started");

            started.Value = this.Started;

            experimentNode.Attributes.Append(started);

            XmlAttribute stopped = xmlDoc.CreateAttribute("stopped");

            stopped.Value = this.Stopped;

            experimentNode.Attributes.Append(stopped);

            XmlAttribute duration = xmlDoc.CreateAttribute("duration");

            TimeSpan calcDuration = DateTime.Parse(Stopped) - DateTime.Parse(Started);

            duration.Value = calcDuration.ToString();

            experimentNode.Attributes.Append(duration);

            XmlAttribute visitedWebpagesCount = xmlDoc.CreateAttribute("count-of-visited-pages");

            visitedWebpagesCount.Value = this._visitedPages.Count.ToString();

            experimentNode.Attributes.Append(visitedWebpagesCount);

            //add participant data
            experimentNode.AppendChild(participant.ToXML(xmlDoc));

            //add algorithm data
            XmlNode algorithmNode = xmlDoc.CreateElement("algorithm");

            algorithmNode.AppendChild(algorithm.ToXML(xmlDoc));

            experimentNode.AppendChild(algorithmNode);

            //add experiment data
            XmlNode webpagesNode = xmlDoc.CreateElement("webpages");

            foreach (WebpageModel page in _visitedPages)
            {
                webpagesNode.AppendChild(page.GenerateSaccadesXML(xmlDoc, includeSingleGazeData));
            }

            experimentNode.AppendChild(webpagesNode);

            return experimentNode;
        }

        /// <summary>
        /// Checks if it is already possible to save.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Checks if the last gaze action is older than the wait timeout.
        /// </remarks>
        public Boolean CheckForSave(){
            if(_unassignedPositions.Count == 0){
                return true;
            }

            if (_lastGazeActionTimestamp == "")
            {
                return true;
            }

            //duration since last gaze was assigned/ disposed
            long duration = long.Parse(Util.Timestamp.GetMillisecondsUnixTimestamp()) - long.Parse(_lastGazeActionTimestamp);

            if (duration >= Properties.Settings.Default.TestrunDataWaitDuration)
            {
                RemoveEmptyPages();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes all webpages which don't contain data
        /// </summary>
        private void RemoveEmptyPages()
        {
            _visitedPages.RemoveAll(page => page.Gazes.Count == 0);
        }

        /// <summary>
        /// Returns the gaze model to the given uniqueid from the unassgined positions
        /// </summary>
        /// <param name="uniqueId">Uniqueid from the gaze model</param>
        /// <returns></returns>
        /// <remarks>
        /// Used to assign messages to gazes in the DataMessageHandler
        /// </remarks>
        public GazeModel GetGazeModel(String uniqueId)
        {
            if (_unassignedPositions.ContainsKey(uniqueId))
            {
                return _unassignedPositions[uniqueId];
            }

            Logger.Log("No GazeModel could be found: " + uniqueId);

            return null;
        }

        /// <summary>
        /// Adds the gaze with the given uniqueid to the webpage with the given url
        /// </summary>
        /// <param name="uniqueId">UniqueID of the gaze</param>
        /// <param name="url">URL of the webpage</param>
        /// <param name="connectionUID">ConnectionUID of the websocket connection</param>
        /// <returns></returns>
        /// <remarks>
        /// Removes the gaze from the unassigned positions
        /// </remarks>
        public Boolean AssignGazeToWebpage(String uniqueId, String url, String connectionUID)
        {
            if (!_unassignedPositions.ContainsKey(uniqueId))
            {
                return false;
            }

            this.AddGazeData(url, connectionUID, _unassignedPositions[uniqueId]);

            lock (_unassignedPositions)
            {
                _unassignedPositions.Remove(uniqueId);
            }
            

            _lastGazeActionTimestamp = Util.Timestamp.GetMillisecondsUnixTimestamp();

            return true;
        }

        /// <summary>
        /// Adds the gaze to the webpage with the given url
        /// </summary>
        /// <param name="gazeModel">The gaze to assign</param>
        /// <param name="url">URL of the webpage</param>
        /// <param name="connectionUID">ConnectionUID of the websocket connection</param>
        /// <returns></returns>
        /// <remarks>
        /// Removes the gaze from the unassigned positions
        /// </remarks>
        public Boolean AssignGazeToWebpage(GazeModel gazeModel, String url, String connectionUID)
        {
            if (!_unassignedPositions.ContainsKey(gazeModel.UniqueId))
            {
                return false;
            }

            this.AddGazeData(url, connectionUID, gazeModel);

            lock (_unassignedPositions)
            {
                _unassignedPositions.Remove(gazeModel.UniqueId);
            }

            _lastGazeActionTimestamp = Util.Timestamp.GetMillisecondsUnixTimestamp();

            return true;
        }

        /// <summary>
        /// Assigns the given event to a webpage
        /// </summary>
        /// <param name="eventModel">Event to add</param>
        /// <param name="url">URL of the webpage</param>
        /// <param name="connectionUID">ConnectionUID of the websocket connection</param>
        public void AssignEventToWebpage(BaseEventModel eventModel, String url, String connectionUID)
        {
            WebpageModel pageModel = this.GetPageModel(url, connectionUID, eventModel.ServerReceivedTimestamp);

            if (pageModel != null)
            {
                pageModel.Events.Add(eventModel);
            }
            else
            {
                Logger.Log("No pagemodel for event found: " + url);
            }

        }

        /// <summary>
        /// Removes the gaze with the given uniqueid from the unassigned positions
        /// </summary>
        /// <param name="uniqueId">Uniqueid of gaze to remove</param>
        /// <returns></returns>
        public Boolean DisposeOfGazeData(String uniqueId)
        {
            if (!_unassignedPositions.ContainsKey(uniqueId))
            {
                return false;
            }

            lock (_unassignedPositions)
            {
                _unassignedPositions.Remove(uniqueId);
            }

            _lastGazeActionTimestamp = Util.Timestamp.GetMillisecondsUnixTimestamp();

            return true;
        }

        /// <summary>
        /// Removes the given gaze from the unassigned positions
        /// </summary>
        /// <param name="gazeModel">Gaze to remove</param>
        /// <returns></returns>
        public Boolean DisposeOfGazeData(GazeModel gazeModel)
        {
            if (_unassignedPositions.ContainsKey(gazeModel.UniqueId))
            {

                lock (_unassignedPositions)
                {
                    _unassignedPositions.Remove(gazeModel.UniqueId);

                }

                _lastGazeActionTimestamp = Util.Timestamp.GetMillisecondsUnixTimestamp();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Prepares gaze data before requesting the data over the websocket
        /// </summary>
        /// <param name="leftEye">Data of the left eye</param>
        /// <param name="rightEye">Data of the right eye</param>
        /// <returns></returns>
        public String PrepareGazeData(BaseTrackingData leftEye, BaseTrackingData rightEye)
        {
            String requestedTimestamp = Timestamp.GetMillisecondsUnixTimestamp();

            GazeModel gaze = new GazeModel(leftEye.CallbackTimestamp);

            gaze.DataRequestedTimestamp = requestedTimestamp;

            

            // left eye

            PositionDataModel leftPos = new PositionDataModel();

            leftPos.TrackingData = leftEye;


            // right eye

            PositionDataModel rightPos = new PositionDataModel();

            rightPos.TrackingData = rightEye;

            // add to gaze model

            gaze.LeftEye = leftPos;

            gaze.RightEye = rightPos;

            // add to unassigned position

            lock (_unassignedPositions)
            {
                _unassignedPositions.Add(gaze.UniqueId, gaze);
            }

            return gaze.UniqueId;
        }

        /// <summary>
        /// Adds the gaze to the webpage with the given url
        /// </summary>
        /// <param name="gazeModel">Gaze to add</param>
        /// <param name="url">URL of the webpage</param>
        /// <param name="connectionUID">ConnectionUID of the websocket connection</param>
        /// <returns></returns>
        /// <remarks>
        /// Removes the gaze from the unassigned positions
        /// </remarks>
        private GazeModel AddGazeData(String url, String connectionUID, GazeModel gazeModel)
        {
            WebpageModel pageModel = this.GetPageModel(url, connectionUID, gazeModel.DataRequestedTimestamp);

            if (pageModel != null)
            {
                pageModel.AddGazeData(gazeModel);
            }
            else
            {
                Logger.Log("No page model for gaze found????");
            }

            return gazeModel;
        }

        /// <summary>
        /// Returns the pagemodel for the url
        /// </summary>
        /// <param name="url">URL of the webpage</param>
        /// <param name="connectionUID">Connection UID of the websocket connection which received the data</param>
        /// <param name="timestamp">Timestamp on which the data was requested</param>
        /// <returns></returns>
        private WebpageModel GetPageModel(String url, String connectionUID, String timestamp)
        {
            // reiterate from the back of the list, so that the newest page gets used

            lock (_visitedPages)
            {
                if (_visitedPages.Count == 1 && _visitedPages[0].Url == url && _visitedPages[0].ConnectionUID == connectionUID)
                {
                    return _visitedPages[0];
                }

                for (int i = _visitedPages.Count - 1; i > 0; i--)
                {
                    if (_visitedPages[i].Url == url && 
                        _visitedPages[i].ConnectionUID == connectionUID)
                    {
                        if (long.Parse(_visitedPages[i].VisitTimestamp) < long.Parse(timestamp) )
                        {
                            return _visitedPages[i];
                        }
                        else
                        {
                            Logger.Log(_visitedPages[i].VisitTimestamp + " timestamp not right " + timestamp);
                        }
                    }
                    else
                    {
                        if (_visitedPages[i].Url != url)
                        {
                            Logger.Log(_visitedPages[i].Url + " url not right: " + url);
                        }
                        else if (_visitedPages[i].ConnectionUID != connectionUID)
                        {
                            Logger.Log(_visitedPages[i].ConnectionUID + " connectionUID not right: " + connectionUID);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Event callback for the message sent event
        /// </summary>
        /// <param name="uniqueId">Uniqueid of the gaze</param>
        /// <param name="sentTimestamp">Messagesent timestamp</param>
        /// <remarks>
        /// Adds the message sent timestamp to the gaze
        /// </remarks>
        public void MessageSent(String uniqueId, String sentTimestamp)
        {
            if (uniqueId != null)
            {
                if (_unassignedPositions.ContainsKey(uniqueId))
                {
                    _unassignedPositions[uniqueId].ServerSentTimestamp = sentTimestamp;
                }
            }
        }

        /// <summary>
        /// Method for extracting fixations and saccades
        /// </summary>
        /// <param name="algorithm">Algorithm to use for the extraction</param>
        public void ExtractFixationsAndSaccades(Algorithm algorithm)
        {
            foreach (WebpageModel page in _visitedPages)
            {
                page.ExtractFixationsAndSaccades(algorithm);
            }
        }

        /// <summary>
        /// Adds the given webpage to the visited pages
        /// </summary>
        /// <param name="pageModel">Webpage to add</param>
        public void AddWebpage(WebpageModel pageModel)
        {
            lock (_visitedPages)
            {
                _visitedPages.Add(pageModel);
            }
        }
    }
}
