using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Util
{
    public static class Utilities
    {
        public static string GetAppLocation()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
