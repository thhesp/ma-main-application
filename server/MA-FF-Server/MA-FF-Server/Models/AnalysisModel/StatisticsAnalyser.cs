using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.AnalysisModel
{
    /// <summary>
    /// Collection of methods used for calculating statistics
    /// </summary>
    public class StatisticsAnalyser
    {

        /// <summary>
        /// Calculates the durations between request and sent values from the given list of positions
        /// </summary>
        /// <param name="positions">List of gazes</param>
        /// <returns>array of durations</returns>
        public static long[] ArrayOfDurationFromRequestTillSent(List<GazeModel> positions)
        {
            long[] durations = new long[positions.Count];
            for (int i = 0; i < positions.Count; i++)
            {
                durations[i] = positions[i].DurationFromRequestTillSending();
            }

            return durations;
        }

        /// <summary>
        /// Calculates the durations between server sent and received values from the given list of positions
        /// </summary>
        /// <param name="positions">List of gazes</param>
        /// <returns>array of durations</returns>
        public static long[] ArrayOfDurationFromServerSentToReceived(List<GazeModel> positions)
        {
            long[] durations = new long[positions.Count];
            for (int i = 0; i < positions.Count; i++)
            {
                durations[i] = positions[i].DurationFromServerSentToReceived();
            }

            return durations;
        }


        /// <summary>
        /// Calculates the durations between client received and client sent values from the given list of positions
        /// </summary>
        /// <param name="positions">List of gazes</param>
        /// <returns>array of durations</returns>
        public static long[] ArrayOfDurationFromClientReceivedToClientSent(List<GazeModel> positions)
        {
            long[] durations = new long[positions.Count];
            for (int i = 0; i < positions.Count; i++)
            {
                durations[i] = positions[i].DurationFromClientReceivedToClientSent();
            }

            return durations;
        }
    }
}
