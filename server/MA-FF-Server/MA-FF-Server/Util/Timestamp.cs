using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Util
{
    public class Timestamp
    {

        public static String GetUnixTimestamp(){
            DateTime now = DateTime.Now;

            Int32 unixTimestamp = (Int32)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            return unixTimestamp.ToString();
        }

        public static String GetMillisecondsUnixTimestamp()
        {
            DateTime now = DateTime.Now;

            long timestamp = (long)(now.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

            return timestamp.ToString();

        }

        public static String GetDateTime()
        {
            return DateTime.Now.ToString("HHmm-ddMMyyyy");
        }
    }
}
