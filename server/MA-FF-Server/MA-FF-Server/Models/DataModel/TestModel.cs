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
    public class TestModel
    {
        private String _started;
        private String _stopped;

        private String _label;

        private String _protocol;

        private List<WebpageModel> _visitedPages = new List<WebpageModel>();

        private Dictionary<String, GazeModel> _unassignedPositions = new Dictionary<String, GazeModel>();

        private String _lastGazeActionTimestamp = "";

        private double _trackingInterval = 0;
        private int _messageSentInterval = 0;

        public String Filename
        {
            get
            {
                return Timestamp.GetDateTime(Started) + ".xml";
            }
        }


        public String FilenameWithoutExtension
        {
            get
            {
                return Timestamp.GetDateTime(Started);
            }
        }

        public String Started
        {
            get { return _started; }
            set { _started = value; }
        }

        public String Stopped
        {
            get { return _stopped; }
            set { _stopped = value; }
        }

        public String Label
        {
            get { return _label; }
            set { _label = value; }
        }

        public String Protocol
        {
            get { return _protocol; }
            set { _protocol = value; }
        }

        public double TrackingInterval
        {
            get { return _trackingInterval; }
            set { _trackingInterval = value; }
        }

        public int MessageSentInterval
        {
            get { return _messageSentInterval; }
            set { _messageSentInterval = value; }
        }
        
        public Boolean Exportable()
        {
            if(_visitedPages.Count > 0)
            {
                return true;
            }

            return false;
        }

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

        public XmlNode GenerateStatisticsXML(XmlDocument xmlDoc)
        {
            return GenerateStatisticsXML(xmlDoc, true);
        }

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

        public long[] ArrayOfDurationFromRequestTillSent()
        {
            long[] durations = new long[0];
            foreach (WebpageModel page in _visitedPages)
            {


                durations = durations.Concat(StatisticsAnalyser.ArrayOfDurationFromRequestTillSent(page.Gazes)).ToArray();
            }

            return durations;
        }

        public long[] ArrayOfDurationFromServerSentToReceived()
        {
            long[] durations = new long[0];
            foreach (WebpageModel page in _visitedPages)
            {
                durations = durations.Concat(StatisticsAnalyser.ArrayOfDurationFromServerSentToReceived(page.Gazes)).ToArray();
            }

            return durations;
        }


        public long[] ArrayOfDurationFromClientReceivedToClientSent()
        {
            long[] durations = new long[0];
            foreach (WebpageModel page in _visitedPages)
            {
                durations = durations.Concat(StatisticsAnalyser.ArrayOfDurationFromClientReceivedToClientSent(page.Gazes)).ToArray();
            }

            return durations;
        }

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

        private void RemoveEmptyPages()
        {
            _visitedPages.RemoveAll(page => page.Gazes.Count == 0);
        }

        public GazeModel GetGazeModel(String uniqueId)
        {
            if (_unassignedPositions.ContainsKey(uniqueId))
            {
                return _unassignedPositions[uniqueId];
            }

            Logger.Log("No GazeModel could be found: " + uniqueId);

            return null;
        }

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

        public void ExtractFixationsAndSaccades(Algorithm algorithm)
        {
            foreach (WebpageModel page in _visitedPages)
            {
                page.ExtractFixationsAndSaccades(algorithm);
            }
        }

        public void AddWebpage(WebpageModel pageModel)
        {
            lock (_visitedPages)
            {
                _visitedPages.Add(pageModel);
            }
        }
    }
}
