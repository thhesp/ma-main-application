using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Controller;

namespace WebAnalyzer.Experiment
{
    class TestController
    {

        private TestModel _test;

        private Boolean _running;

        public TestController()
        {
            _test = new TestModel();
        }

        public TestModel Test
        {
            get { return _test; }
            set { _test = value; }
        }

        public Boolean Running
        {
            get { return _running; }
            set { _running = value; }
        }

        public void StartTest()
        {
            Logger.Log("Start Experiment");
            _running = true;
            // start server etc.
            // set listeners
        }

        public void StopTest()
        {
            Logger.Log("Stop Experiment");
            _running = false;
            // stop server etc.

            // export raw data

            //this.ExportToXML();
        }

        public String PrepareGazeData(String timestamp, double x, double y)
        {
            if (this.Running)
            {
                return _test.PrepareGazeData(timestamp, x, y);
            }
            return null;
        }

        public String PrepareGazeData(String timestamp, double leftX, double leftY, double rightX, double rightY)
        {
            if (this.Running)
            {
                return _test.PrepareGazeData(timestamp, leftX, leftY, rightX, rightY);
            }
            return null;
        }

        public Boolean AssignGazeToWebpage(String uniqueId, String url)
        {
            return _test.AssignGazeToWebpage(uniqueId, url);
        }

        public Boolean AssignGazeToWebpage(GazeModel gazeModel, String url)
        {
            return _test.AssignGazeToWebpage(gazeModel, url);
        }

        public Boolean DisposeOfGazeData(String uniqueId)
        {
            return _test.DisposeOfGazeData(uniqueId);
        }

        public Boolean DisposeOfGazeData(GazeModel gazeModel)
        {
            return _test.DisposeOfGazeData(gazeModel);
        }



        public GazeModel GetGazeModel(String uniqueId)
        {
            return _test.GetGazeModel(uniqueId);
        }
    }
}
