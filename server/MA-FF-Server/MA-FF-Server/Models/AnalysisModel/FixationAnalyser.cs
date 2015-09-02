using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.AnalysisModel
{
    public class FixationAnalyser
    {

        public static List<FixationModel> ExtractFixations(List<GazeModel> positions, String eye, double minimumDuration, double acceptableDeviations)
        {
            List<FixationModel> fixations = new List<FixationModel>();

            long duration = 0;
            List<GazeModel> relatedGazes = new List<GazeModel>();


            // iniate with first gaze data element
            PositionDataModel posModel = positions[0].GetEyeData(eye);

            double startX = posModel.EyeTrackingData.X - acceptableDeviations;
            double startY = posModel.EyeTrackingData.Y - acceptableDeviations;

            double endX = posModel.EyeTrackingData.X + acceptableDeviations;
            double endY = posModel.EyeTrackingData.Y + acceptableDeviations;

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
                    duration = long.Parse(positions[pos].DataRequestedTimestamp) - long.Parse(startTimestamp);
                    //found related gaze
                    relatedGazes.Add(positions[pos]);
                }
                else
                {
                    //fixation (if there was one) ends
                    //@Todo: add constant/ setting for duration min time
                    if (duration >= minimumDuration)
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
                    startX = currentPos.EyeTrackingData.X - acceptableDeviations;
                    startY = currentPos.EyeTrackingData.Y - acceptableDeviations;

                    endX = currentPos.EyeTrackingData.X + acceptableDeviations;
                    endY = currentPos.EyeTrackingData.Y + acceptableDeviations;

                    startTimestamp = positions[pos].DataRequestedTimestamp;

                    duration = 0;

                    relatedGazes.Add(positions[pos]);
                }
            }

            //clear up a possible ending fixation

            if (duration >= minimumDuration)
            {
                //Logger.Log("Saving last fixation...");
                //create fixation
                FixationModel fixation = new FixationModel(startTimestamp, relatedGazes.Last().DataRequestedTimestamp, duration, eye);

                fixation.From(startX, startY);
                fixation.To(endX, endY);
                fixation.RelatedGazes = relatedGazes;

                fixations.Add(fixation);

            }
            else
            {
                //Logger.Log("Last fixation to short :(");
            }

            return fixations;
        }
    }
}
