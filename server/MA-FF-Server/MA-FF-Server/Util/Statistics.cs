using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Util
{
    /// <summary>
    /// Class for calculating some statistics. Currently mainly used when calculating the duration needed for messages to get processed.
    /// </summary>
    class Statistics
    {
        /// <summary>
        /// Calculates the mean of the given array of values.
        /// </summary>
        /// <param name="values">Array of long values to be used for calculation.</param>
        public static double CalculateMean(long[] values)
        {
            if(values != null && values.Length != 0)
                return values.Average();

            return 0;
        }

        /// <summary>
        /// Calculates the median of the given array of values.
        /// </summary>
        /// <param name="values">Array of long values to be used for calculation.</param>
        public static double CalculateMedian(long[] values)
        {
            if (values == null || values.Length == 0)
                return 0;

            long[] sortedValues = (long[])values.Clone();
            Array.Sort(sortedValues);

            int middle = sortedValues.Count() / 2;

            if (sortedValues.Count() % 2 != 0)
            {
                return sortedValues[middle];
            }
            else
            {
                return (sortedValues[middle] + sortedValues[middle - 1]) / 2;
            }

        }

        /// <summary>
        /// Calculates the standard deviation of the given array of values.
        /// </summary>
        /// <param name="values">Array of long values to be used for calculation.</param>
        /// <remarks>Currently does no calculation :x</remarks>
        public static long CalculateStandardDeviation(long[] values)
        {


            return 0;
        }
    }
}
