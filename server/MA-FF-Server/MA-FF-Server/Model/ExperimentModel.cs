using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebAnalyzer.Model
{
    class ExperimentModel
    {

        private String _experimentName;

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
    }
}
