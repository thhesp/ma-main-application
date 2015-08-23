using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.UI.InteractionObjects;

namespace WebAnalyzer.Events
{
    public delegate void CreateExperimentEventHandler(ExperimentWizardObj sender, CreateExperimentEvent e);

    public class CreateExperimentEvent : EventArgs
    {

        private String _name;
        private Boolean _importData = false;
        private String _importExperimentPath;
        private Boolean _importParticipants = false;
        private Boolean _importSettings = false;

        public CreateExperimentEvent(String name)
        {
            _name = name;
        }

        public void SetImportData(String importExperimentPath, Boolean importParticipants, Boolean importSettings){
            if(importSettings || importParticipants){
                _importData = true;

                _importExperimentPath = importExperimentPath;
                _importParticipants = importParticipants;
                _importSettings = importSettings;
            }
        }

        public String Name {
            get { return _name; }
        }

        public Boolean ImportData
        {
            get { return _importData; }
        }

        public String ImportExperimentPath
        {
            get { return _importExperimentPath; }
        }

        public Boolean ImportParticipants
        {
            get { return _importParticipants; }
        }

        public Boolean ImportSettings
        {
            get { return _importSettings; }
        }
    }
}
