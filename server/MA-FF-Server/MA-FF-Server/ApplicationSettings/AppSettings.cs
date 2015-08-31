using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;

namespace WebAnalyzer.ApplicationSettings
{

    /// <summary>
    /// Class for handling the settings. Currently only really used on first startup of application. 
    /// </summary>
    class AppSettings
    {

        /// <summary>
        /// Resets all the environment variables. Currently thats only the data location.
        /// </summary>
        /// <remarks>
        /// Sets FirstStart to false, propably should do that here, since the name of the method doesn't show it.
        /// </remarks>
        public static void ResetEnvironmentVariables()
        {
            AppSettings.ResetDataLocation();

            Properties.Settings.Default.FirstStart = false;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Resets the data location by using the MyDocuments folder of the user as a base.
        /// </summary>
        public static void ResetDataLocation()
        {
            String documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            String defaultPath = documentsPath + "\\" + Properties.Settings.Default.Applicationname + "\\";

            bool exists = System.IO.Directory.Exists(defaultPath);

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(defaultPath);
            }

            AppSettings.SetDatalocation(defaultPath);
        }

        /// <summary>
        /// Sets the data location but checks first if the given path exists.
        /// </summary>
        public static Boolean SetDatalocation(String path)
        {
            bool exists = System.IO.Directory.Exists(path);

            if (!exists)
            {
                return false;
            }

            Properties.Settings.Default.Datalocation = path;
            Properties.Settings.Default.Save();

            Logger.Log("New Datalocation: " + path);

            return true;
        }
    }
}
