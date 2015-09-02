using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Models.Base;
using WebAnalyzer.Util;
using WebAnalyzer.Models.SettingsModel;
using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Controller
{
    /// <summary>
    /// Controller which is used for exporting/saving data to files.
    /// </summary>
    public class ExportController
    {

        /// <summary>
        /// Saves the experiment data to a file and checks the file path.
        /// </summary>
        /// <remarks>Calls the SaveExperimentSettings and SaveExperimentParticipants methods.</remarks>
        /// <param name="experiment">Experiment to save</param>
        public static Boolean SaveExperiment(ExperimentModel experiment)
        {
            String dir = experiment.GetBaseExperimentLocation();

            FileIO.CheckPath(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(experiment.ToXML(xmlDoc));

            xmlDoc.Save(dir + Properties.Settings.Default.ExperimentFilename);

            if (experiment.Settings != null)
            {
                SaveExperimentSettings(dir, experiment.Settings);
            }

            if (experiment.Participants != null)
            {
                SaveExperimentParticipants(dir, experiment.Participants);
            }
            

            return true;
        }

        /// <summary>
        /// Saves the experiment settings to a file and checks the file path.
        /// </summary>
        /// <param name="experiment">Experiment of which the settings to save</param>
        public static Boolean SaveExperimentSettings(ExperimentModel experiment)
        {
            String dir = experiment.GetBaseExperimentLocation();

            FileIO.CheckPath(dir);
            if (experiment.Settings != null)
            {
                return SaveExperimentSettings(dir, experiment.Settings);
            }

            return true;
        }

        /// <summary>
        /// Saves the experiment settings to a file.
        /// </summary>
        /// <param name="baseDir">Directory to save to.</param>
        /// <param name="settings">The experiment settings to save.</param>
        private static Boolean SaveExperimentSettings(String baseDir, ExperimentSettings settings)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(settings.ToXML(xmlDoc));

            xmlDoc.Save(baseDir + Properties.Settings.Default.SettingsFilename);

            return true;
        }

        /// <summary>
        /// Saves the experiment participants to a file and checks the file path.
        /// </summary>
        /// <param name="experiment">Experiment of which the participants to save</param>
        public static Boolean SaveExperimentParticipants(ExperimentModel experiment)
        {
            String dir = experiment.GetBaseExperimentLocation();

            FileIO.CheckPath(dir);
            if (experiment.Participants != null)
            {
                return SaveExperimentParticipants(dir, experiment.Participants);
            }

            return true;
        }

        /// <summary>
        /// Saves the experiment participants to a file.
        /// </summary>
        /// <param name="baseDir">Directory to save to.</param>
        /// <param name="participants">The experiment participants to save.</param>
        private static Boolean SaveExperimentParticipants(String baseDir, List<ExperimentParticipant> participants)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlNode participantsNode = xmlDoc.CreateElement("participants");

            foreach (ExperimentParticipant participant in participants)
            {
                participantsNode.AppendChild(participant.ToXML(xmlDoc));
            }

            xmlDoc.AppendChild(participantsNode);

            xmlDoc.Save(baseDir + Properties.Settings.Default.ParticipantsFilename);

            return true;
        }

        /// <summary>
        /// Saves the given testrun to the given experiment and given participant.
        /// </summary>
        /// <param name="experiment">The experiment to which the testrun belongs.</param>
        /// <param name="currentParticipant">The participant to which the testrun belongs</param>
        /// <param name="testrun">The testdata</param>
        public static Boolean SaveExperimentTestRun(ExperimentModel experiment, ExperimentParticipant currentParticipant, TestModel testrun){
            String dir = experiment.GetBaseExperimentLocation();
            dir += Properties.Settings.Default.RawdataLocation.Replace("{1}", currentParticipant.Identifier);

            String timestamp = Util.Timestamp.GetDateTime(testrun.Started);

            FileIO.CheckPath(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(testrun.ToXML(xmlDoc));

            xmlDoc.Save(dir + "\\" + timestamp + ".xml");

            SaveExperimentFixations(experiment, currentParticipant, timestamp, testrun);
            SaveExperimentStatistics(experiment, currentParticipant, timestamp, testrun);
            SaveExperimentAOI(experiment, currentParticipant, timestamp, testrun);

            return true;
        }

        /// <summary>
        /// Saves the given testrun fixations to the given experiment and given participant.
        /// </summary>
        /// <param name="experiment">The experiment to which the testrun belongs.</param>
        /// <param name="currentParticipant">The participant to which the testrun belongs</param>
        /// <param name="timestamp">Timestamp to be used for the file name.</param>
        /// <param name="testrun">The testdata</param>
        public static Boolean SaveExperimentFixations(ExperimentModel experiment, ExperimentParticipant currentParticipant, String timestamp, TestModel testrun)
        {
            String dir = experiment.GetBaseExperimentLocation();
            dir += Properties.Settings.Default.FixdataLocation.Replace("{1}", currentParticipant.Identifier);

            FileIO.CheckPath(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(testrun.GenerateFixationXML(xmlDoc, false));

            xmlDoc.Save(dir + "\\" + timestamp + ".xml");
            

            return true;
        }

        /// <summary>
        /// Saves the given testrun statistics to the given experiment and given participant.
        /// </summary>
        /// <param name="experiment">The experiment to which the testrun belongs.</param>
        /// <param name="currentParticipant">The participant to which the testrun belongs</param>
        /// <param name="timestamp">Timestamp to be used for the file name.</param>
        /// <param name="testrun">The testdata</param>
        public static Boolean SaveExperimentStatistics(ExperimentModel experiment, ExperimentParticipant currentParticipant, String timestamp, TestModel testrun)
        {
            String dir = experiment.GetBaseExperimentLocation();
            dir += Properties.Settings.Default.StatisticsLocation.Replace("{1}", currentParticipant.Identifier);

            FileIO.CheckPath(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(testrun.GenerateStatisticsXML(xmlDoc, false));

            xmlDoc.Save(dir + "\\" + timestamp + ".xml");


            return true;
        }

        /// <summary>
        /// Saves the given testrun aois to the given experiment and given participant.
        /// </summary>
        /// <param name="experiment">The experiment to which the testrun belongs.</param>
        /// <param name="currentParticipant">The participant to which the testrun belongs</param>
        /// <param name="timestamp">Timestamp to be used for the file name.</param>
        /// <param name="testrun">The testdata</param>
        public static Boolean SaveExperimentAOI(ExperimentModel experiment, ExperimentParticipant currentParticipant, String timestamp, TestModel testrun)
        {
            String dir = experiment.GetBaseExperimentLocation();
            dir += Properties.Settings.Default.AOILocation.Replace("{1}", currentParticipant.Identifier);

            FileIO.CheckPath(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(testrun.GenerateAOIXML(experiment.Settings, xmlDoc, false));

            xmlDoc.Save(dir + "\\" + timestamp + ".xml");


            return true;
        }

    }
}
