using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Export
{
    class ExperimentExporter
    {

        public ExperimentExporter()
        {

        }

        public Boolean ExportToXML(String dir, String filename, ExperimentModel experiment)
        {
            this.checkPath(dir);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(experiment.ToXML(xmlDoc));

            xmlDoc.Save(dir + filename + ".xml");

            return true;
        }

        private void checkPath(String dir)
        {
            bool exists = System.IO.Directory.Exists(dir);

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(dir);
            }
        }


    }
}
