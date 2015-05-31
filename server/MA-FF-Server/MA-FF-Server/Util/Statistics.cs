using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Util
{
    class Statistics
    {

        public static double CalculateMean(long[] values)
        {
            return values.Average();
        }

        public static double CalculateMedian(long[] values)
        {
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

        public static long CalculateStandardDeviation(long[] values)
        {


            return 0;
        }

        public static long GetMin(long[] values)
        {
            return values.Min();
        }

        public static long GetMax(long[] values)
        {
            return values.Max();
        }


    }
}
