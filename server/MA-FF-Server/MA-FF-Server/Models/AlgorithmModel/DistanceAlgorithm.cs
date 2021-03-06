﻿using System;
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
    /// Algorithm which calculates the distance between the gazes and generates fixations based on these distances and a minimum duration
    /// </summary>
    public class DistanceAlgorithm : Algorithm
    {
        /// <summary>
        /// Minimum duration of a fixation
        /// </summary>
        private double _minimumDuration;
        
        /// <summary>
        /// Accecptable deviations between the coordinates
        /// </summary>
        private double _acceptableDeviations;

        /// <summary>
        /// Getter/Setter for the Minimum duration
        /// </summary>
        public Double MinimumDuration{
            get{ return _minimumDuration; }
            set {_minimumDuration = value; }
        }

        /// <summary>
        /// Getter/ Setter for the acceptable deviation
        /// </summary>
        public Double AccetableDeviations
        {
            get { return _acceptableDeviations; }
            set { _acceptableDeviations = value; }
        }

        /// <summary>
        /// Extracts all fixations from the given list of positions
        /// </summary>
        /// <param name="positions">List of all gazes for this testrun</param>
        /// <param name="eye">String identifier for the eye</param>
        /// <returns>List of Fixations</returns>
        public override List<FixationModel> ExtractFixation(List<GazeModel> positions, String eye)
        {
            List<FixationModel> fixations = new List<FixationModel>();

            if (positions.Count == 0)
            {
                Logger.Log("Not position data");
                return fixations;
            }

            long duration = 0;
            List<GazeModel> relatedGazes = new List<GazeModel>();

            // iniate with first gaze data element
            PositionDataModel posModel = positions[0].GetEyeData(eye);

            double startX = posModel.TrackingData.X - AccetableDeviations;
            double startY = posModel.TrackingData.Y - AccetableDeviations;

            double endX = posModel.TrackingData.X + AccetableDeviations;
            double endY = posModel.TrackingData.Y + AccetableDeviations;

            String startTimestamp = positions[0].DataRequestedTimestamp;

            relatedGazes.Add(positions[0]);

            //start loop with second element

            for (int pos = 1; pos < positions.Count; pos++)
            {
                // get element with data
                PositionDataModel currentPos = positions[pos].GetEyeData(eye);

                if ((startX <= currentPos.TrackingData.X && endX >= currentPos.TrackingData.X) &&
                    (startY <= currentPos.TrackingData.Y && endY >= currentPos.TrackingData.Y))
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
                    startX = currentPos.TrackingData.X - AccetableDeviations;
                    startY = currentPos.TrackingData.Y - AccetableDeviations;

                    endX = currentPos.TrackingData.X + AccetableDeviations;
                    endY = currentPos.TrackingData.Y + AccetableDeviations;

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

        /// <summary>
        /// Returns an xml representation of the algorithm
        /// </summary>
        /// <param name="xmlDoc">The XML Document which will contain the xml</param>
        /// <returns>XMLNode which contains the representation of the algorithm</returns>
        public override XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode algorithm = xmlDoc.CreateElement("distance");

            /* minimum duration */

            XmlAttribute minDuration = xmlDoc.CreateAttribute("minimum-duration");

            minDuration.Value = this.MinimumDuration.ToString();

            algorithm.Attributes.Append(minDuration);

            /* accepatable deviation */

            XmlAttribute acceptableDeviation = xmlDoc.CreateAttribute("acceptable-deviation");

            acceptableDeviation.Value = this.AccetableDeviations.ToString();

            algorithm.Attributes.Append(acceptableDeviation);

            return algorithm;
        }
    }
}
