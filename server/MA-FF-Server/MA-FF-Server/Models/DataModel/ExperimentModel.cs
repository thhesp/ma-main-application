using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Util;

namespace WebAnalyzer.Models.DataModel
{
    class ExperimentModel
    {

        private String _experimentName;


        private String _lastPage;
        private WebpageModel _lastPageModel;

        private List<WebpageModel> _visitedPages = new List<WebpageModel>();

        private Dictionary<String, GazeModel> _unassignedPositions;
        

        public ExperimentModel(String experimentName)
        {
            _experimentName = experimentName;
            _unassignedPositions = new Dictionary<String, GazeModel>();
        }
        
        #region GetterSetterFunctions

        public String ExperimentName
        {
            get { return _experimentName; }
            set { _experimentName = value; }
        }

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

            XmlAttribute experimentName = xmlDoc.CreateAttribute("name");

            experimentName.Value = this.ExperimentName;

            experimentNode.Attributes.Append(experimentName);

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

        public XmlNode GenerateStatisticsXML(XmlDocument xmlDoc)
        {
            XmlNode statisticsNode = xmlDoc.CreateElement("statistics");

            XmlAttribute experimentName = xmlDoc.CreateAttribute("name");

            experimentName.Value = this.ExperimentName;

            statisticsNode.Attributes.Append(experimentName);

            // create & insert statistics for whole experiment

            XmlAttribute visitedWebpagesCount = xmlDoc.CreateAttribute("count-of-visited-pages");

            visitedWebpagesCount.Value = this._visitedPages.Count.ToString();

            statisticsNode.Attributes.Append(visitedWebpagesCount);

            //???
            statisticsNode.AppendChild(CreateExperimentStatistics(xmlDoc));

            XmlNode webpagesNode = xmlDoc.CreateElement("webpages");

            foreach (WebpageModel page in _visitedPages)
            {
                webpagesNode.AppendChild(page.GenerateStatisticsXML(xmlDoc));
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

            min.Value = Statistics.GetMin(array).ToString();

            node.Attributes.Append(min);


            XmlAttribute max = xmlDoc.CreateAttribute("max");

            max.Value = Statistics.GetMax(array).ToString();

            node.Attributes.Append(max);

        }

        public long[] ArrayOfDurationFromRequestTillSent()
        {
            long[] durations = new long[0];
            foreach (WebpageModel page in _visitedPages)
            {
                durations = durations.Concat(page.ArrayOfDurationFromRequestTillSent()).ToArray();
            }

            return durations;
        }

        public long[] ArrayOfDurationFromServerSentToReceived()
        {
            long[] durations = new long[0];
            foreach (WebpageModel page in _visitedPages)
            {
                durations = durations.Concat(page.ArrayOfDurationFromServerSentToReceived()).ToArray();
            }

            return durations;
        }


        public long[] ArrayOfDurationFromClientReceivedToClientSent()
        {
            long[] durations = new long[0];
            foreach (WebpageModel page in _visitedPages)
            {
                durations = durations.Concat(page.ArrayOfDurationFromClientReceivedToClientSent()).ToArray();
            }

            return durations;
        }

        #endregion

        public String GetBaseExperimentLocation()
        {
            return Properties.Settings.Default.Datalocation + _experimentName + "\\";
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

            _unassignedPositions.Remove(uniqueId);

            return true;
        }

        public Boolean AssignGazeToWebpage(GazeModel gazeModel, String url)
        {
            if (!_unassignedPositions.ContainsKey(gazeModel.UniqueId))
            {
                return false;
            }
            
            this.AddGazeData(url, gazeModel);

             _unassignedPositions.Remove(gazeModel.UniqueId);

            return true;
        }

        public Boolean DisposeOfGazeData(String uniqueId)
        {
            if (!_unassignedPositions.ContainsKey(uniqueId))
            {
                return false;
            }

            _unassignedPositions.Remove(uniqueId);

            return true;
        }

        public Boolean DisposeOfGazeData(GazeModel gazeModel)
        {
            if (_unassignedPositions.ContainsKey(gazeModel.UniqueId))
            {
                _unassignedPositions.Remove(gazeModel.UniqueId);
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
            GazeModel gaze = new GazeModel(timestamp);

            String receiveTimestamp = Timestamp.GetMillisecondsUnixTimestamp();

            // left eye

            EyeTrackingData leftData = new EyeTrackingData(leftX, leftY, receiveTimestamp);

            PositionDataModel leftPos = new PositionDataModel();

            leftPos.EyeTrackingData = leftData;


            // right eye

            EyeTrackingData rightData = new EyeTrackingData(rightX, rightY, receiveTimestamp);

            PositionDataModel rightPos = new PositionDataModel();

            rightPos.EyeTrackingData = rightData;

            // add to gaze model

            gaze.LeftEye = leftPos;

            gaze.RightEye = rightPos;

            // add to unassigned position

            _unassignedPositions.Add(gaze.UniqueId, gaze);

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
    }
}
