using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebAnalyzer.Controller;
using WebAnalyzer.Events;
using WebAnalyzer.Models.Base;
using WebAnalyzer.Models.SettingsModel;
using WebAnalyzer.Util;

namespace WebAnalyzer.UI.InteractionObjects
{
    /// <summary>
    /// Interaction object for representing the currently loaded experiment
    /// </summary>
    public class ExperimentObject : BaseInteractionObject
    {

        /// <summary>
        /// Method used for triggering a save to xml
        /// </summary>
        public event TriggerSaveEventHandler TriggerSave;

        /// <summary>
        /// Reference to the main UI
        /// </summary>
        private HTMLUI _form;

        /// <summary>
        /// The different connection stati used for websocket connection and tracking component
        /// </summary>
        public enum CONNECTION_STATUS { connected = 0, disconnected = 1, warning = 2 };


        /// <summary>
        /// Reference to the actual loaded experiment
        /// </summary>
        private ExperimentModel _exp;

        /// <summary>
        /// Current status of the websocket connection
        /// </summary>
        private CONNECTION_STATUS _wsConnection = CONNECTION_STATUS.disconnected;

        /// <summary>
        /// Current status of the tracking component
        /// </summary>
        private CONNECTION_STATUS _trackingConnection = CONNECTION_STATUS.disconnected;

        /// <summary>
        /// Current connection count of the websocket server
        /// </summary>
        private int _connectionCount = 0;

        /// <summary>
        /// timestamp for the last time the description was updated
        /// </summary>
        private String _lastDescriptionUpdateTimestamp;

        /// <summary>
        /// timer which checks if the description needs to be saved again
        /// </summary>
        private Boolean _saveTimer = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">Reference to the main ui window</param>
        public ExperimentObject(HTMLUI form)
        {
            _form = form;
        }

        /// <summary>
        /// Getter / Setter for the currently loaded experiment
        /// </summary>
        public ExperimentModel Experiment
        {
            get { return _exp; }
            set { _exp = value; }
        }

        /// <summary>
        /// Returns the name of the current experiment
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String getName()
        {
            if (_exp != null)
                return _exp.ExperimentName;

            return null;
        }

        /// <summary>
        /// Returns the description of the current experiment
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String getDescription()
        {
            if (_exp != null)
                return _exp.Description;

            return null;
        }

        /// <summary>
        /// Updates the description of the experiment
        /// </summary>
        /// <param name="description">The new description</param>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public void updateDescription(String description)
        {
            _exp.Description = description;
            _lastDescriptionUpdateTimestamp = Timestamp.GetMillisecondsUnixTimestamp();

            checkSaveTimer();
        }

        /// <summary>
        /// Checks the save timer
        /// </summary>
        private void checkSaveTimer()
        {
            if (!_saveTimer)
            {
                _saveTimer = true;
                createSaveTimer();
            }
        }

