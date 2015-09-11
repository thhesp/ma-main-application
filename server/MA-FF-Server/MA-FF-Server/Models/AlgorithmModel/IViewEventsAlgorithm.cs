using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Models.AnalysisModel;
using WebAnalyzer.Models.Base;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Util;

namespace WebAnalyzer.Models.AlgorithmModel
{
    public class IViewEventsAlgorithm : Algorithm
    {

        private RawTrackingData _rawData;


        public RawTrackingData RawData
        {
            get { return _rawData; }
            set { _rawData = value; }
        }


        public override List<FixationModel> ExtractFixation(List<GazeModel> positions, String eye)
        {
            List<FixationModel> fixations = new List<FixationModel>();




            return fixations;
        }
    }
}
