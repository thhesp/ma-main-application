using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.UI.InteractionObjects;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// CreateExperimentEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">CreateExperimentEvent which is sent</param>
    public delegate void CreateExperimentEventHandler(ExperimentWizardObj sender, CreateExperimentEvent e);

    /// <summary>
    /// Used for creating a new experiment
    /// </summary>
    public class CreateExperimentEvent : EventArgs
    {

        /// <summary>
        /// Name of the experiment
        /// </summary>
        private String _name;

        /// <summary>
        /// Shall data be imported
        /// </summary>
        private Boolean _importData = false;

        /// <summary>
        /// Path to the experiment from which data shall be imported
        /// </summary>
        private String _importExperimentPath;

        /// <summary>
        /// Shall the participants be imported
        /// </summary>
        private Boolean _importParticipants = false;

        /// <summary>
        /// Shall the experiment settings be imported
        /// </summary>
        private Boolean _importSettings = false;

        /// <summary>
        /// EventConstructor
        /// </summary>
        /// <param name="name">Name of the experiment</param>
        public CreateExperimentEvent(String name)
        {
            _name = name;
        }

        /// <summary>
        /// Method for setting additional data
        /// </summary>
        /// <param name="importExperimentPath">Path to the experiment from which data shall be imported</param>
        /// <param name="importParticipants">Shall the participants be imported</param>
        /// <param name="importSettings">Shall the experiment settings be imported</param>
        public void SetImportData(String importExperimentPath, Boolean importParticipants, Boolean importSettings){
            if(importSettings || importParticipants){
                _importData = true;

                _importExperimentPath = importExperimentPath;
                _importParticipants = importParticipants;
                _importSettings = importSettings;
            }
        }

        /// <summary>
        /// Getter for the experiment name
        /// </summary>
        public String Name {
            get { return _name; }
        }

        /// <summary>
        /// Getter if the data shall be imported
        /// </summary>
        public Boolean ImportData
        {
            get { return _importData; }
        }

        /// <summary>
        /// Getter to the path of the experiment from which data shall be imported
        /// </summary>
        public String ImportExperimentPath
        {
            get { return _importExperimentPath; }
        }

        /// <summary>
        /// Getter if the participants shall be imported
        /// </summary>
        public Boolean ImportParticipants
        {
            get { return _importParticipants; }
        }

        /// <summary>
        /// Getter if the experiment settings shall be imported
        /// </summary>
        public Boolean ImportSettings
        {
            get { return _importSettings; }
        }
    }
}
