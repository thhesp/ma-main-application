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
        

        public ExperimentModel(String experimentName)
        {
            _experimentName = experimentName;   
        }
        
        #region GetterSetterFunctions

        public String ExperimentName
        {
            get { return _experimentName; }
            set { _experimentName = value; }
        }

        public void AddWebpage(String url)
        {
            WebpageModel page = new WebpageModel(url);
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


            foreach(WebpageModel page in _visitedPages)
            {
                experimentNode.AppendChild(page.ToXML(xmlDoc));
            }

            return experimentNode;
        }

        #endregion

        public PositionDataModel AddPositionData(String url, int xPosition, int yPosition, String timestamp)
        {
            WebpageModel pageModel = this.GetPageModel(url);

            _lastPage = url;
            _lastPageModel = pageModel;

            PositionDataModel posModel = pageModel.AddPositionData(xPosition, yPosition, timestamp);

            if(_firstExperimentPosition == null)
            {
                _firstExperimentPosition = posModel;
            }

            if (_previousPosition != null) 
            {
                _previousPosition.NextPosition = posModel;
            }

            _previousPosition = posModel;

            return posModel;
        }

        public PositionDataModel AddPositionData(String url, PositionDataModel posModel)
        {
            WebpageModel pageModel = this.GetPageModel(url);

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

        private WebpageModel GetPageModel(String url)
        {
            if (_lastPage != null && _lastPage == url && _lastPageModel != null)
            {
                return _lastPageModel;
            }

            WebpageModel pageModel = FindPageModel(url);

            if (pageModel != null)
            {
                return pageModel;
            }

            pageModel = new WebpageModel(url);

            _visitedPages.Add(pageModel);

            return pageModel;
        }

        private WebpageModel FindPageModel(String url)
        {
            foreach (WebpageModel pageModel in _visitedPages)
            {
                if (pageModel.Url == url)
                {
                    return pageModel;
                }
            }

            return null;
        }
    }
}
