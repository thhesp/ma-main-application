using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebAnalyzer.Model
{
    class WebpageModel
    {

        private String _url;
        private List<PositionDataModel> _positionData = new List<PositionDataModel>();

        public WebpageModel(String url)
        {
            _url = url;
        }

        #region GetterSetterFunctions

        public String Url
        {
            get { return _url; }
            set { _url = value;}
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


            foreach (PositionDataModel data in _positionData)
            {
                webpageNode.AppendChild(data.ToXML(xmlDoc));
            }

            return webpageNode;
        }

        #endregion
    }
}
