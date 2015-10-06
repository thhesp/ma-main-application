using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.UI.InteractionObjects;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// LoadExperimentEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">LoadExperimentEvent which is sent</param>
    public delegate void LoadExperimentEventHandler(ExperimentWizardObj sender, LoadExperimentEvent e);

    /// <summary>
    /// Event which gets triggered when an experiment gets loaded from the experimentWizard
    /// </summary>
    public class LoadExperimentEvent : EventArgs
    {

        /// <summary>
        /// Path to the main directory of the experiment
        /// </summary>
        private String _path;

        /// <summary>
        /// EventConstructor
        /// </summary>
        /// <param name="path">Path to the main directory of the experiment</param>
        public LoadExperimentEvent(String path){
            _path = path;
        }

        /// <summary>
        /// Getter of the path to the main directory of the experiment
        /// </summary>
        public String Path
        {   
            get {return _path; }
        }
    }
}
