using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Util
{
    class Timestamp
    {

        public static String GetUnixTimestamp(){
            DateTime now = DateTime.Now;

            return now.ToString("yyyyMMddHHmmssffff");
        }
    }
}
