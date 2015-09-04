﻿using System;
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
        public enum ALGORITHM_TYPES { DISTANCE };

        public abstract List<FixationModel> ExtractFixation(List<GazeModel> positions, String eye);
    }
}
