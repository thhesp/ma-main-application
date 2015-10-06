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

            if (positions.Count == 0)
            {
                Logger.Log("Not position data");
                return fixations;
            }

            List<RawTrackingEvent> rawFixations = RawData.GetFixationsForEye(eye);

            foreach (RawTrackingEvent fixationEvent in rawFixations)
            {
                //find all gazes which belong to this fixation
                FixationModel fixation = new FixationModel(fixationEvent.StartTime, fixationEvent.EndTime, long.Parse(fixationEvent.Duration), eye);

                fixation.From(fixationEvent.X, fixationEvent.Y);

                fixation.To(fixationEvent.X, fixationEvent.Y);

                long startTimestamp;
                long endTimestamp;

                if (fixationEvent.StartTime != "0" && fixationEvent.EndTime != "0")
                {
                    startTimestamp = long.Parse(fixationEvent.StartTime);
                    endTimestamp = long.Parse(fixationEvent.EndTime);
                }
                else if (fixationEvent.StartTime != "0")
                {
                    startTimestamp = long.Parse(fixationEvent.StartTime);

                    endTimestamp = startTimestamp + long.Parse(fixationEvent.Duration);
                }
                else if (fixationEvent.EndTime != "0")
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
                    //use the "timestamp" since it is the one saved from the tracking event
                    long gazeTimestamp = long.Parse(pos.Timestamp);

                    //Logger.Log("GazeTimestamp: " + gazeTimestamp + " should be between " + startTimestamp + " and " + endTimestamp);

                    if (gazeTimestamp >= startTimestamp && gazeTimestamp <= endTimestamp)
                    {
                        //Logger.Log("Related Gaze found.");

                        fixation.AddRelatedGaze(pos);
                    }

                }

                //only add fixations with related gazes (to remove data before and after browser action)
                if (fixation.RelatedGazes.Count > 0)
                {
                    fixations.Add(fixation);
                }
            }


            return fixations;
        }
    }
}
