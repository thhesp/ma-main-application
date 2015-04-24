using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.ApplicationSettings
{
    class SettingsValidator
    {

        public static Boolean Validate()
        {
            return ValidateDatalocation();
        }

        private static Boolean ValidateDatalocation()
        {
            bool exists = System.IO.Directory.Exists(Properties.Settings.Default.Datalocation);

            if (!exists)
            {
                return false;
            }

            return true;
        }


    }
}
