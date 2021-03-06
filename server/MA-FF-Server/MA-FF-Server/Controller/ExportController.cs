﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Models.Base;
using WebAnalyzer.Util;
using WebAnalyzer.Models.SettingsModel;
using WebAnalyzer.Models.DataModel;
using System.IO;
using WebAnalyzer.Models.AlgorithmModel;

namespace WebAnalyzer.Controller
{
    /// <summary>
    /// Controller which is used for exporting/saving data to files.
    /// </summary>
    public class ExportController
    {

        /// <summary>
        /// Enum which export formats are possible
        /// </summary>
        public enum EXPORT_FORMATS { XML = 0, CSV = 1 };

        /// <summary>
        /// Saves the experiment data to a file and checks the file path.
        /// </summary>
        /// <remarks>Calls the SaveExperimentSettings and SaveExperimentParticipants methods.</remarks>
        /// <param name="experiment">Experiment to save</param>
        public static Boolean SaveExperiment(ExperimentModel experiment)
        {
            String dir = experiment.GetBaseExperimentLocation();

            FileIO.CheckPathAndCreate(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", null, null));;

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

            FileIO.CheckPathAndCreate(dir);
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

            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", null, null));;

            xmlDoc.AppendChild(settings.ToXML(xmlDoc));

            xmlDoc.Save(baseDir + Properties.Settings.Default.SettingsFilename);

            return true;
        }

        /// <summary>
        /// Exports the experiment settings to a file.
        /// </summary>
        /// <param name="file">Path to file</param>
        /// <param name="settings">The experiment settings to save.</param>
        public static Boolean ExportExperimentSettings(String file, ExperimentSettings settings)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", null, null)); ;

            xmlDoc.AppendChild(settings.ToXML(xmlDoc));

            xmlDoc.Save(file);

            return true;
        }

