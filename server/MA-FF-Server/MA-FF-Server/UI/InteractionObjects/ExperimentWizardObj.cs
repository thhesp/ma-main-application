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
using WebAnalyzer.Events;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class ExperimentWizardObj : BaseInteractionObject
    {

        public event CreateExperimentEventHandler CreateExperiment;
        public event LoadExperimentEventHandler LoadExperiment;

        public ExperimentWizard _form;

        public ExperimentWizardObj(ExperimentWizard form)
        {
            _form = form;
        }

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

            CreateExperiment(this, new CreateExperimentEvent(name));
        }

        public void createExperimentWithImport(String name, String importExp, Boolean importSettings, Boolean importParticipants)
        {
            Logger.Log("Create experiment with name: " + name);

            Logger.Log("Import from: " + importExp);

            CreateExperimentEvent eventData = new CreateExperimentEvent(name);

            eventData.SetImportData(importExp, importParticipants, importSettings);

            CreateExperiment(this, eventData);
        }

        public String selectExperimentToImportData()
        {
            String path = SelectExperimentDialog();

            return path;
        }

        public void selectExperimentToLoad()
        {
            String path = SelectExperimentDialog();
            Logger.Log(path);
            if (path != null)
            {
                LoadExperiment(this, new LoadExperimentEvent(path));
            }

        }

        private String SelectExperimentDialog()
        {

            String selectPath = "";
            SelectExperimentForm selectExperiment = new SelectExperimentForm();

            if (selectExperiment.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //refresh aois

                selectPath = selectExperiment.Path;
            }

            return selectPath;
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
                {
                    selectedPath = null;
                    return;
                }


                selectedPath = fbd.SelectedPath;
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            return selectedPath;
        }
    }
}
