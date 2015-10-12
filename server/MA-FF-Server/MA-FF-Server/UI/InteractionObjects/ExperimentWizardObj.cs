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
    /// <summary>
    /// Interaction object for the experiment wizard
    /// </summary>
    public class ExperimentWizardObj : BaseInteractionObject
    {
        /// <summary>
        /// Enumeration to check if the select experiment window is for loading or importing
        /// </summary>
        public enum SELECT_ACTION { NONE = -1, FOR_LOADING = 0, FOR_IMPORTING = 1 };

        /// <summary>
        /// Eventhandler for creating an experiment
        /// </summary>
        public event CreateExperimentEventHandler CreateExperiment;

        /// <summary>
        /// Eventhandler for loading an experiment
        /// </summary>
        public event LoadExperimentEventHandler LoadExperiment;

        /// <summary>
        /// Reference to the experiment wizard window
        /// </summary>
        private ExperimentWizard _form;

        /// <summary>
        /// Path to the experiment from which data shall be imported
        /// </summary>
        /// <remarks>Only used when creating a new experiment</remarks>
        private String _importPath;

        /// <summary>
        /// Flag to check if the select participant window is for importing or loading
        /// </summary>
        private SELECT_ACTION _currentAction = SELECT_ACTION.NONE;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">Reference to the experiment wizard form</param>
        public ExperimentWizardObj(ExperimentWizard form)
        {
            _form = form;
        }

        /// <summary>
        /// Method for going back in the browser
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void back()
        {
            Browser.Back();
        }

        /// <summary>
        /// Method for changing to the create experiment screen
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void loadCreateExperimentScreen()
        {
            string page = string.Format("{0}UI/HTMLResources/html/popup/experiment_wizard/create_experiment.html", Utilities.GetAppLocation());
            Browser.Load(page);
        }

        /// <summary>
        /// Method for creating an experiment
        /// </summary>
        /// <param name="name">Name of the new experiment</param>
        /// <remarks>Called from javascript</remarks>
        public void createExperiment(String name)
        {
            Logger.Log("Create experiment with name: " + name);

            CreateExperiment(this, new CreateExperimentEvent(name));
        }

        /// <summary>
        /// Method for creating an experiment while importing data
        /// </summary>
        /// <param name="name">Name of the experiment</param>
        /// <param name="importSettings">Shall settings be imported</param>
        /// <param name="importParticipants">Shall participants be imported</param>
        /// <remarks>Called from javascript</remarks>
        public void createExperimentWithImport(String name, Boolean importSettings, Boolean importParticipants)
        {
            Logger.Log("Create experiment with name: " + name);

            Logger.Log("Import from: " + _importPath);

            CreateExperimentEvent eventData = new CreateExperimentEvent(name);

            eventData.SetImportData(_importPath, importParticipants, importSettings);

            CreateExperiment(this, eventData);
        }

        /// <summary>
        /// Method for opening the select experiment window for importing
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void selectExperimentToImportData()
        {
            _currentAction = SELECT_ACTION.FOR_IMPORTING;

            SelectExperimentDialog();
        }

        /// <summary>
        /// Method for opening the select experiment window for loading the experiment
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void selectExperimentToLoad()
        {
            _currentAction = SELECT_ACTION.FOR_LOADING;
            SelectExperimentDialog();
        }

        /// <summary>
        /// Opens the Select Experiment Dialog
        /// </summary>
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

        /// <summary>
        /// Callback for when an experiment was selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
