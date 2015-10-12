using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAnalyzer.ApplicationSettings;
using WebAnalyzer.Models.SettingsModel;
using WebAnalyzer.Util;

namespace WebAnalyzer.Models.Base
{
    /// <summary>
    /// Class which represents a experiment
    /// </summary>
    public class ExperimentModel
    {

        /// <summary>
        /// Name of the experiment
        /// </summary>
        private String _experimentName;

        /// <summary>
        /// Description of the experiment
        /// </summary>
        private String _description;

        /// <summary>
        /// Creationdate of the experiment
        /// </summary>
        private DateTime _createdAt;

        /// <summary>
        /// List of participants
        /// </summary>
        private List<ExperimentParticipant> _participants = new List<ExperimentParticipant>();

        /// <summary>
        /// Reference to experiment settings
        /// </summary>
        private ExperimentSettings _settings = new ExperimentSettings();

        /// <summary>
        /// Constructor for creating a new experiment
        /// </summary>
        /// <param name="experimentName">Name of the experiment</param>
        public ExperimentModel(String experimentName)
        {
            _experimentName = experimentName;
            _createdAt = DateTime.Now;
        }

        /// <summary>
        /// Constructor used for loading the experiment
        /// </summary>
        public ExperimentModel()
        {
            _createdAt = DateTime.Now;
        }

        /// <summary>
        /// Destructor which resets hidden settings
        /// </summary>
        ~ExperimentModel()
        {
            ResetInternSettings();
        }

        /// <summary>
        /// Getter/ Setter for the experiment name
        /// </summary>
        public String ExperimentName
        {
            get { return _experimentName; }
            set { _experimentName = value;  }
        }

        /// <summary>
        /// Getter/ Setter for the experiment description
        /// </summary>
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Getter/ Setter for the creation date
        /// </summary>
        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { _createdAt = value; }
        }

        /// <summary>
        /// Getter/ Setter for the experiment participants
        /// </summary>
        public List<ExperimentParticipant> Participants
        {
            get { return _participants; }
            set { _participants = value; }
        }


        /// <summary>
        /// Getter/ Setter for the experiment settings
        /// </summary>
        public ExperimentSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        /// <summary>
        /// Returns an array with all participant identifiers
        /// </summary>
        /// <returns>Array of participant identifiers</returns>
        /// <remarks>
        /// Used for displaying participants in the ui
        /// </remarks>
        public String[] GetParticipantArray()
        {
            String[] participantArray = new String[Participants.Count];

            for (int i = 0; i < Participants.Count; i++)
            {
                participantArray[i] = Participants[i].Identifier;
            }

            return participantArray;
        }

        /// <summary>
        /// Returns an array of participant uids
        /// </summary>
        /// <returns>Array of participant uids</returns>
        /// <remarks>
        /// Used for displaying participants in the ui
        /// </remarks>
        public String[] GetParticipantUIDs()
        {
            String[] uidArray = new String[Participants.Count];

            for (int i = 0; i < Participants.Count; i++)
            {
                uidArray[i] = Participants[i].UID;
            }

            return uidArray;
        }

        /// <summary>
        /// Returns an array with all domains
        /// </summary>
        /// <returns>Array of domains</returns>
        /// <remarks>
        /// Used for displaying domains in the ui
        /// </remarks>
        public String[] GetDomainSettingArray()
        {
            return Settings.GetDomainIdentifiers();
        }

        /// <summary>
        /// Returns an array with all domain uids
        /// </summary>
        /// <returns>Array of domain uids</returns>
        /// <remarks>
        /// Used for displaying domains in the ui
        /// </remarks>
        public String[] GetDomainSettingUIDs()
        {
            return Settings.GetDomainUIDs();
        }

        /// <summary>
        /// Returns the domain setting for the given uid
        /// </summary>
        /// <param name="uid">UID of the domain</param>
        /// <returns>The domain setting for the uid</returns>
        public DomainSettings GetDomainSettingByUid(String uid)
        {
            return Settings.GetDomainSettingByUid(uid);
        }

        /// <summary>
        /// Returns the participant for the given uid
        /// </summary>
        /// <param name="uid">UID of the participant</param>
        /// <returns>The participant for the uid</returns>
        public ExperimentParticipant GetParticipantByUID(String uid)
        {
            foreach (ExperimentParticipant par in Participants)
            {
                if (par.UID.Equals(uid))
                {
                    return par;
                }

            }

            return null;
        }

        /// <summary>
        /// Returns the base experiment location
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Uses the experiment settings and the experiment name.
        /// </remarks>
        public String GetBaseExperimentLocation()
        {
            return Properties.Settings.Default.Datalocation + _experimentName + "\\";
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode experiment = xmlDoc.CreateElement("experiment");

            /* name */

            XmlAttribute name = xmlDoc.CreateAttribute("name");

            name.Value = this.ExperimentName;

            experiment.Attributes.Append(name);

            /* Created At */

            XmlAttribute createdAt = xmlDoc.CreateAttribute("created-at");

            createdAt.Value = this.CreatedAt.ToString();

            experiment.Attributes.Append(createdAt);


            /* settings */

            XmlNode description = xmlDoc.CreateElement("description");

            description.InnerText = this.Description;

            experiment.AppendChild(description);

            /* settings */

            XmlNode internSettings = xmlDoc.CreateElement("intern-settings");

            SaveInternSettings(internSettings, xmlDoc);

            experiment.AppendChild(internSettings);

            return experiment;
        }

