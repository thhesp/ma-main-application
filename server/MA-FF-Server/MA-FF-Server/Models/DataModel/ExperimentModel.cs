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

        private String _lastTimestamp;

        private PositionDataModel _firstExperimentPosition;
        private PositionDataModel _previousPosition;

        private List<WebpageModel> _visitedPages = new List<WebpageModel>();

        private Dictionary<String, PositionDataModel> _unassignedPositions;
        

        public ExperimentModel(String experimentName)
        {
            _experimentName = experimentName;
            _unassignedPositions = new Dictionary<String,PositionDataModel>();
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

        #endregion

        public String GetBaseExperimentLocation()
        {
            return Properties.Settings.Default.Datalocation + _experimentName + "\\";
        }

        public PositionDataModel GetPosition(String uniqueId)
        {
            if (_unassignedPositions.ContainsKey(uniqueId))
            {
                return _unassignedPositions[uniqueId];
            }

            Logger.Log("No PositionModel could be found: " + uniqueId);

            return null;
        }

        public Boolean AssignPositionToWebpage(String uniqueId, String url)
        {
            if (!_unassignedPositions.ContainsKey(uniqueId))
            {
                return false;
            }

            this.AddPositionData(url, _unassignedPositions[uniqueId]);

            _unassignedPositions.Remove(uniqueId);

            return true;
        }

        public Boolean AssignPositionToWebpage(PositionDataModel posModel, String url)
        {
            this.AddPositionData(url, posModel);

            if (_unassignedPositions.ContainsKey(posModel.UniqueId))
            {
                _unassignedPositions.Remove(posModel.UniqueId);
            }

            return true;
        }

        public String PreparePositionData(double xPosition, double yPosition, String startTime, String endTime, String duration)
        {
            EyeTrackingData data = new EyeTrackingData(xPosition, yPosition);

            data.StartTime = startTime;
            data.EndTime = endTime;
            data.Duration = duration;

            PositionDataModel pos = new PositionDataModel();

            pos.EyeTrackingData = data;

            // add to unassigned position

            _unassignedPositions.Add(pos.UniqueId, pos);

            return pos.UniqueId;
        }

        private PositionDataModel AddPositionData(String url, PositionDataModel posModel)
        {
            WebpageModel pageModel = this.GetPageModel(url, posModel.ServerReceivedTimestamp);

            _lastPage = url;
            _lastPageModel = pageModel;

            pageModel.AddPositionData(posModel);

            if (_firstExperimentPosition == null)
            {
                _firstExperimentPosition = posModel;
            }

            if (_previousPosition != null)
            {
                _previousPosition.NextPosition = posModel;
            }

            _previousPosition = posModel;
            _lastTimestamp = posModel.ServerReceivedTimestamp;

            return posModel;
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
