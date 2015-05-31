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

            String filename = timestamp + experiment.ExperimentName;

            Logger.Log("Exporting XML: " + dir + filename + ".xml");

            ExperimentExporter.checkPath(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(experiment.ToXML(xmlDoc));

            xmlDoc.Save(dir + filename + ".xml");


            String statsFilename = filename + "-stats";

            XmlDocument statsDoc = new XmlDocument();

            statsDoc.AppendChild(experiment.GenerateStatisticsXML(statsDoc));

            statsDoc.Save(dir + statsFilename + ".xml");

            return true;
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
