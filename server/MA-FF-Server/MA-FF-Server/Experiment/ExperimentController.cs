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
            Logger.Log("Start Experiment");
            // start server etc.
            // set listeners
        }

        public void StopExperiment()
        {
            Logger.Log("Stop Experiment");
            // stop server etc.

            // export raw data

            this.ExportToXML();
        }

        public String PreparePositionData(double xPosition, double yPosition, String startTime, String endTime, String duration)
        {
            return _experiment.PreparePositionData(xPosition, yPosition, startTime, endTime, duration);
        }

        public Boolean AssignPositionToWebpage(String uniqueId, String url)
        {
            return _experiment.AssignPositionToWebpage(uniqueId, url);
        }

        public Boolean AssignPositionToWebpage(PositionDataModel posModel, String url)
        {
            return _experiment.AssignPositionToWebpage(posModel, url);
        }

        public PositionDataModel GetPosition(String uniqueId)
        {
            return _experiment.GetPosition(uniqueId);
        }

        private void ExportToXML()
        {
            String timestamp = Timestamp.GetUnixTimestamp();

            String filename = timestamp + _experiment.ExperimentName;

            new ExperimentExporter().ExportToXML(_experiment.GetBaseExperimentLocation(), filename, _experiment);
        }
    }
}
