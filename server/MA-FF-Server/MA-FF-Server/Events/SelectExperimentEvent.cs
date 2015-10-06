using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// SelectExperimentEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">SelectExperimentEvent which is sent</param>
    public delegate void SelectExperimentEventHandler(object sender, SelectExperimentEvent e);

    /// <summary>
    /// Used for selecting an experiment from the experiment wizard
    /// </summary>
    /// <remarks>Used to import data from existing event while creating a new one.</remarks>
    public class SelectExperimentEvent : EventArgs
    {
        /// <summary>
        /// Path to the main directory of the experiment
        /// </summary>
        private String _path;

        /// <summary>
        /// EventConstructor
        /// </summary>
        /// <param name="path">Path to the main directory of the experiment</param>
        public SelectExperimentEvent(String path)
        {
            _path = path;
        }

        /// <summary>
        /// Getter of the path to the main directory of the experiment
        /// </summary>
        public String Path
        {
            get { return _path; }
        }
    }
}
