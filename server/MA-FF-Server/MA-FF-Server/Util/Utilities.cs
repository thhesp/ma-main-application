using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Util
{
    /// <summary>
    /// Class for all utilies that can't be sorted into own classes or won't need own classes.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Returns the location of which the application was started.
        /// </summary>
        /// <remarks>Used for getting the html files for the UI.</remarks>
        public static string GetAppLocation()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
