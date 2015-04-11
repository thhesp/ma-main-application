using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public String ExperimentName
        {
            get { return _experimentName; }
            set { _experimentName = value; }
        }

        public void addWebpage(String url)
        {
            WebpageModel page = new WebpageModel(url);
            _visitedPages.Add(page);
        }

    }
}
