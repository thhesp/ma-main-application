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

        public String PrepareGazeData(String timestamp, double leftX, double leftY, double rightX, double rightY)
        {
            return _experiment.PrepareGazeData(timestamp, leftX, leftY, rightX, rightY);
        }

        public Boolean AssignGazeToWebpage(String uniqueId, String url)
        {
            return _experiment.AssignGazeToWebpage(uniqueId, url);
        }

        public Boolean AssignGazeToWebpage(GazeModel gazeModel, String url)
        {
            return _experiment.AssignGazeToWebpage(gazeModel, url);
        }

        public GazeModel GetGazeModel(String uniqueId)
        {
            return _experiment.GetGazeModel(uniqueId);
        }

        private void ExportToXML()
        {
            String timestamp = Timestamp.GetUnixTimestamp();

            String filename = timestamp + _experiment.ExperimentName;

            new ExperimentExporter().ExportToXML(_experiment.GetBaseExperimentLocation(), filename, _experiment);
        }
    }
}
