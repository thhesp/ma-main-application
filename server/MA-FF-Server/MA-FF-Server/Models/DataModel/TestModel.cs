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

namespace WebAnalyzer.Models.DataModel
{
    public class TestModel
    {
        private String _started;
        private String _stopped;

        private List<WebpageModel> _visitedPages = new List<WebpageModel>();

        private Dictionary<String, GazeModel> _unassignedPositions = new Dictionary<String, GazeModel>();

        private String _lastGazeActionTimestamp = "";

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
                }
            }

            foreach (XmlNode childs in experimentNode.ChildNodes)
            {
                if (childs.Name == "webpages")
                {
                    foreach (XmlNode webpage in childs)
                    {
                        WebpageModel page = WebpageModel.LoadFromXML(webpage);

                        if (page != null)
                        {
                            test._visitedPages.Add(page);
                        }
                    }
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

        public XmlNode GenerateFixationXML(ExperimentParticipant participant, XmlDocument xmlDoc, Boolean includeSingleGazeData)
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

            //add experiment data
            XmlNode webpagesNode = xmlDoc.CreateElement("webpages");

            foreach (WebpageModel page in _visitedPages)
            {
                webpagesNode.AppendChild(page.GenerateFixationXML(xmlDoc, includeSingleGazeData));
            }

            experimentNode.AppendChild(webpagesNode);

            return experimentNode;
        }

        public XmlNode GenerateAOIXML(ExperimentSettings settings, ExperimentParticipant participant, XmlDocument xmlDoc, Boolean includeSingleGazeData)
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

            //add experiment data
            XmlNode webpagesNode = xmlDoc.CreateElement("webpages");

            foreach (WebpageModel page in _visitedPages)
            {
                webpagesNode.AppendChild(page.GenerateAOIXML(settings, xmlDoc, includeSingleGazeData));
            }

            experimentNode.AppendChild(webpagesNode);

            return experimentNode;
        }

        public XmlNode GenerateSaccadesXML(ExperimentParticipant participant, XmlDocument xmlDoc, Boolean includeSingleGazeData)
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

        public Boolean AssignGazeToWebpage(String uniqueId, String url)
        {
            if (!_unassignedPositions.ContainsKey(uniqueId))
            {
                return false;
            }

            this.AddGazeData(url, _unassignedPositions[uniqueId]);

            lock (_unassignedPositions)
            {
                _unassignedPositions.Remove(uniqueId);
            }
            

            _lastGazeActionTimestamp = Util.Timestamp.GetMillisecondsUnixTimestamp();

            return true;
        }

        public Boolean AssignGazeToWebpage(GazeModel gazeModel, String url)
        {
            if (!_unassignedPositions.ContainsKey(gazeModel.UniqueId))
            {
                return false;
            }
            
            this.AddGazeData(url, gazeModel);

            lock (_unassignedPositions)
            {
                _unassignedPositions.Remove(gazeModel.UniqueId);
            }

            _lastGazeActionTimestamp = Util.Timestamp.GetMillisecondsUnixTimestamp();

            return true;
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

        public String PrepareGazeData(String timestamp, double x, double y)
        {
            return this.PrepareGazeData(timestamp, x, y, x, y);
        }

        public String PrepareGazeData(String timestamp, double leftX, double leftY, double rightX, double rightY)
        {
            String requestedTimestamp = Timestamp.GetMillisecondsUnixTimestamp();

            GazeModel gaze = new GazeModel(timestamp);

            gaze.DataRequestedTimestamp = requestedTimestamp;

            

            // left eye

            EyeTrackingData leftData = new EyeTrackingData(leftX, leftY, requestedTimestamp);

            PositionDataModel leftPos = new PositionDataModel();

            leftPos.EyeTrackingData = leftData;


            // right eye

            EyeTrackingData rightData = new EyeTrackingData(rightX, rightY, requestedTimestamp);

            PositionDataModel rightPos = new PositionDataModel();

            rightPos.EyeTrackingData = rightData;

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

        private GazeModel AddGazeData(String url, GazeModel gazeModel)
        {
            WebpageModel pageModel = this.GetPageModel(url, gazeModel.DataRequestedTimestamp);

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

        private WebpageModel GetPageModel(String url, String timestamp)
        {
            // reiterate from the back of the list, so that the newest page gets used

            lock (_visitedPages)
            {
                if (_visitedPages.Count == 1 && _visitedPages[0].Url == url)
                {
                    return _visitedPages[0];
                }

                for (int i = _visitedPages.Count - 1; i > 0; i--)
                {
                    if (_visitedPages[i].Url == url)
                    {
                        if (long.Parse(_visitedPages[i].VisitTimestamp) < long.Parse(timestamp) )
                        {
                            return _visitedPages[i];
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

        public void ExtractFixations(Algorithm algorithm)
        {
            foreach (WebpageModel page in _visitedPages)
            {
                page.ExtractFixations(algorithm);
            }
        }

        public void AddWebpage(String url, String timestamp)
        {
            WebpageModel pageModel = new WebpageModel(url, timestamp);

            lock (_visitedPages)
            {
                _visitedPages.Add(pageModel);
            }
        }
    }
}
