using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using WebAnalyzer.Util;
using WebAnalyzer.Models.Base;
using System.IO;

using WebAnalyzer.Controller;
using WebAnalyzer.Events;

namespace WebAnalyzer.UI.InteractionObjects
{
    /// <summary>
    /// window for selecting or loading an experiment
    /// </summary>
    public class SelectExperimentControl : BaseInteractionObject
    {
        /// <summary>
        /// Eventhandler for selecting the experiment
        /// </summary>
        public event SelectExperimentEventHandler SelectExperiment;

        /// <summary>
        /// Reference to the select experiment form
        /// </summary>
        private SelectExperimentForm _form;

        /// <summary>
        /// List of all experiments
        /// </summary>
        private List<String> _experimentNames = new List<String>();

        /// <summary>
        /// List of paths to all experiments
        /// </summary>
        private List<String> _experimentPaths = new List<String>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">Reference to the select experiment form</param>
        public SelectExperimentControl(SelectExperimentForm form)
        {
            _form = form;
            createExperimentList();
        }

        /// <summary>
        /// Fills the list of experiment data
        /// </summary>
        private void createExperimentList()
        {
            String path = Properties.Settings.Default.Datalocation;

            String[] directories = Directory.GetDirectories(path);

            for (int i = 0; i < directories.Length; i++)
            {

                if (LoadController.ValidateExperimentFolder(directories[i]))
                {
                    _experimentPaths.Add(directories[i]);

                    _experimentNames.Add(directories[i].Remove(0, path.Length));
                }
            }
        }

        /// <summary>
        /// Used for selecting an experiment
        /// </summary>
        /// <param name="path">Path to the experiment</param>
        /// <remarks>Called from javascript</remarks>
        public void selectExperiment(String path)
        {
            Logger.Log("Select experiment: " + path);

            SelectExperiment(this, new SelectExperimentEvent(path));

            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.OK;
            });
        }

        /// <summary>
        /// Returns an array of experiment names
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String[] getExperimentNames()
        {
            return _experimentNames.ToArray<String>();
        }

        /// <summary>
        /// Returns an array of experiment paths
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String[] getExperimentPaths()
        {
            return _experimentPaths.ToArray<String>();
        }
    }
}
