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

        public enum SELECT_ACTION { NONE = -1, FOR_LOADING = 0, FOR_IMPORTING = 1 };

        public event CreateExperimentEventHandler CreateExperiment;
        public event LoadExperimentEventHandler LoadExperiment;

        private ExperimentWizard _form;

        private String _importPath;

        private SELECT_ACTION _currentAction = SELECT_ACTION.NONE;

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

        public void createExperimentWithImport(String name, Boolean importSettings, Boolean importParticipants)
        {
            Logger.Log("Create experiment with name: " + name);

            Logger.Log("Import from: " + _importPath);

            CreateExperimentEvent eventData = new CreateExperimentEvent(name);

            eventData.SetImportData(_importPath, importParticipants, importSettings);

            CreateExperiment(this, eventData);
        }

        public void selectExperimentToImportData()
        {
            _currentAction = SELECT_ACTION.FOR_IMPORTING;

            SelectExperimentDialog();
        }

        public void selectExperimentToLoad()
        {
            _currentAction = SELECT_ACTION.FOR_LOADING;
            SelectExperimentDialog();
        }

        private void SelectExperimentDialog()
        {
            _form.BeginInvoke((Action)delegate
            {
                SelectExperimentForm selectExperiment = new SelectExperimentForm();

                selectExperiment.SelectExperiment += On_SelectExperiment;

                if (selectExperiment.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //refresh aois
                }
            });
        }

        private void On_SelectExperiment(object sender, SelectExperimentEvent e)
        {
            if (_currentAction == SELECT_ACTION.FOR_IMPORTING)
            {
                Logger.Log(e.Path);
                if (e.Path != null)
                {
                    _importPath = e.Path;

                    String name = e.Path.Remove(0, Properties.Settings.Default.Datalocation.Length);

                    EvaluteJavaScript("updateImportExperiment('" + name + "');");
                }
            }
            else if (_currentAction == SELECT_ACTION.FOR_LOADING)
            {
                Logger.Log(e.Path);
                if (e.Path != null)
                {
                    LoadExperiment(this, new LoadExperimentEvent(e.Path));
                }
            }

        }
    }
}
