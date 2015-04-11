using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public String Url
        {
            get { return _url; }
            set { _url = value;}
        }

        public void addPositionData(PositionDataModel data)
        {
            _positionData.Add(data);
        }

    }
}
