using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.Models.AnalysisModel;
using WebAnalyzer.Models.Base;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Util;

namespace WebAnalyzer.Models.AlgorithmModel
{
    /// <summary>
    /// Algorithm which uses the raw fixation events from the smi eyetracker
    /// </summary>
    public class IViewEventsAlgorithm : Algorithm
    {

        /// <summary>
        /// Reference to the rawdata which contain the fixation events
        /// </summary>
        private RawTrackingData _rawData;

        /// <summary>
        /// Getter/Setter for the rawdata
        /// </summary>
        public RawTrackingData RawData
        {
            get { return _rawData; }
            set { _rawData = value; }
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

        /// <summary>
        /// Returns an xml representation of the algorithm
        /// </summary>
        /// <param name="xmlDoc">The XML Document which will contain the xml</param>
        /// <returns>XMLNode which contains the representation of the algorithm</returns>
        public override XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode algorithm = xmlDoc.CreateElement("iview-events");

            return algorithm;
        }
    }
}
