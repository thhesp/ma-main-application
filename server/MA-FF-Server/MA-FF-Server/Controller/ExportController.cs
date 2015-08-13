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
    public class ExportController
    {

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

        private static Boolean SaveExperimentSettings(String baseDir, ExperimentSettings settings)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(settings.ToXML(xmlDoc));

            xmlDoc.Save(baseDir + Properties.Settings.Default.SettingsFilename);

            return true;
        }

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


        public static Boolean SaveExperimentTestRun(ExperimentModel experiment, ExperimentParticipant currentParticipant, TestModel testrun){
            String dir = experiment.GetBaseExperimentLocation() + Properties.Settings.Default.RawdataLocation;
            String timestamp = Timestamp.GetUnixTimestamp();

            FileIO.CheckPath(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(testrun.ToXML(xmlDoc));

            xmlDoc.Save(dir + currentParticipant.Identifier + "-" + timestamp + ".xml");

            SaveExperimentFixations(experiment, currentParticipant, testrun);
            SaveExperimentStatistics(experiment, currentParticipant, testrun);

            return true;
        }

        public static Boolean SaveExperimentFixations(ExperimentModel experiment, ExperimentParticipant currentParticipant, TestModel testrun){
        
            String dir = experiment.GetBaseExperimentLocation() + Properties.Settings.Default.FixdataLocation;
            String timestamp = Timestamp.GetUnixTimestamp();

            FileIO.CheckPath(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(testrun.GenerateFixationXML(xmlDoc));

            xmlDoc.Save(dir + currentParticipant.Identifier + "-" + timestamp + ".xml");
            

            return true;
        }

        public static Boolean SaveExperimentStatistics(ExperimentModel experiment, ExperimentParticipant currentParticipant, TestModel testrun)
        {

            String dir = experiment.GetBaseExperimentLocation() + Properties.Settings.Default.StatisticsLocation;
            String timestamp = Timestamp.GetUnixTimestamp();

            FileIO.CheckPath(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(testrun.GenerateStatisticsXML(xmlDoc));

            xmlDoc.Save(dir + currentParticipant.Identifier + "-" + timestamp + ".xml");


            return true;
        }

    }
}
