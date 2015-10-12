using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Models.AnalysisModel;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Util;

namespace WebAnalyzer.Models.AlgorithmModel
{
    /// <summary>
    /// Abstract base class for all algorithms
    /// </summary>
    public abstract class Algorithm
    {
        /// <summary>
        /// Enumeration of all implemented algorithms
        /// </summary>
        public enum ALGORITHM_TYPES { DISTANCE, IVIEW_EVENTS };

        /// <summary>
        /// Abstract base for the ExtractFixation Method
        /// </summary>
        /// <param name="positions">List of all gazes for this testrun</param>
        /// <param name="eye">String identifier for the eye</param>
        /// <returns>List of Fixations</returns>
        public abstract List<FixationModel> ExtractFixation(List<GazeModel> positions, String eye);

        /// <summary>
        /// Method for creating an xml representation of the algorithm
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns>XMLNode which contains the information about the algorithm</returns>
        public abstract XmlNode ToXML(XmlDocument xmlDoc);

        /// <summary>
        /// Method which extracts all saccades
        /// </summary>
        /// <param name="positions">List of all gazes for this testrun</param>
        /// <param name="eye">String identifier for the eye</param>
        /// <returns>List of Saccades</returns>
        public List<SaccadeModel> ExtractSaccades(List<GazeModel> positions, String eye)
        {
            List<SaccadeModel> saccades = new List<SaccadeModel>();

            if (positions.Count == 0)
            {
                Logger.Log("Not position data");
                return saccades;
            }

            List<FixationModel> fixations = ExtractFixation(positions, eye);

            List<GazeModel> relatedGazes = new List<GazeModel>();

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
                            saccades.AddRange(ExtractRealSaccades(relatedGazes, eye));

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
                    saccades.AddRange(ExtractRealSaccades(relatedGazes, eye));
                    //Logger.Log("Saccade at the end found");
                }
            }
            else
            {
                Logger.Log("No fixations, so everything is in one saccade?");

                saccades.AddRange(ExtractRealSaccades(positions, eye));
            }

            return saccades;
        }

        /// <summary>
        /// Intern Method which extracts the real saccades for a subset of all gazes.
        /// </summary>
        /// <param name="gazes">List of gazes between to fixations</param>
        /// <param name="eye">String identifier for the eye</param>
        /// <returns>List of Saccades</returns>
        /// <remarks>
        /// Uses the vector of the saccades to determine which gazes contain to the same saccade and which don't.
        /// </remarks>
        private List<SaccadeModel> ExtractRealSaccades(List<GazeModel> gazes, String eye)
        {
            List<SaccadeModel> saccades = new List<SaccadeModel>();

            if (gazes.Count == 0)
            {
                //do nothing
            }
            else if (gazes.Count == 1)
            {
                String startTimestamp = gazes[0].DataRequestedTimestamp;

                SaccadeModel saccade = new SaccadeModel(startTimestamp, startTimestamp, 0, eye);

                saccade.RelatedGazes = gazes;

                saccades.Add(saccade);
            }
            else if (gazes.Count == 2)
            {
                String startTimestamp = gazes[0].DataRequestedTimestamp;
                String endTimestamp = gazes[1].DataRequestedTimestamp;

                long duration = long.Parse(endTimestamp) - long.Parse(startTimestamp);

                SaccadeModel saccade = new SaccadeModel(startTimestamp, endTimestamp, duration, eye);

                saccade.RelatedGazes = gazes;

                saccades.Add(saccade);
            }
            else
            {
                List<GazeModel> relatedGazes = new List<GazeModel>();

                //add first two gazes
                relatedGazes.Add(gazes[0]);
                relatedGazes.Add(gazes[1]);
                double[] vectorStart = CalculateVector(relatedGazes[0], relatedGazes[1], eye);

                for (int i = 2; i < gazes.Count; i++)
                {
                    //check if gaze coordinates is in the same direction
                    double[] currentVector = CalculateVector(relatedGazes.First(), gazes[i], eye);

                    if (CompareVectors(vectorStart, currentVector))
                    {
                        //same direction
                        relatedGazes.Add(gazes[i]);
                    }
                    else
                    {
                        //other direction

                        //create saccade
                        String startTimestamp = relatedGazes.First().DataRequestedTimestamp;
                        String endTimestamp = relatedGazes.Last().DataRequestedTimestamp;

                        long duration = long.Parse(endTimestamp) - long.Parse(startTimestamp);

                        SaccadeModel saccade = new SaccadeModel(startTimestamp, endTimestamp, duration, eye);

                        saccade.RelatedGazes = relatedGazes;

                        saccades.Add(saccade);

                        //prepare for next saccade
                        relatedGazes = new List<GazeModel>();

                        //add this gaze
                        relatedGazes.Add(gazes[i]);

                        if ((i + 1) < gazes.Count)
                        {
                            //add next gaze
                            relatedGazes.Add(gazes[i + 1]);
                            vectorStart = CalculateVector(relatedGazes.First(), relatedGazes[1], eye);
                        }
                        else
                        {
                            String timestamp = gazes[i].DataRequestedTimestamp;

                            SaccadeModel lastSaccade = new SaccadeModel(timestamp, timestamp, 0, eye);

                            lastSaccade.RelatedGazes = relatedGazes;

                            saccades.Add(lastSaccade);
                        }
                    }

                }

                if (relatedGazes.Count > 0)
                {
                    //check for last gaze
                    String startTimestamp = relatedGazes.First().DataRequestedTimestamp;
                    String endTimestamp = relatedGazes.Last().DataRequestedTimestamp;

                    long duration = long.Parse(endTimestamp) - long.Parse(startTimestamp);

                    SaccadeModel saccade = new SaccadeModel(startTimestamp, endTimestamp, duration, eye);

                    saccade.RelatedGazes = relatedGazes;

                    saccades.Add(saccade);
                }
            }

            return saccades;
        }

        /// <summary>
        /// Calculates the vector between to gazes for the given eye
        /// </summary>
        /// <param name="gaze1">First gaze</param>
        /// <param name="gaze2">Second gaze</param>
        /// <param name="eye">String identifier for the eye</param>
        /// <returns>Vector for a two dimensional realm</returns>
        protected double[] CalculateVector(GazeModel gaze1, GazeModel gaze2, String eye)
        {
            double deltaX = gaze2.GetEyeData(eye).TrackingData.X - gaze1.GetEyeData(eye).TrackingData.X;
            double deltaY = gaze2.GetEyeData(eye).TrackingData.Y - gaze1.GetEyeData(eye).TrackingData.Y;

            return new double[] { deltaX, deltaY };
        }

        /// <summary>
        /// Compares two two dimensional vectors
        /// </summary>
        /// <param name="vector1">First vector</param>
        /// <param name="vector2">Second vector</param>
        /// <returns>If the vectors are similar or not</returns>
        protected Boolean CompareVectors(double[] vector1, double[] vector2)
        {
            //https://de.wikipedia.org/wiki/Kosinus-%C3%84hnlichkeit

            double top = (vector1[0] * vector2[0]) + (vector1[1] * vector2[1]);
            double bottom = (Math.Sqrt(Math.Pow(vector1[0], 2) + Math.Pow(vector1[1], 2)) * Math.Sqrt(Math.Pow(vector2[0], 2) + Math.Pow(vector2[1], 2)));

            double similarity = top / bottom;

            return similarity > Properties.Settings.Default.SaccadeAngleSimiliarity;
        }
    }
}
