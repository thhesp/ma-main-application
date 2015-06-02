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

        private Boolean _running;

        private ExperimentController()
        {
            
        }

        public void CreateExperiment(String name)
        {
            _experiment = new ExperimentModel(name);
        }

        public Boolean Running
        {
            get { return _running; }
            set { _running = value; }
        }

        public void StartExperiment()
        {
            Logger.Log("Start Experiment");
            _running = true;
            // start server etc.
            // set listeners
        }

        public void StopExperiment()
        {
            Logger.Log("Stop Experiment");
            _running = false;
            // stop server etc.

            // export raw data

            this.ExportToXML();
        }

        public String PrepareGazeData(String timestamp, double x, double y)
        {
            if (this.Running)
            {
                return _experiment.PrepareGazeData(timestamp, x, y);
            }
            return null;
        }

        public String PrepareGazeData(String timestamp, double leftX, double leftY, double rightX, double rightY)
        {
            if (this.Running)
            {
                return _experiment.PrepareGazeData(timestamp, leftX, leftY, rightX, rightY);
            }
            return null;
        }

        public Boolean AssignGazeToWebpage(String uniqueId, String url)
        {
            return _experiment.AssignGazeToWebpage(uniqueId, url);
        }

        public Boolean AssignGazeToWebpage(GazeModel gazeModel, String url)
        {
            return _experiment.AssignGazeToWebpage(gazeModel, url);
        }

        public Boolean DisposeOfGazeData(String uniqueId)
        {
            return _experiment.DisposeOfGazeData(uniqueId);
        }

        public Boolean DisposeOfGazeData(GazeModel gazeModel)
        {
            return _experiment.DisposeOfGazeData(gazeModel);
        }



        public GazeModel GetGazeModel(String uniqueId)
        {
            return _experiment.GetGazeModel(uniqueId);
        }

        private void ExportToXML()
        {
            if (_experiment.Exportable())
            {
                ExperimentExporter.ExportToXML(_experiment);
            }
        }
    }
}