        /// <summary>
        /// Saves the experiment participants to a file and checks the file path.
        /// </summary>
        /// <param name="experiment">Experiment of which the participants to save</param>
        public static Boolean SaveExperimentParticipants(ExperimentModel experiment)
        {
            String dir = experiment.GetBaseExperimentLocation();

            FileIO.CheckPathAndCreate(dir);
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

            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", null, null));;

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
        /// Exports the experiment participants to a file.
        /// </summary>
        /// <param name="file">Path to file.</param>
        /// <param name="participants">The experiment participants to save.</param>
        public static Boolean ExportExperimentParticipants(String file, List<ExperimentParticipant> participants)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", null, null)); ;

            XmlNode participantsNode = xmlDoc.CreateElement("participants");

            foreach (ExperimentParticipant participant in participants)
            {
                participantsNode.AppendChild(participant.ToXML(xmlDoc));
            }

            xmlDoc.AppendChild(participantsNode);

            xmlDoc.Save(file);

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
            dir += Properties.Settings.Default.ExperimentDataLocation.Replace("{1}", currentParticipant.Identifier);

            String filename = testrun.Filename;

            FileIO.CheckPathAndCreate(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", null, null));;

            xmlDoc.AppendChild(testrun.ToXML(currentParticipant, xmlDoc));

            xmlDoc.Save(dir + "\\" + filename);

            SaveExperimentStatistics(experiment, currentParticipant, filename, testrun);
            return true;
        }

        /// <summary>
        /// Saves the given testrun statistics to the given experiment and given participant.
        /// </summary>
        /// <param name="experiment">The experiment to which the testrun belongs.</param>
        /// <param name="currentParticipant">The participant to which the testrun belongs</param>
        /// <param name="filename">Filename to be used for the export.</param>
        /// <param name="testrun">The testdata</param>
        public static Boolean SaveExperimentStatistics(ExperimentModel experiment, ExperimentParticipant currentParticipant, String filename, TestModel testrun)
        {
            String dir = experiment.GetBaseExperimentLocation();
            dir += Properties.Settings.Default.StatisticsLocation.Replace("{1}", currentParticipant.Identifier);

            FileIO.CheckPathAndCreate(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", null, null));;

            xmlDoc.AppendChild(testrun.GenerateStatisticsXML(xmlDoc, false));

            xmlDoc.Save(dir + "\\" + filename);


            return true;
        }

        /// <summary>
        /// Saves the given rawdata of the testrun to the given experiment and given participant.
        /// </summary>
        /// <param name="experiment">The experiment to which the testrun belongs.</param>
        /// <param name="currentParticipant">The participant to which the testrun belongs</param>
        /// <param name="testrun">The testdata to which the raw data belongs</param>
        /// <param name="rawData">Raw data to be exported</param>
        public static Boolean SaveExperimentRawData(ExperimentModel experiment, ExperimentParticipant currentParticipant, TestModel testrun, RawTrackingData rawData)
        {
            String dir = experiment.GetBaseExperimentLocation();
            dir += Properties.Settings.Default.RawdataLocation.Replace("{1}", currentParticipant.Identifier);

            String filename = testrun.Filename;

            FileIO.CheckPathAndCreate(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", null, null));;

            xmlDoc.AppendChild(rawData.ToXML(currentParticipant, xmlDoc));

            xmlDoc.Save(dir + "\\" + filename);
            return true;
        }


        /// <summary>
        /// Export the given testrun to the given directory and filename with the given format.
        /// </summary>
        /// <param name="testrun">The testdata</param>
        /// <param name="dir">Directory to export to</param>
        /// <param name="participant">the Participant to which the data belongs</param>
        /// <param name="filename">Filename to use</param>
        /// <param name="format">Format in which to save the data</param>
        public static Boolean ExportExperimentTestRun(TestModel testrun, ExperimentParticipant participant, String dir, String filename, EXPORT_FORMATS format)
        {
            if (Directory.Exists(dir))
            {

                Logger.Log("Exporting to: " + dir + "\\" + filename + ".xml");
                switch (format)
                {
                    case EXPORT_FORMATS.XML:
                        XmlDocument xmlDoc = new XmlDocument();

                        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", null, null));;

                        xmlDoc.AppendChild(testrun.ToXML(participant, xmlDoc));

                        xmlDoc.Save(dir + "\\" + filename + ".xml");
                        break;

                    case EXPORT_FORMATS.CSV:

                        break;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Export the given raw data to the given directory and filename with the given format.
        /// </summary>
        /// <param name="rawData">The data to be exported</param>
        /// <param name="participant">The participant</param>
        /// <param name="dir">Directory to export to</param>
        /// <param name="filename">Filename to use</param>
        /// <param name="format">Format in which to save the data</param>
        public static Boolean ExportExperimentRawData(RawTrackingData rawData, ExperimentParticipant participant, String dir, String filename, EXPORT_FORMATS format)
        {
            if (Directory.Exists(dir))
            {

                Logger.Log("Exporting RawData to: " + dir + "\\" + filename + ".xml");
                switch (format)
                {
                    case EXPORT_FORMATS.XML:
                        XmlDocument xmlDoc = new XmlDocument();

                        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", null, null)); ;

                        xmlDoc.AppendChild(rawData.ToXML(participant, xmlDoc));

                        xmlDoc.Save(dir + "\\" + filename + "-raw.xml");
                        break;

                    case EXPORT_FORMATS.CSV:

                        break;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Export the given testrun fixations to the given directory and filename with the given format.
        /// </summary>
        /// <param name="testrun">The testdata</param>
        /// <param name="participant">The current participant</param>
        /// <param name="algorithm">The used algorithm while generating the fixations</param>
        /// <param name="dir">Directory to export to</param>
        /// <param name="filename">Filename to use</param>
        /// <param name="format">Format in which to save the data</param>
        /// <param name="includeGazeData">Should the export contain data about the single gazes</param>
        public static Boolean SaveExperimentFixations(TestModel testrun, ExperimentParticipant participant, Algorithm algorithm, String dir, String filename, EXPORT_FORMATS format, Boolean includeGazeData)
        {
            if (Directory.Exists(dir))
            {

                Logger.Log("Exporting fixations to: " + dir + "\\" + filename + "-fixations.xml");
                switch (format)
                {
                    case EXPORT_FORMATS.XML:
                        XmlDocument xmlDoc = new XmlDocument();

                        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", null, null));;

                        xmlDoc.AppendChild(testrun.GenerateFixationXML(participant, algorithm, xmlDoc, includeGazeData));

                        xmlDoc.Save(dir + "\\" + filename + "-fixations.xml");
                        break;

                    case EXPORT_FORMATS.CSV:

                        break;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Export the given testrun aois to the given directory and filename with the given format.
        /// </summary>
        /// <param name="experiment">The current experiment</param>
        /// <param name="testrun">The testdata</param>
        /// <param name="participant">The current participant</param>
        /// <param name="algorithm">The used algorithm while generating the aoi</param>
        /// <param name="dir">Directory to export to</param>
        /// <param name="filename">Filename to use</param>
        /// <param name="format">Format in which to save the data</param>
        /// <param name="includeGazeData">Should the export contain data about the single gazes</param>
        public static Boolean SaveExperimentAOI(ExperimentModel experiment, TestModel testrun, ExperimentParticipant participant, Algorithm algorithm, String dir, String filename, EXPORT_FORMATS format, Boolean includeGazeData)
        {
            if (Directory.Exists(dir))
            {

                Logger.Log("Exporting AOIs to: " + dir + "\\" + filename + "-aois.xml");
                switch (format)
                {
                    case EXPORT_FORMATS.XML:
                        XmlDocument xmlDoc = new XmlDocument();

                        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", null, null));;

                        xmlDoc.AppendChild(testrun.GenerateAOIXML(experiment.Settings, participant, algorithm, xmlDoc, includeGazeData));

                        xmlDoc.Save(dir + "\\" + filename + "-aois.xml");
                        break;

                    case EXPORT_FORMATS.CSV:

                        break;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Export the given testrun saccades to the given directory and filename with the given format.
        /// </summary>
        /// <param name="testrun">The testdata</param>
        /// <param name="participant">The current participant</param>
        /// <param name="algorithm">The used algorithm while generating the saccades</param>
        /// <param name="dir">Directory to export to</param>
        /// <param name="filename">Filename to use</param>
        /// <param name="format">Format in which to save the data</param>
        /// <param name="includeGazeData">Should the export contain data about the single gazes</param>
        public static Boolean SaveExperimentSaccades(TestModel testrun, ExperimentParticipant participant, Algorithm algorithm, String dir, String filename, EXPORT_FORMATS format, Boolean includeGazeData)
        {
            if (Directory.Exists(dir))
            {

                Logger.Log("Exporting Saccades to: " + dir + "\\" + filename + "-saccades.xml");
                switch (format)
                {
                    case EXPORT_FORMATS.XML:
                        XmlDocument xmlDoc = new XmlDocument();

                        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", null, null));

                        xmlDoc.AppendChild(testrun.GenerateSaccadesXML(participant, algorithm, xmlDoc, includeGazeData));

                        xmlDoc.Save(dir + "\\" + filename + "-saccades.xml");
                        break;

                    case EXPORT_FORMATS.CSV:

                        break;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
