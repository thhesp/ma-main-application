using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

using WebAnalyzer.Models.SettingsModel;
using WebAnalyzer.Models.Base;
using WebAnalyzer.Util;
using WebAnalyzer.Models.DataModel;
using System.IO;

namespace WebAnalyzer.Controller
{
    /// <summary>
    /// Class which is used for loading data from files.
    /// </summary>
    public static class LoadController
    {
        /// <summary>
        /// Validates if the given path contains experiment data.
        /// </summary>
        /// <param name="path">Directory to check for experiment data</param>
        public static Boolean ValidateExperimentFolder(String path)
        {
            if (System.IO.Directory.Exists(path))
            {
                if (!System.IO.File.Exists(path + "\\" + Properties.Settings.Default.ExperimentFilename))
                {
                    return false;
                }

                if (!System.IO.File.Exists(path + "\\" + Properties.Settings.Default.ParticipantsFilename))
                {
                    return false;
                }

                if (!System.IO.File.Exists(path + "\\" + Properties.Settings.Default.SettingsFilename))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Validates if a experiment already exists by checking the path.
        /// </summary>
        /// <param name="exp">New Experiment for which to check if there is already an experiment with the same name.</param>
        public static Boolean ValidateIfExperimentDoesNotExist(ExperimentModel exp)
        {
            String dir = exp.GetBaseExperimentLocation();

            if (ValidateExperimentFolder(dir))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Loads the experiment from path.
        /// </summary>
        /// <remarks>Calls LoadParticipants and LoadSettings to load them.</remarks>
        /// <param name="path">Path to the directory from which to load the experiment.</param>
        public static ExperimentModel LoadExperiment(String path)
        {
            String experimentPath = path + "\\" + Properties.Settings.Default.ExperimentFilename;

            if (ValidateXMLFile(experimentPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(experimentPath);

                ExperimentModel experiment = ExperimentModel.LoadFromXML(doc);

                if (experiment != null)
                {
                    experiment.Participants = LoadParticipants(path);
                    experiment.Settings = LoadSettings(path);
                }

                return experiment;
            }
            else
            {
                Logger.Log("Invalid experiment file!");
            }
            // throw exception?
            return null;
        }

        /// <summary>
        /// Loads the Participants and returns them as a list.
        /// </summary>
        /// <remarks>Used when importing data from an existing experiment.</remarks>
        /// <param name="path">Path to the directory from which to load the experiment participants.</param>
        public static List<ExperimentParticipant> LoadParticipants(String path)
        {
            String participantsPath = path + "\\" + Properties.Settings.Default.ParticipantsFilename;

            if (ValidateXMLFile(participantsPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(participantsPath);

                XmlNode participantsNode = doc.DocumentElement.SelectSingleNode("/participants");

                if (participantsNode == null)
                {
                    return null;
                }

                List<ExperimentParticipant> participants = new List<ExperimentParticipant>();

                foreach (XmlNode participantNode in participantsNode.ChildNodes)
                {
                    ExperimentParticipant participant = ExperimentParticipant.LoadFromXML(participantNode);
                    if (participant != null)
                    {
                        participants.Add(participant);
                    }
                }

                return participants;
            }
            else
            {
                Logger.Log("Invalid participants file!");
            }
            // throw exception?
            return null;

        }

        /// <summary>
        /// Loads the Settings and returns them as a ExperimentSettings object.
        /// </summary>
        /// <remarks>Used when importing data from an existing experiment.</remarks>
        /// <param name="path">Path to the directory from which to load the experiment settings.</param>
        public static ExperimentSettings LoadSettings(String path)
        {
            String settingsPath = path + "\\" + Properties.Settings.Default.SettingsFilename;

            if (ValidateXMLFile(settingsPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(settingsPath);

                return ExperimentSettings.LoadFromXML(doc);
            }
            else
            {
                Logger.Log("Invalid settings file!");
            }
            // throw exception?
            return null;
        }

        /// <summary>
        /// Loads test data and returns it as an TestModel object.
        /// </summary>
        /// <param name="path">Path to the xml file from which to load the test.</param>
        public static TestModel LoadTest(String path)
        {
            if (ValidateXMLFile(path))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                return TestModel.LoadFromXML(doc);
            }
            // throw exception?
            return null;
        }

        /// <summary>
        /// Loads raw data and returns it as an RawData object.
        /// </summary>
        /// <param name="path">Path to the xml file from which to load the raw data.</param>
        public static RawTrackingData LoadRawData(String path)
        {
            if (ValidateXMLFile(path))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                return RawTrackingData.LoadFromXML(doc);
            }
            // throw exception?
            return null;
        }


        /// <summary>
        /// Generates Path to Testdata Location of Testrun
        /// </summary>
        /// <param name="experiment">Current experiment to get the data from</param>
        /// <param name="participant">Current participant to get the data from</param>
        public static String GetTestdataLocation(ExperimentModel experiment, ExperimentParticipant participant)
        {
            String dir = experiment.GetBaseExperimentLocation();
            dir += Properties.Settings.Default.ExperimentDataLocation.Replace("{1}", participant.Identifier);

            FileIO.CheckPathAndCreate(dir);

            return dir;
        }

        /// <summary>
        /// Generates Path to Rawdata Location of Testrun
        /// </summary>
        /// <param name="experiment">Current experiment to get the data from</param>
        /// <param name="participant">Current participant to get the data from</param>
        public static String GetRawdataLocation(ExperimentModel experiment, ExperimentParticipant participant)
        {
            String dir = experiment.GetBaseExperimentLocation();
            dir += Properties.Settings.Default.RawdataLocation.Replace("{1}", participant.Identifier);

            FileIO.CheckPathAndCreate(dir);

            return dir;
        }

        /// <summary>
        /// Generates Path to Rawdata Location of Testrun
        /// </summary>
        /// <param name="experiment">Current experiment to get the data from</param>
        /// <param name="participant">Current participant to get the data from</param>
        /// <param name="path">Path to the testdata</param>
        public static String GetAssociatedRawDataForTestdata(ExperimentModel experiment, ExperimentParticipant participant, String path)
        {
            String dir = experiment.GetBaseExperimentLocation();
            dir += Properties.Settings.Default.RawdataLocation.Replace("{1}", participant.Identifier);

            bool exists = Directory.Exists(dir);
            if (exists)
            {
                String filename = Path.GetFileName(path);

                return Path.Combine(dir, filename);
            }

           

            return "";
        }

        /// <summary>
        /// Validates if the given path contains testrun data.
        /// </summary>
        /// <param name="path">File to check for testrun data</param>
        public static Boolean ValidateXMLFile(String path)
        {
            if (!File.Exists(path))
            {
                Logger.Log("File " + path + " does not exist.");
                return false;
            }

            if(!path.Contains(".xml")){
                Logger.Log("File " + path + " is no XML File.");
                return false;
            }

            /*var doc = new XmlDocument();
            try
            {
                doc.LoadXml(path);
            }
            catch (XmlException e)
            {
                // put code here that should be executed when the XML is not valid.

                Logger.Log("XML File is not valid: " + e.Message);

                return false;
            }*/

            return true;
        }
    }
}
