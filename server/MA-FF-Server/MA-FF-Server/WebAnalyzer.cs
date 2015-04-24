using System.Windows.Forms;

using WebAnalyzer.Experiment;
using WebAnalyzer.UI;
using WebAnalyzer.ApplicationSettings;
using WebAnalyzer.Util;

namespace WebAnalyzer
{
    class WebAnalyzer
    {

        public WebAnalyzer()
        {
            checkFirstStartup();
            validateSettings();
            createExperiment();
            renderForm();
        }

        private void checkFirstStartup()
        {
            if (Properties.Settings.Default.FirstStart)
            {
                Logger.Log("Reset environmental variables since its the first start on this system...");
                AppSettings.ResetEnvironmentVariables();
            }
        }

        private void validateSettings()
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

        private void createExperiment()
        {
            ExperimentController.getInstance().CreateExperiment("eyetracking-test");
        }

        private void renderForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestForm());
        }
    }
}
