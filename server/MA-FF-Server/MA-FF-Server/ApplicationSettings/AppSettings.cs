using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;

namespace WebAnalyzer.ApplicationSettings
{
    class AppSettings
    {

        public static void ResetEnvironmentVariables()
        {
            AppSettings.ResetDataLocation();

            Properties.Settings.Default.FirstStart = false;
            Properties.Settings.Default.Save();
        }

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
