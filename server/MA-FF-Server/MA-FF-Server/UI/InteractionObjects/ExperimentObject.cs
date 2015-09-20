using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebAnalyzer.Controller;
using WebAnalyzer.Models.Base;
using WebAnalyzer.Util;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class ExperimentObject : BaseInteractionObject
    {

        private HTMLUI _form;

        public enum CONNECTION_STATUS { connected = 0, disconnected = 1, warning = 2 };

        private ExperimentModel _exp;

        private CONNECTION_STATUS _wsConnection = CONNECTION_STATUS.disconnected;

        private CONNECTION_STATUS _trackingConnection = CONNECTION_STATUS.disconnected;

        private int _connectionCount = 0;


        public ExperimentObject(HTMLUI form)
        {
            _form = form;
        }

        public ExperimentModel Experiment
        {
            get { return _exp; }
            set { _exp = value; }
        }

        public String getName()
        {
            if (_exp != null)
                return _exp.ExperimentName;

            return null;
        }

        public String[] participantArray()
        {
            if (_exp != null)
                return _exp.GetParticipantArray();

            return null;
        }

        public String[] participantUIDs()
        {
            if (_exp != null)
                return _exp.GetParticipantUIDs();

            return null;
        }

        public String[] domainSettingsArray()
        {
            if (_exp != null)
                return _exp.GetDomainSettingArray();

            return null;
        }

        public String[] domainSettingUIDs()
        {
            if (_exp != null)
                return _exp.GetDomainSettingUIDs();

            return null;
        }

        public String getWSConnectionStatus()
        {
            return _wsConnection.ToString();
        }


        public String getTrackingConnectionStatus()
        {
            return _trackingConnection.ToString();
        }

        public void SetWSConnectionStatus(CONNECTION_STATUS status)
        {
            _wsConnection = status;
        }

        public void SetTrackingConnectionStatus(CONNECTION_STATUS status)
        {
            _trackingConnection = status;
        }

        public void SetWSConnectionCount(int count)
        {
            _connectionCount = count;
        }

        public int getConnectionCount()
        {
            return _connectionCount;
        }

        public void RefreshData()
        {
            EvaluteJavaScript("initialize();");
        }

        public void importParticipants()
        {
            Logger.Log("Import Participants");

            String filePath = loadFileDialog();

            if (filePath != "")
            {

            }
        }

        public void importExperimentSettings()
        {
            Logger.Log("Import Experimentsettings");

            String filePath = loadFileDialog();

            if (filePath != "")
            {
                
            }
        }

        public void exportParticipants()
        {
            Logger.Log("Export Participants");

            String filePath = saveFileDialog();

            if (filePath != "")
            {
                ExportController.ExportExperimentParticipants(filePath, _exp.Participants);


            }
            
        }

        public void exportExperimentSettings()
        {
            Logger.Log("Export Experimentsettings");

            String filePath = saveFileDialog();

            if (filePath != "")
            {
                ExportController.ExportExperimentSettings(filePath, _exp.Settings);
            }
        }

        private String loadFileDialog()
        {
            String filePath = "";
            _form.Invoke((Action)delegate
            {
                Stream myStream = null;
                OpenFileDialog loadFileDialog = new OpenFileDialog();

                loadFileDialog.InitialDirectory = "c:\\";
                loadFileDialog.Filter = "XML Datei|*.xml";
                loadFileDialog.RestoreDirectory = true;

                if (loadFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if(loadFileDialog.FileName != ""){
                        Logger.Log("Selected Filename: " + loadFileDialog.FileName);

                        filePath = loadFileDialog.FileName;
                    }
                    
                }
            });

            return filePath;
        }

        private String saveFileDialog()
        {
            String filePath = "";
            _form.Invoke((Action)delegate
            {

                // Displays a SaveFileDialog so the user can save the Image
                // assigned to Button2.
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML Datei|*.xml";
                saveFileDialog.Title = "XML Exportieren";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // If the file name is not an empty string open it for saving.
                    if (saveFileDialog.FileName != "")
                    {
                        Logger.Log("Selected Filename: " + saveFileDialog.FileName);

                        filePath = saveFileDialog.FileName;
                    }
                }
            });

            return filePath;
        }
    }
}
