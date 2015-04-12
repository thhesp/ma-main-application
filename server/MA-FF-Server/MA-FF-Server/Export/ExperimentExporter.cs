using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WebAnalyzer.DataModel;

namespace WebAnalyzer.Export
{
    class ExperimentExporter
    {

        public ExperimentExporter()
        {

        }

        public Boolean ExportToXML(String filename, ExperimentModel experiment)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(experiment.ToXML(xmlDoc));

            xmlDoc.Save(filename + ".xml");

            return true;
        }


    }
}
