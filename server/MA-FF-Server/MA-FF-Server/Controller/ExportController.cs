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
            String dir = experiment.GetBaseExperimentLocation() + Properties.Settings.Default.TestdataLocation;

            FileIO.CheckPath(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(testrun.ToXML(xmlDoc));

            xmlDoc.Save(dir + currentParticipant.Identifier);
            

            return true;
        }

        /*
        public static Boolean ExportToXML(ExperimentModel experiment)
        {
            String dir = experiment.GetBaseExperimentLocation();

            String timestamp = Timestamp.GetUnixTimestamp();



            String filename = ExportDataXML(experiment, dir, timestamp);

            ExportStatisticsXML(experiment, dir, filename);
            ExportFixationXML(experiment, dir, filename);

            return true;
        }

        private static string ExportDataXML(ExperimentModel experiment, String dir, String timestamp)
        {
            String filename = timestamp + experiment.ExperimentName;

            Logger.Log("Exporting XML: " + dir + filename + ".xml");

            FileIO.CheckPath(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(experiment.ToXML(xmlDoc));

            xmlDoc.Save(dir + filename + ".xml");
            return filename;
        }

        private static void ExportStatisticsXML(ExperimentModel experiment, String dir, String filename)
        {
            String statsFilename = filename + "-stats";

            XmlDocument statsDoc = new XmlDocument();

            statsDoc.AppendChild(experiment.GenerateStatisticsXML(statsDoc));

            statsDoc.Save(dir + statsFilename + ".xml");
        }

        private static void ExportFixationXML(ExperimentModel experiment, String dir, String filename)
        {
            String fixationFilename = filename + "-fixations";

            XmlDocument fixationDoc = new XmlDocument();

            fixationDoc.AppendChild(experiment.GenerateFixationXML(fixationDoc));

            fixationDoc.Save(dir + fixationFilename + ".xml");
        }

        */
    }
}
