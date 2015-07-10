using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using WebAnalyzer.Util;

using WebAnalyzer.Models.Base;
using WebAnalyzer.Controller;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class ExperimentWizardObj : BaseInteractionObject
    {


        public void back()
        {
            Browser.Back();
        }

        public void loadCreateExperimentScreen()
        {
            string page = string.Format("{0}UI/HTMLResources/html/popup/experiment_wizard/create_experiment.html", Utilities.GetAppLocation());
            Browser.Load(page);
        }

        public void createExperiment(String name)
        {
            Logger.Log("Create experiment with name: " + name);

            ExperimentModel exp = ExperimentModel.CreateExperiment(name);

            ExportController.SaveExperiment(exp);
        }

        public void createExperimentWithImport(String name, String importExp, Boolean importAOI, Boolean importParticipants)
        {
            Logger.Log("Create experiment with name: " + name);

            Logger.Log("Import from: " + importExp);
        }

        public String selectExperimentToImportData()
        {
            String path = SelectFolderDialog();

            return path;
        }

        public void selectExperimentToLoad()
        {
            String path = SelectFolderDialog();
            Logger.Log(path);

            ExperimentModel experiment = LoadController.LoadExperiment(path);

            Logger.Log(experiment.ExperimentName);
        }

        private String SelectFolderDialog()
        {
            // done in a new thread (kinda is a hack) because of the STAThread problem.
            //http://stackoverflow.com/questions/6860153/exception-when-using-folderbrowserdialog
            string selectedPath = "";
            var t = new Thread((ThreadStart)(() =>
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                Logger.Log("Default path: " + Properties.Settings.Default.Datalocation);
                fbd.RootFolder = System.Environment.SpecialFolder.MyDocuments;
                fbd.SelectedPath = Properties.Settings.Default.Datalocation;

                Logger.Log("SelectedPath: " + fbd.SelectedPath);

                fbd.ShowNewFolderButton = false;
                if (fbd.ShowDialog() == DialogResult.Cancel)
                    return;

                selectedPath = fbd.SelectedPath;
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            return selectedPath;
        }
    }
}
