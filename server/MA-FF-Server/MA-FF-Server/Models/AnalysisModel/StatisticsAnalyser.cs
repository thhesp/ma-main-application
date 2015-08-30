using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.AnalysisModel
{
    public class StatisticsAnalyser
    {

        public static long[] ArrayOfDurationFromRequestTillSent(List<GazeModel> positions)
        {
            long[] durations = new long[positions.Count];
            for (int i = 0; i < positions.Count; i++)
            {
                durations[i] = positions[i].DurationFromRequestTillSending();
            }

            return durations;
        }

        public static long[] ArrayOfDurationFromServerSentToReceived(List<GazeModel> positions)
        {
            long[] durations = new long[positions.Count];
            for (int i = 0; i < positions.Count; i++)
            {
                durations[i] = positions[i].DurationFromServerSentToReceived();
            }

            return durations;
        }


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
