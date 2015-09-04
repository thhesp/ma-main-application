using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Util
{
    /// <summary>
    /// Class for timestamp creation. So that timestamps follow the same format.
    /// </summary>
    public class Timestamp
    {

        /// <summary>
        /// Returns the unix timestamp for the current time.
        /// </summary>
        public static String GetUnixTimestamp(){
            DateTime now = DateTime.Now;

            Int32 unixTimestamp = (Int32)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            return unixTimestamp.ToString();
        }

        /// <summary>
        /// Returns an milliseconds timestamp of the current time.
        /// </summary>
        /// <remarks>Mainly used for the websocket communication because every milliseconds is important.</remarks>
        public static String GetMillisecondsUnixTimestamp()
        {
            DateTime now = DateTime.Now;

            long timestamp = (long)(now.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

            return timestamp.ToString();

        }

        /// <summary>
        /// Creates an date string for the current time.
        /// </summary>
        public static String GetDateTime()
        {
            return DateTime.Now.ToString("ddMMyyyy-HHmm");
        }

        /// <summary>
        /// Creates an date string for the given date.
        /// </summary>
        /// <remarks>Not really sure right now if this is needed or if the string could just be reused...</remarks>
        public static String GetDateTime(String date)
        {
            return DateTime.Parse(date).ToString("ddMMyyyy-HHmm");
        }


        /// <summary>
        /// Creates an date string for the the given testrun filename.
        /// </summary>
        /// <remarks>Not really sure right now if this is needed or if the string could just be reused...</remarks>
        public static String GetCreatedFromFilename(String filename)
        {
            return DateTime.ParseExact(filename, "ddMMyyyy-HHmm", CultureInfo.InvariantCulture).ToString();
        }
    }
}
