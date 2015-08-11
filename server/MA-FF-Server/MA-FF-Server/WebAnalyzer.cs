﻿using System.Windows.Forms;

using WebAnalyzer.Experiment;
using WebAnalyzer.UI;
using WebAnalyzer.ApplicationSettings;
using WebAnalyzer.Util;
using WebAnalyzer.Controller;

namespace WebAnalyzer
{
    class WebAnalyzer
    {

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
                Logger.Log("Reset environmental variables since its the first start on this system...");
                AppSettings.ResetEnvironmentVariables();
            }
        }

        private void ValidateSettings()
        {
            bool validate = SettingsValidator.Validate();

            if (validate)
            {
                Logger.Log("Settings validation successful");
            }
            else
            {
                Logger.Log("Problems while validating...");
            }
            
        }

        private void InitializeMainController()
        {
            MainController main = new MainController();
            main.Start();
        }
    }
}
