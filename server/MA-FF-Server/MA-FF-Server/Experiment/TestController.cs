﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Controller;
using WebAnalyzer.Server;
using WebAnalyzer.Test.Communication;
using WebAnalyzer.EyeTracking;
using WebAnalyzer.Events;
using WebAnalyzer.UI.InteractionObjects;

namespace WebAnalyzer.Experiment
{
    class TestController
    {

        private TestModel _test;
        private EyeTrackingModel _etModel;

        private Boolean _ETWarning = false;

        private WebsocketServer _wsServer;

        private MouseModel _debugModel;

        private Boolean _running;

        public TestController()
        {
            _test = new TestModel();
            PrepareServices();
        }

        ~TestController()
        {
            EndServices();
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

        public ExperimentObject.CONNECTION_STATUS WSStatus
        {

            get
            {
                if (_wsServer != null)
                {
                    return ExperimentObject.CONNECTION_STATUS.connected;
                }

                return ExperimentObject.CONNECTION_STATUS.disconnected;
            }
        }

        public ExperimentObject.CONNECTION_STATUS TrackingStatus
        {

            get
            {

                if (Boolean.Parse(Properties.Settings.Default.UseMouseTracking))
                {
                    if (_debugModel != null)
                    {
                        return ExperimentObject.CONNECTION_STATUS.connected;
                    }

                    return ExperimentObject.CONNECTION_STATUS.disconnected;
                }
                else
                {
                    if (_etModel != null)
                    {
                        if (_ETWarning)
                        {
                            return ExperimentObject.CONNECTION_STATUS.warning;
                        }

                        if (_etModel.isConnected() == 0) // RET_SUCCESS
                        {
                            return ExperimentObject.CONNECTION_STATUS.connected;
                        }
                    }

                    return ExperimentObject.CONNECTION_STATUS.disconnected;
                }
            }
        }

        public void RefreshServices()
        {
            EndServices();
            PrepareServices();
        }

        public void PrepareServices()
        {
            Logger.Log("Prepare Services");
            _wsServer = new WebsocketServer(this);

            _wsServer.start(int.Parse(Properties.Settings.Default.WebsocketPort));

            if (Boolean.Parse(Properties.Settings.Default.UseMouseTracking))
            {
                _debugModel = new MouseModel();

                _debugModel.PrepareGaze += On_PrepareGazeData;

            }
            else
            {
                _etModel = new EyeTrackingModel();

                _etModel.PrepareGaze += On_PrepareGazeData;


                if (Boolean.Parse(Properties.Settings.Default.ETConnectLocal))
                {
                    _ETWarning = _etModel.connectLocal();
                }
                else
                {
                    _ETWarning = _etModel.connect(Properties.Settings.Default.ETSentIP, Properties.Settings.Default.ETSentPort, Properties.Settings.Default.ETReceiveIP, Properties.Settings.Default.ETReceivePort);
                }
            }
        }

        private void EndServices()
        {
            if (_running)
            {
                StopTest();
            }
            
            _wsServer.stop();

            if (_debugModel != null)
            {
                _debugModel.PrepareGaze -= On_PrepareGazeData;
                _debugModel = null;
            }
            if(_etModel != null)
            {
                _etModel.disconnect();

                _etModel.PrepareGaze -= On_PrepareGazeData;

                _etModel = null;
            }
        }

        public void StartTest()
        {
            Logger.Log("Start Experiment");
            _running = true;
            // set listeners
            if (Boolean.Parse(Properties.Settings.Default.UseMouseTracking))
            {
                _debugModel.startTracking();
            }
            else{
                _etModel.startTracking();
            }
        }

        public void StopTest()
        {
            Logger.Log("Stop Experiment");
            _running = false;
            // stop server etc.

            if (Boolean.Parse(Properties.Settings.Default.UseMouseTracking))
            {
                if (_debugModel != null)
                {
                    _debugModel.stopTracking();
                }
            }
            else
            {
                if (_etModel != null)
                {
                    _etModel.stopTracking();
                }
            }
        }

        private void On_PrepareGazeData(object source, PrepareGazeDataEvent e)
        {

            String uniqueId = PrepareGazeData(e.GazeTimestamp, e.LeftX, e.LeftY, e.RightX, e.RightY);

            if (e.LeftX == e.RightX && e.LeftY == e.RightY)
            {
                _wsServer.RequestData(uniqueId, e.LeftX, e.LeftY);
            }
            else
            {
                _wsServer.RequestData(uniqueId, e.LeftX, e.LeftY, e.RightX, e.RightY);
            }
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
