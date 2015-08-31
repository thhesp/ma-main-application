using System.Windows.Forms;
using System;

using WebAnalyzer.Controller;
using WebAnalyzer.UI;
using WebAnalyzer.ApplicationSettings;
using WebAnalyzer.Util;

namespace WebAnalyzer
{


    /// <summary>
    /// The main class of the whole application. Used for initializing everything necessary.
    /// </summary>
    public class WebAnalyzer
    {

        /// <summary>
        /// The main method of the application.
        /// </summary>
        /// <remarks> 
        /// If an debugger is attached (by VisualStudio for example) 
        /// the Settings are resetted to the default values in this method.
        /// </remarks> 
        [STAThread]
        static void Main()
        {

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Properties.Settings.Default.Reset();
            }

            new WebAnalyzer();
        }


        /// <summary>
        /// Constructor for the WebAnalyzer Class.
        /// </summary>
        public WebAnalyzer()
        {
            CheckFirstStartup();
            ValidateSettings();
            InitializeMainController();
        }

        /// <summary>
        /// Checks if it's the first run of the application. 
        /// If it is the environment will set the environment variables to fit the current system.
        /// </summary>
        /// <seealso cref="AppSettings.ResetEnvironmentVariables()">
        /// Contains the code for setting the environment variables. </seealso>
        private void CheckFirstStartup()
        {
            if (Properties.Settings.Default.FirstStart)
            {
                Console.WriteLine("Reset environmental variables since its the first start on this system...");
                AppSettings.ResetEnvironmentVariables();
            }
        }

        /// <summary>
        /// Validates the settings, so that no problems should arise.
        /// </summary>
        /// <seealso cref="SettingsValidator.Validate()">
        /// Contains the code for validating the settings.</seealso> 

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

        /// <summary>
        /// Initalizes the main part of the application. The main controller.
        /// </summary>
        /// <seealso cref="MainController">
        /// It is the core of the application.</seealso> 

        private void InitializeMainController()
        {
            MainController main = new MainController();
            main.Start();
        }
    }
}
