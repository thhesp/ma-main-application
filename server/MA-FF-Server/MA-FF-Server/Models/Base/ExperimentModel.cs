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
    public class ExperimentModel
    {

        private String _experimentName;
        private DateTime _createdAt;

        private List<ExperimentParticipant> _participants = new List<ExperimentParticipant>();

        private ExperimentSettings _settings = new ExperimentSettings();

        public ExperimentModel(String experimentName)
        {
            _experimentName = experimentName;
            _createdAt = DateTime.Now;
        }

        public ExperimentModel()
        {
            _createdAt = DateTime.Now;
        }

        ~ExperimentModel()
        {
            ResetInternSettings();
        }

        public String ExperimentName
        {
            get { return _experimentName; }
            set { _experimentName = value;  }
        }

        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { _createdAt = value; }
        }

        public List<ExperimentParticipant> Participants
        {
            get { return _participants; }
            set { _participants = value; }
        }

        public ExperimentSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        public String[] GetParticipantArray()
        {
            String[] participantArray = new String[Participants.Count];

            for (int i = 0; i < Participants.Count; i++)
            {
                participantArray[i] = Participants[i].Identifier;
            }

            return participantArray;
        }

        public String[] GetParticipantUIDs()
        {
            String[] uidArray = new String[Participants.Count];

            for (int i = 0; i < Participants.Count; i++)
            {
                uidArray[i] = Participants[i].UID;
            }

            return uidArray;
        }

        public String[] GetDomainSettingArray()
        {
            return Settings.GetDomainIdentifiers();
        }

        public String[] GetDomainSettingUIDs()
        {
            return Settings.GetDomainUIDs();
        }

        public DomainSettings GetDomainSettingByUid(String uid)
        {
            return Settings.GetDomainSettingByUid(uid);
        }

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

        public String GetBaseExperimentLocation()
        {
            return Properties.Settings.Default.Datalocation + _experimentName + "\\";
        }

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

            XmlNode internSettings = xmlDoc.CreateElement("intern-settings");

            SaveInternSettings(internSettings, xmlDoc);

            experiment.AppendChild(internSettings);

            return experiment;
        }

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



        }

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
            }

            return experiment;
        }

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
            }
        }

        private void ResetInternSettings(){
            AppSettings.ResetExperimentVariables();
        }

        public static ExperimentModel CreateExperiment(String name)
        {
            ExperimentModel experiment = new ExperimentModel(name);

            return experiment;
        }
    }
}
