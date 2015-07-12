using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebAnalyzer.UI;
using WebAnalyzer.ApplicationSettings;
using WebAnalyzer.Util;
using WebAnalyzer.Events;

using WebAnalyzer.Models.Base;


namespace WebAnalyzer.Controller
{
    public class MainController
    {

        public void Start()
        {
            ShowMainUI();
        }

        private void ShowMainUI()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            HTMLUI mainUI = new HTMLUI();
            mainUI.Shown += new System.EventHandler(this.MainUIFinishedLoading);
            Application.Run(mainUI);
        }

        private void MainUIFinishedLoading(object sender, EventArgs e)
        {
            ShowExperimentWizard();
        }

       
        private void ShowExperimentWizard()
        {
            Logger.Log("Show experiment wizard?");
            ExperimentWizard experimentWizard = new ExperimentWizard();
            experimentWizard.CreateExperiment += On_CreateExperiment;
            experimentWizard.LoadExperiment += On_LoadExperiment;

            if (experimentWizard.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("user pressed ok");
            }
        }

        private void On_CreateExperiment(object source, CreateExperimentEvent e)
        {
            Logger.Log("On create experiment event");

            ExperimentModel experiment = ExperimentModel.CreateExperiment(e.Name);

            if (e.ImportData)
            {
                //import data
                if (e.ImportParticipants)
                    experiment.Particpants = LoadController.LoadParticipants(e.ImportExperimentPath);

                if (e.ImportSettings)
                    experiment.Settings = LoadController.LoadSettings(e.ImportExperimentPath);

            }

            ExportController.SaveExperiment(experiment);
        }

        private void On_LoadExperiment(object source, LoadExperimentEvent e)
        {
            Logger.Log("On load experiment event");

            ExperimentModel experiment = LoadController.LoadExperiment(e.Path);

            Logger.Log(experiment.ExperimentName);
        }
    }
}
