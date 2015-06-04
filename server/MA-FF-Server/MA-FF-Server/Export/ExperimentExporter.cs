using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Util;

namespace WebAnalyzer.Export
{
    class ExperimentExporter
    {
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

            ExperimentExporter.checkPath(dir);
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

        private static void checkPath(String dir)
        {
            bool exists = System.IO.Directory.Exists(dir);

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(dir);
            }
        }


    }
}
