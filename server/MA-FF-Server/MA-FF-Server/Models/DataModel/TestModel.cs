using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Util;
using WebAnalyzer.Models.SettingsModel;
using WebAnalyzer.Models.AnalysisModel;

namespace WebAnalyzer.Models.DataModel
{
    public class TestModel
    {
        private String _started;

        private String _lastPage;
        private WebpageModel _lastPageModel;

        private List<WebpageModel> _visitedPages = new List<WebpageModel>();

        private Dictionary<String, GazeModel> _unassignedPositions;

        private String _lastGazeActionTimestamp = "";
        

        public TestModel()
        {
            _unassignedPositions = new Dictionary<String, GazeModel>();
        }

        public String Started
        {
            get { return _started; }
            set { _started = value; }
        }
        
        #region GetterSetterFunctions

        public void AddWebpage(String url)
        {
            this.AddWebpage(url, Timestamp.GetMillisecondsUnixTimestamp());
        }

        public void AddWebpage(String url, String timestamp)
        {
            WebpageModel page = new WebpageModel(url, timestamp);
            _visitedPages.Add(page);
        }


        #endregion

        public Boolean Exportable()
        {
            if(_visitedPages.Count > 0)
            {
                return true;
            }

            return false;
        }

        #region XMLFunctions

        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode experimentNode = xmlDoc.CreateElement("experiment");

            XmlAttribute started = xmlDoc.CreateAttribute("started");

            started.Value = this.Started;

            experimentNode.Attributes.Append(started);

            XmlAttribute visitedWebpagesCount = xmlDoc.CreateAttribute("count-of-visited-pages");

            visitedWebpagesCount.Value = this._visitedPages.Count.ToString();

            experimentNode.Attributes.Append(visitedWebpagesCount);

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
                }
            }

            foreach (XmlNode child in experimentNode.ChildNodes)
            {
                WebpageModel page = WebpageModel.LoadFromXML(child);

                if (page != null)
                {
                    test._visitedPages.Add(page);
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

        public XmlNode GenerateFixationXML(XmlDocument xmlDoc)
        {
            return GenerateFixationXML(xmlDoc, true);
        }

        public XmlNode GenerateFixationXML(XmlDocument xmlDoc, Boolean includeSingleGazeData)
        {
            XmlNode experimentNode = xmlDoc.CreateElement("experiment");


            XmlAttribute started = xmlDoc.CreateAttribute("started");

            started.Value = this.Started;

            experimentNode.Attributes.Append(started);

            XmlAttribute visitedWebpagesCount = xmlDoc.CreateAttribute("count-of-visited-pages");

            visitedWebpagesCount.Value = this._visitedPages.Count.ToString();

            experimentNode.Attributes.Append(visitedWebpagesCount);

            XmlNode webpagesNode = xmlDoc.CreateElement("webpages");

            foreach (WebpageModel page in _visitedPages)
            {
                webpagesNode.AppendChild(page.GenerateFixationXML(xmlDoc, includeSingleGazeData));
            }

            experimentNode.AppendChild(webpagesNode);

            return experimentNode;
        }

        public XmlNode GenerateAOIXML(ExperimentSettings settings, XmlDocument xmlDoc)
        {
            return GenerateAOIXML(settings, xmlDoc, true);
        }

        public XmlNode GenerateAOIXML(ExperimentSettings settings, XmlDocument xmlDoc, Boolean includeSingleGazeData)
        {
            XmlNode experimentNode = xmlDoc.CreateElement("experiment");

            XmlAttribute started = xmlDoc.CreateAttribute("started");

            started.Value = this.Started;

            experimentNode.Attributes.Append(started);

            XmlAttribute visitedWebpagesCount = xmlDoc.CreateAttribute("count-of-visited-pages");

            visitedWebpagesCount.Value = this._visitedPages.Count.ToString();

            experimentNode.Attributes.Append(visitedWebpagesCount);

            XmlNode webpagesNode = xmlDoc.CreateElement("webpages");

            foreach (WebpageModel page in _visitedPages)
            {
                webpagesNode.AppendChild(page.GenerateAOIXML(settings, xmlDoc, includeSingleGazeData));
            }

            experimentNode.AppendChild(webpagesNode);

            return experimentNode;
        }

        #endregion

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
                return true;
            }

            return false;
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
            String callbackTimestamp = Timestamp.GetMillisecondsUnixTimestamp();

            GazeModel gaze = new GazeModel(timestamp);

            gaze.DataRequestedTimestamp = callbackTimestamp;

            

            // left eye

            EyeTrackingData leftData = new EyeTrackingData(leftX, leftY, callbackTimestamp);

            PositionDataModel leftPos = new PositionDataModel();

            leftPos.EyeTrackingData = leftData;


            // right eye

            EyeTrackingData rightData = new EyeTrackingData(rightX, rightY, callbackTimestamp);

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
            WebpageModel pageModel = this.GetPageModel(url, gazeModel.Timestamp);

            _lastPage = url;
            _lastPageModel = pageModel;

            pageModel.AddGazeData(gazeModel);

            return gazeModel;
        }

        private WebpageModel GetPageModel(String url, String timestamp)
        {
            if (_lastPage != null && _lastPage == url && _lastPageModel != null)
            {
                return _lastPageModel;
            }

            WebpageModel pageModel = new WebpageModel(url, timestamp);

            _visitedPages.Add(pageModel);

            return pageModel;
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
    }
}
