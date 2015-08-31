using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.ApplicationSettings
{
    /// <summary>
    /// Class for settings validation.
    /// </summary>
    class SettingsValidator
    {

        /// <summary>
        /// Validates all settings (for which validation methods are given).
        /// </summary>
        public static Boolean Validate()
        {
            return ValidateDatalocation();
        }


        /// <summary>
        /// Checks if the Datalocation exists.
        /// </summary>
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