        /// <summary>
        /// Creates an xml representation of the hidden experiment settings
        /// </summary>
        /// <param name="internSettings">XMLNode for the settings</param>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        private void SaveInternSettings(XmlNode internSettings, XmlDocument xmlDoc)
        {

            /* log count */

            XmlComment logCountComment = xmlDoc.CreateComment("Amout of logs which are backed up");

            internSettings.AppendChild(logCountComment);

            XmlNode logCount = xmlDoc.CreateElement("log-count");

            logCount.InnerText = Properties.Settings.Default.LogCount.ToString();

            internSettings.AppendChild(logCount);


            /* websocket delay*/

            XmlComment websocketDelayComment = xmlDoc.CreateComment("Delay in which messages from the websocket message queue are sent (if possible) (In milliseconds)");

            internSettings.AppendChild(websocketDelayComment);

            XmlNode websocketDelay = xmlDoc.CreateElement("websocket-delay");

            websocketDelay.InnerText = Properties.Settings.Default.WSMessageDelay.ToString();

            internSettings.AppendChild(websocketDelay);

            /* mousetracking delay*/

            XmlComment mousetrackingIntervalComment = xmlDoc.CreateComment("Interval in which the mousedata gets collected when using mousetracking. (In milliseconds)");

            internSettings.AppendChild(mousetrackingIntervalComment);

            XmlNode mousetrackingInterval = xmlDoc.CreateElement("mousetracking-interval");

            mousetrackingInterval.InnerText = Properties.Settings.Default.MouseTrackingInterval.ToString();

            internSettings.AppendChild(mousetrackingInterval);


            /* data timeout*/

            XmlComment dataTimeoutComment = xmlDoc.CreateComment("Duration after which a message is too old, to be sent. (In milliseconds)");

            internSettings.AppendChild(dataTimeoutComment);

            XmlNode dataTimeout = xmlDoc.CreateElement("data-timeout");

            dataTimeout.InnerText = Properties.Settings.Default.DataTimeout.ToString();

            internSettings.AppendChild(dataTimeout);


            /* data timeout*/

            XmlComment forceMessageDelaySettingsComment = xmlDoc.CreateComment("Should the settings for the message delay be forced and not changed by the software.");

            internSettings.AppendChild(forceMessageDelaySettingsComment);

            XmlNode forceMessageDelaySettings = xmlDoc.CreateElement("force-message-delay");

            forceMessageDelaySettings.InnerText = Properties.Settings.Default.ForceMessageDelaySettings.ToString();

            internSettings.AppendChild(forceMessageDelaySettings);

        }

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="doc">XML Document which contains the data</param>
        /// <returns>The loaded object</returns>
        public static ExperimentModel LoadFromXML(XmlDocument doc)
        {
            ExperimentModel experiment = new ExperimentModel();
            XmlNode expNode = doc.DocumentElement.SelectSingleNode("/experiment");

            if (expNode == null)
            {
                Logger.Log("No Experiment Node found.");
                return null;
            }

            foreach(XmlAttribute attr in expNode.Attributes){
                switch (attr.Name)
                {
                    case "name":
                        experiment.ExperimentName = attr.Value;
                        break;
                    case "created-at":
                        experiment.CreatedAt = DateTime.Parse(attr.Value);
                        break;
                }
            }

            foreach (XmlNode child in expNode.ChildNodes)
            {
                if (child.Name == "intern-settings")
                {
                    foreach (XmlNode setting in child.ChildNodes)
                    {
                        LoadSetting(setting);
                    }

                    Properties.Settings.Default.Save();
                }
                else if (child.Name == "description")
                {
                    experiment.Description = child.InnerText;
                }
            }

            return experiment;
        }

        /// <summary>
        /// Loads the settings from the xml
        /// </summary>
        /// <param name="setting">XMLNode containing the setting</param>
        private static void LoadSetting(XmlNode setting)
        {
            switch (setting.Name)
            {
                case "websocket-delay":
                    Properties.Settings.Default.WSMessageDelay = int.Parse(setting.InnerText);
                    break;
                case "log-count":
                    Properties.Settings.Default.LogCount = int.Parse(setting.InnerText);
                    break;
                case "mousetracking-interval":
                    Properties.Settings.Default.MouseTrackingInterval = int.Parse(setting.InnerText);
                    break;
                case "data-timeout":
                    Properties.Settings.Default.DataTimeout = int.Parse(setting.InnerText);
                    break;
                case "force-message-delay":
                    Properties.Settings.Default.ForceMessageDelaySettings = Boolean.Parse(setting.InnerText);
                    break;
            }
        }

        /// <summary>
        /// Resets the experiment variables
        /// </summary>
        private void ResetInternSettings(){
            AppSettings.ResetExperimentVariables();
        }

        /// <summary>
        /// Creates a new experiment with the given name
        /// </summary>
        /// <param name="name">The name for the new experiment</param>
        /// <returns>The new experiment</returns>
        public static ExperimentModel CreateExperiment(String name)
        {
            ExperimentModel experiment = new ExperimentModel(name);

            return experiment;
        }
    }
}
