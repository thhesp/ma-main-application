﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Util;

namespace WebAnalyzer.Models.DataModel
{
    class WebpageModel
    {

        private String _url;
        private String _visitTimestamp;
        private List<PositionDataModel> _positionData = new List<PositionDataModel>();

        public WebpageModel(String url, String visitTimestamp)
        {
            _url = url;
            _visitTimestamp = visitTimestamp;
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

        public void addPositionData(PositionDataModel data)
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

            foreach (PositionDataModel data in _positionData)
            {
                webpageNode.AppendChild(data.ToXML(xmlDoc));
            }

            return webpageNode;
        }

        #endregion

        public PositionDataModel AddPositionData(int xPosition, int yPosition, String timestamp)
        {
            PositionDataModel posModel = new PositionDataModel(xPosition, yPosition, timestamp);

            _positionData.Add(posModel);

            return posModel;
        }

        public PositionDataModel AddPositionData(PositionDataModel posModel)
        {
            _positionData.Add(posModel);

            return posModel;
        }
    }
}