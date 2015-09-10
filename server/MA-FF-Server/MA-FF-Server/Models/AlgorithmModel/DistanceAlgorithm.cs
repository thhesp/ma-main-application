using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Models.AnalysisModel;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Util;

namespace WebAnalyzer.Models.AlgorithmModel
{
    public class DistanceAlgorithm : Algorithm
    {

        private double _minimumDuration;
        
        private double _acceptableDeviations;

        public Double MinimumDuration{
            get{ return _minimumDuration; }
            set {_minimumDuration = value; }
        }

        public Double AccetableDeviations
        {
            get { return _acceptableDeviations; }
            set { _acceptableDeviations = value; }
        }

        public override List<FixationModel> ExtractFixation(List<GazeModel> positions, String eye)
        {
            List<FixationModel> fixations = new List<FixationModel>();

            long duration = 0;
            List<GazeModel> relatedGazes = new List<GazeModel>();


            // iniate with first gaze data element
            PositionDataModel posModel = positions[0].GetEyeData(eye);

            double startX = posModel.EyeTrackingData.X - AccetableDeviations;
            double startY = posModel.EyeTrackingData.Y - AccetableDeviations;

            double endX = posModel.EyeTrackingData.X + AccetableDeviations;
            double endY = posModel.EyeTrackingData.Y + AccetableDeviations;

            String startTimestamp = positions[0].DataRequestedTimestamp;

            relatedGazes.Add(positions[0]);

            //start loop with second element

            for (int pos = 1; pos < positions.Count; pos++)
            {
                // get element with data
                PositionDataModel currentPos = positions[pos].GetEyeData(eye);

                if ((startX <= currentPos.EyeTrackingData.X && endX >= currentPos.EyeTrackingData.X) &&
                    (startY <= currentPos.EyeTrackingData.Y && endY >= currentPos.EyeTrackingData.Y))
                {
                    //found related gaze
                    relatedGazes.Add(positions[pos]);
                }
                else
                {
                    duration = long.Parse(relatedGazes.Last().DataRequestedTimestamp) - long.Parse(startTimestamp);
                    //fixation (if there was one) ends
                    if (duration >= MinimumDuration)
                    {
                        //create fixation
                        FixationModel fixation = new FixationModel(startTimestamp, relatedGazes.Last().DataRequestedTimestamp, duration, eye);

                        fixation.From(startX, startY);
                        fixation.To(endX, endY);
                        fixation.RelatedGazes = relatedGazes;

                        fixations.Add(fixation);

                    }
                    else
                    {
                        //Logger.Log("Fixation to short :(");
                    }


                    //clear related gazes
                    relatedGazes = new List<GazeModel>();

                    // iniate with current gaze element
                    startX = currentPos.EyeTrackingData.X - AccetableDeviations;
                    startY = currentPos.EyeTrackingData.Y - AccetableDeviations;

                    endX = currentPos.EyeTrackingData.X + AccetableDeviations;
                    endY = currentPos.EyeTrackingData.Y + AccetableDeviations;

                    startTimestamp = positions[pos].DataRequestedTimestamp;

                    duration = 0;

                    relatedGazes.Add(positions[pos]);
                }
            }

            if (relatedGazes.Count > 0)
            {
                //clear up a possible ending fixation
                duration = long.Parse(relatedGazes.Last().DataRequestedTimestamp) - long.Parse(startTimestamp);

                if (duration >= MinimumDuration)
                {
                    //Logger.Log("Saving last fixation...");
                    //create fixation
                    FixationModel fixation = new FixationModel(startTimestamp, relatedGazes.Last().DataRequestedTimestamp, duration, eye);

                    fixation.From(startX, startY);
                    fixation.To(endX, endY);
                    fixation.RelatedGazes = relatedGazes;

                    fixations.Add(fixation);

                }
            }
            

            return fixations;
        }

        public override List<SaccadeModel> ExtractSaccades(List<GazeModel> positions, String eye)
        {
            List<SaccadeModel> saccades = new List<SaccadeModel>();

            List<FixationModel> fixations = ExtractFixation(positions, eye);

            List<GazeModel> relatedGazes = new List<GazeModel>();

            long duration = 0;
            String startTimestamp = positions[0].DataRequestedTimestamp;

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

            return saccades;
        }
    }
}
