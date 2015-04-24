using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Export;

namespace WebAnalyzer.Experiment
{
    class ExperimentController
    {

        private static ExperimentController instance;

        public static ExperimentController getInstance()
        {
            if (instance == null)
            {
                instance = new ExperimentController();
            }

            return instance;
        }

        private ExperimentModel _experiment;

        private ExperimentController()
        {
            
        }

        public void CreateExperiment(String name)
        {
            _experiment = new ExperimentModel(name);
        }

        public void StartExperiment()
        {
            // start server etc.
            // set listeners
        }

        public void StopExperiment()
        {
            // stop server etc.

            // export raw data

            this.ExportToXML();
        }

        public void AddPositionData(String url, int xPosition, int yPosition)
        {
            String timestamp = Timestamp.GetUnixTimestamp();

            _experiment.AddPositionData(url, xPosition, yPosition, timestamp);
        }

        public void AddPositionData(String url, PositionDataModel posModel)
        {
            _experiment.AddPositionData(url, posModel);
        }

        private void ExportToXML()
        {
            String timestamp = Timestamp.GetUnixTimestamp();

            String filename = timestamp + _experiment.ExperimentName;

            new ExperimentExporter().ExportToXML(_experiment.GetBaseExperimentLocation(), filename, _experiment);
        }
    }
}
