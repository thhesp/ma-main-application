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

            List<RawTrackingEvent> rawFixations = RawData.GetFixationsForEye(eye);

            foreach (RawTrackingEvent fixationEvent in rawFixations)
            {
                //find all gazes which belong to this fixation
                FixationModel fixation = new FixationModel(fixationEvent.StartTime, fixationEvent.EndTime, long.Parse(fixationEvent.Duration), eye);

                long startTimestamp;
                long endTimestamp;

                if (fixationEvent.StartTime != "" && fixationEvent.EndTime != "")
                {
                    startTimestamp = long.Parse(fixationEvent.StartTime);
                    endTimestamp = long.Parse(fixationEvent.EndTime);
                }
                else if (fixationEvent.StartTime != "")
                {
                    startTimestamp = long.Parse(fixationEvent.StartTime);

                    endTimestamp = startTimestamp + long.Parse(fixationEvent.Duration);
                }
                else if (fixationEvent.EndTime != "")
                {
                    endTimestamp = long.Parse(fixationEvent.EndTime);


                    startTimestamp = long.Parse(fixationEvent.EndTime) - long.Parse(fixationEvent.Duration);
                }
                else
                {
                    Logger.Log("not enough data");
                    continue;
                }

                foreach (GazeModel pos in positions)
                {
                    long gazeTimestamp = long.Parse(pos.DataRequestedTimestamp);

                    if (gazeTimestamp >= startTimestamp && gazeTimestamp <= endTimestamp)
                    {
                        fixation.AddRelatedGaze(pos);
                    }

                }

                fixations.Add(fixation);
            }


            return fixations;
        }
    }
}