        /// <summary>
        /// Creates the save timer
        /// </summary>
        private void createSaveTimer()
        {
            int interval = Properties.Settings.Default.ExperimentDescriptionSaveTimeout;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(interval);
                String currentTimestamp = Timestamp.GetMillisecondsUnixTimestamp();

                Logger.Log("checking if saving is necessary");
                // check if _lastDescription update is newer than it should
                if (long.Parse(_lastDescriptionUpdateTimestamp) - long.Parse(currentTimestamp) < interval)
                {
                    Logger.Log("saving the experiment file");
                    TriggerSave(this, new TriggerSaveEvent(TriggerSaveEvent.SAVE_TYPES.ALL));
                    _saveTimer = false;
                }
                else
                {
                    createSaveTimer();
                }
            });
        }

        /// <summary>
        /// Returns an array of participant identifiers
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Called from javascript
        /// </remarks>
        public String[] participantArray()
        {
            if (_exp != null)
                return _exp.GetParticipantArray();

            return null;
        }

        /// <summary>
        /// Returns an array of participant uids
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Called from javascript
        /// </remarks>
        public String[] participantUIDs()
        {
            if (_exp != null)
                return _exp.GetParticipantUIDs();

            return null;
        }

        /// <summary>
        /// Returns an array of domain setting identifiers
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Called from javascript
        /// </remarks>
        public String[] domainSettingsArray()
        {
            if (_exp != null)
                return _exp.GetDomainSettingArray();

            return null;
        }

        /// <summary>
        /// Returns an array of domain setting uids
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Called from javascript
        /// </remarks>
        public String[] domainSettingUIDs()
        {
            if (_exp != null)
                return _exp.GetDomainSettingUIDs();

            return null;
        }

        /// <summary>
        /// Returns the ws connection status as a string
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String getWSConnectionStatus()
        {
            return _wsConnection.ToString();
        }


        /// <summary>
        /// Returns the tracking component status as a string
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String getTrackingConnectionStatus()
        {
            return _trackingConnection.ToString();
        }

        /// <summary>
        /// Updates the status of the websocket connection
        /// </summary>
        /// <param name="status">The new status</param>
        public void SetWSConnectionStatus(CONNECTION_STATUS status)
        {
            _wsConnection = status;
        }

        /// <summary>
        /// Updates the status of the tracking component
        /// </summary>
        /// <param name="status">The new status</param>
        public void SetTrackingConnectionStatus(CONNECTION_STATUS status)
        {
            _trackingConnection = status;
        }

        /// <summary>
        /// Sets the websocket connection count
        /// </summary>
        /// <param name="count">The new count</param>
        public void SetWSConnectionCount(int count)
        {
            _connectionCount = count;
        }

        /// <summary>
        /// Returns the websocket connection count
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public int getConnectionCount()
        {
            return _connectionCount;
        }

        /// <summary>
        /// Refreshes the data in the browser
        /// </summary>
        public void RefreshData()
        {
            EvaluteJavaScript("initialize();");
        }

        /// <summary>
        /// Called for importing a participant xml
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void importParticipants()
        {
            Logger.Log("Import Participants");

            if (DisplayYesNoQuestion("Beim Import werden alle bestehenden Daten überschrieben. Trotzdem fortfahren?", "Teilnehmer importieren"))
            {

                String filePath = loadFileDialog();

                if (filePath != "")
                {
                    try
                    {
                        List<ExperimentParticipant> participants = LoadController.ImportParticipants(filePath);

                        if (participants != null)
                        {
                            _exp.Participants = participants;

                            _form.ReloadPage();

                            TriggerSave(this, new TriggerSaveEvent(TriggerSaveEvent.SAVE_TYPES.PARTICIPANTS));

                            DisplaySuccess("Die Experimentteilnehmer wurden erfolgreich importiert.", "Import erfolgreich");
                        }
                        else
                        {
                            DisplayError("Das XML enthielt fehlerhafte Daten, darum war ein Import nicht möglich.");
                        }

                    }
                    catch (Exception e)
                    {
                        Logger.Log("Error while importing participants: " + e.Message);

                        DisplayError("Es ist ein Fehler beim Import der Experimentteilnehmer aufgetreten.");
                    }

                }
            }
        }

        /// <summary>
        /// Called for importing a experiment settings xml
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void importExperimentSettings()
        {
            Logger.Log("Import Experimentsettings");

            if (DisplayYesNoQuestion("Beim Import werden alle bestehenden Daten überschrieben. Trotzdem fortfahren?", "Einstellungen importieren"))
            {

                String filePath = loadFileDialog();

                if (filePath != "")
                {
                    try
                    {
                        ExperimentSettings settings = LoadController.ImportSettings(filePath);

                        if (settings != null)
                        {
                            _exp.Settings = settings;

                            _form.ReloadPage();

                            TriggerSave(this, new TriggerSaveEvent(TriggerSaveEvent.SAVE_TYPES.SETTINGS));

                            DisplaySuccess("Die Experimenteinstellungen wurden erfolgreich importiert.", "Import erfolgreich");
                        }
                        else
                        {
                            DisplayError("Das XML enthielt fehlerhafte Daten, darum war ein Import nicht möglich.");
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Log("Error while importing settings: " + e.Message);

                        DisplayError("Es ist ein Fehler beim Import der Experimenteinstellungen aufgetreten.");
                    }

                }
            }
        }

        /// <summary>
        /// Called for exporting the participants
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void exportParticipants()
        {
            Logger.Log("Export Participants");

            String filePath = saveFileDialog();

            if (filePath != "")
            {
                ExportController.ExportExperimentParticipants(filePath, _exp.Participants);

                DisplaySuccess("Die Experimentteilnehmer wurden erfolgreich exportiert.", "Export erfolgreich");
            }

        }

        /// <summary>
        /// Called for exporting the experiment settings
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void exportExperimentSettings()
        {
            Logger.Log("Export Experimentsettings");

            String filePath = saveFileDialog();

            if (filePath != "")
            {
                ExportController.ExportExperimentSettings(filePath, _exp.Settings);

                DisplaySuccess("Die Experimenteinstellungen wurden erfolgreich exportiert.", "Export erfolgreich");
            }
        }

        /// <summary>
        /// Dialog for loading a file
        /// </summary>
        /// <returns></returns>
        /// <remarks>Used for importing participants and settings</remarks>
        private String loadFileDialog()
        {
            String filePath = "";
            _form.Invoke((Action)delegate
            {
                Stream myStream = null;
                OpenFileDialog loadFileDialog = new OpenFileDialog();

                loadFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                loadFileDialog.Filter = "XML Datei|*.xml";
                loadFileDialog.RestoreDirectory = true;

                if (loadFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (loadFileDialog.FileName != "")
                    {
                        Logger.Log("Selected Filename: " + loadFileDialog.FileName);

                        filePath = loadFileDialog.FileName;
                    }

                }
            });

            return filePath;
        }

        /// <summary>
        /// Dialog for saving a file
        /// </summary>
        /// <returns></returns>
        /// <remarks>Used for exporting the settings and participants</remarks>
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
