using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Models.AnalysisModel;
using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.AlgorithmModel
{
    public abstract class Algorithm
    {
        public enum ALGORITHM_TYPES { DISTANCE, IVIEW_EVENTS };

        public abstract List<FixationModel> ExtractFixation(List<GazeModel> positions, String eye);

        public abstract List<SaccadeModel> ExtractSaccades(List<GazeModel> positions, String eye);
    }
}
