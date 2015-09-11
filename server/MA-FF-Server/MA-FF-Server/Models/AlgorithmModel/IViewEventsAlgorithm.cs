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

        public override List<SaccadeModel> ExtractSaccades(List<GazeModel> positions, String eye)
        {
            List<SaccadeModel> saccades = new List<SaccadeModel>();

            List<FixationModel> fixations = ExtractFixation(positions, eye);

            List<GazeModel> relatedGazes = new List<GazeModel>();

            long duration = 0;
            String startTimestamp = positions[0].DataRequestedTimestamp;

            if (fixations.Count > 0)
            {
                int fixationCount = 0;
                List<GazeModel> fixationGazes = fixations[fixationCount].RelatedGazes;

                for (int pos = 0; pos < positions.Count; pos++)
                {
                    //fixations starts, saccades end
                    if (fixationGazes.Contains(positions[pos]))
                    {
                        //Logger.Log("Fixation found, saccade ends now.");

                        // create saccade if related gazes are there
                        if (relatedGazes.Count > 0)
                        {
                            duration = long.Parse(relatedGazes.Last().DataRequestedTimestamp) - long.Parse(startTimestamp);
                            SaccadeModel saccade = new SaccadeModel(startTimestamp, relatedGazes.Last().DataRequestedTimestamp, duration, eye);

                            saccade.RelatedGazes = relatedGazes;

                            saccades.Add(saccade);

                            //create new related gazes
                            relatedGazes = new List<GazeModel>();
                        }

                        //jump over all gazes of the current fixation

                        int newPos = positions.IndexOf(fixationGazes.Last()) + 1;

                        if (newPos >= positions.Count)
                        {
                            break;
                        }

                        //Logger.Log("Jumping from " + pos + " to " + newPos);

                        pos = newPos;

                        startTimestamp = positions[pos].DataRequestedTimestamp;

                        duration = 0;


                        //jump to next fixation
                        fixationCount++;
                        fixationGazes = fixations[fixationCount].RelatedGazes;
                    }
                    else
                    {
                        //belongs to saccade
                        relatedGazes.Add(positions[pos]);
                    }
                }

                if (relatedGazes.Count > 0)
                {
                    //Logger.Log("Saccade at the end found");
                    duration = long.Parse(relatedGazes.Last().DataRequestedTimestamp) - long.Parse(startTimestamp);
                    SaccadeModel saccade = new SaccadeModel(startTimestamp, relatedGazes.Last().DataRequestedTimestamp, duration, eye);

                    saccade.RelatedGazes = relatedGazes;

                    saccades.Add(saccade);
                }
            }

            

            return saccades;
        }
    }
}
