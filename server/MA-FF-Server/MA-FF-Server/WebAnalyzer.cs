using System.Windows.Forms;
using System;

using WebAnalyzer.Controller;
using WebAnalyzer.UI;
using WebAnalyzer.ApplicationSettings;
using WebAnalyzer.Util;

namespace WebAnalyzer
{
    public class WebAnalyzer
    {

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Properties.Settings.Default.Reset();
            }

            new WebAnalyzer();
        }


        public WebAnalyzer()
        {
            CheckFirstStartup();
            ValidateSettings();
            InitializeMainController();
        }

        private void CheckFirstStartup()
        {
            if (Properties.Settings.Default.FirstStart)
            {
                Console.WriteLine("Reset environmental variables since its the first start on this system...");
                AppSettings.ResetEnvironmentVariables();
            }
        }

        private void ValidateSettings()
        {
            bool validate = SettingsValidator.Validate();

            if (validate)
            {
                Console.WriteLine("Settings validation successful");
            }
            else
            {
                Console.WriteLine("Problems while validating...");
            }
            
        }

        private void InitializeMainController()
        {
            MainController main = new MainController();
            main.Start();
        }
    }
}
