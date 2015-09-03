﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using WebAnalyzer.Util;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Controller;
using WebAnalyzer.Server;
using WebAnalyzer.Test.Communication;
using WebAnalyzer.EyeTracking;
using WebAnalyzer.Events;
using WebAnalyzer.UI.InteractionObjects;

namespace WebAnalyzer.Controller
{
    /// <summary>
    /// Class used for controlling an test run.
    /// </summary>
    class TestController
    {

        /// <summary>
        ///  Event for saving the testrun.
        /// </summary>
        public event SaveTestrunEventHandler SaveTestrun;

        /// <summary>
        ///  Event for updating the connection count.
        /// </summary>
        public event UpdateWSConnectionCountEventHandler UpdateWSConnectionCount;

        /// <summary>
        /// Reference to the test data for current test.
        /// </summary>
        private TestModel _test;

        /// <summary>
        /// Reference to the EyeTracker which is used for getting data.
        /// </summary>
        /// <remarks>Can be null if MouseTracking is used.</remarks>
        private EyeTrackingModel _etModel;

        /// <summary>
        /// Boolean for indicating if there was a problem with the EyeTracker.
        /// </summary>
        private Boolean _ETWarning = false;
        /// <summary>
        /// Boolean for indicating if there was a problem for the WebSocket Server.
        /// </summary>
        private Boolean _WSWarning = false;

        /// <summary>
        /// Reference to the Websocket Server.
        /// </summary>
        private WebsocketServer _wsServer;

        /// <summary>
        /// Reference to the MouseModel used as an alternative to the EyeTracking.
        /// </summary>
        private MouseModel _debugModel;

        /// <summary>
        /// Boolean for indicating if the test is currently running.
        /// </summary>
        private Boolean _running;

        /// <summary>
        /// Boolean for indicating if any data was collected.
        /// </summary>
        private Boolean _dataCollected = false;

        /// <summary>
        /// Reference to an timer used for checking if all communication is done and the data can be saved.
        /// </summary>
        private System.Timers.Timer saveTimer;

        /// <summary>
        /// Constructor which creates the TestModel, prepares all Servies (Websocket and Tracking) and creates the timer.
        /// </summary>
        public TestController()
        {
            _test = new TestModel();
            PrepareServices();
            CreateSaveTimer();
        }


        /// <summary>
        /// Destructor to ensure all services get stopped.
        /// </summary>
        ~TestController()
        {
            EndServices();
        }


        /// <summary>
        /// Creates the Timer used when checking if data can now be saved.
        /// </summary>
        private void CreateSaveTimer()
        {
            saveTimer = new System.Timers.Timer();
            saveTimer.Enabled = true;
            saveTimer.Interval = Properties.Settings.Default.TestrunSaveCheckIntervall;
            saveTimer.Elapsed += new ElapsedEventHandler(CheckForSave);
        }

        /// <summary>
        /// Interface to access the TestModel
        /// </summary>
        public TestModel Test
        {
            get { return _test; }
        }

        /// <summary>
        /// Interface to check if the Test ir Running.
        /// </summary>
        public Boolean Running
        {
            get { return _running; }
        }

        /// <summary>
        /// Interface to check if data was collected.
        /// </summary>
        public Boolean DataCollected
        {
            get { return _dataCollected; }
        }

        /// <summary>
        /// Interface to check the ConnectionStatus of the Websocket Server.
        /// </summary>
        /// <remarks>
        /// Used for the indicator of the main UI.
        /// </remarks>
        public ExperimentObject.CONNECTION_STATUS WSStatus
        {

            get
            {
                if (_WSWarning)
                {
                    return ExperimentObject.CONNECTION_STATUS.warning;
                }

                if (_wsServer != null)
                {

                    return ExperimentObject.CONNECTION_STATUS.connected;
                }

                return ExperimentObject.CONNECTION_STATUS.disconnected;
            }
        }

        /// <summary>
        /// Interface to check the ConnectionStatus of the Tracker (EyeTracking or Mouse).
        /// </summary>
        /// <remarks>
        /// Used for the indicator of the main UI.
        /// </remarks>
        public ExperimentObject.CONNECTION_STATUS TrackingStatus
        {
            
            get
            {
                return GetTrackingStatus();
            }
        }

        /// <summary>
        /// Intern method to extract the Tracking Status.
        /// </summary>
        private ExperimentObject.CONNECTION_STATUS GetTrackingStatus()
        {
            if (Properties.Settings.Default.UseMouseTracking)
            {
                Logger.Log("Get MouseTrackingStatus....");
                if (_debugModel != null)
                {
                    return ExperimentObject.CONNECTION_STATUS.connected;
                }

                return ExperimentObject.CONNECTION_STATUS.disconnected;
            }
            else
            {
                Logger.Log("Get EyeTrackingStatus....");
                if (_ETWarning)
                {
                    return ExperimentObject.CONNECTION_STATUS.warning;
                }


                if (_etModel != null
                    && _etModel.isConnected() == 0) // RET_SUCCESS
                {
                    return ExperimentObject.CONNECTION_STATUS.connected;
                }

                return ExperimentObject.CONNECTION_STATUS.disconnected;
            }
        }

        /// <summary>
        /// Methods for Refreshing Services.
        /// </summary>
        /// <remarks>Only calls EndServices and then PrepareServices.</remarks>
        public void RefreshServices()
        {
            EndServices();
            PrepareServices();
        }

        /// <summary>
        /// Prepares all Websocket Server and Tracking Interface for the test.
        /// </summary>
        /// <remarks>Uses the settings to choose what to prepare and what to use for preparing.</remarks>
        public void PrepareServices()
        {
            Logger.Log("Prepare Services");

            try
            {

                _wsServer = new WebsocketServer(this);

                _wsServer.start(Properties.Settings.Default.WebsocketPort);

            }
            catch (Exception e)
            {
                Logger.Log("WSException: " + e.Source + "-" + e.Message);

                _WSWarning = true;
            }

            if (Properties.Settings.Default.UseMouseTracking)
            {
                _debugModel = new MouseModel();

                _debugModel.PrepareGaze += On_PrepareGazeData;

            }
            else
            {

                try
                {
                    _etModel = new EyeTrackingModel();

                    _etModel.PrepareGaze += On_PrepareGazeData;


                    if (Properties.Settings.Default.ETConnectLocal)
                    {
                        _ETWarning = !_etModel.connectLocal();
                    }
                    else
                    {
                        _ETWarning = !_etModel.connect(Properties.Settings.Default.ETSentIP, Properties.Settings.Default.ETSentPort, Properties.Settings.Default.ETReceiveIP, Properties.Settings.Default.ETReceivePort);
                    }

                    Logger.Log("ET_Warning????" + _ETWarning.ToString());
                }
                catch (Exception e)
                {
                    Logger.Log("EyeTrackingException: " + e.Source + "-" + e.Message);

                    _ETWarning = true;
                }

            }
        }

        /// <summary>
        /// Stops all Websocket Server and Tracking Interface.
        /// </summary>
        /// <remarks>Uses the settings to choose what to prepare and what to use for preparing.</remarks>
        private void EndServices()
        {
            if (_running)
            {
                StopTest();
            }

            if (_wsServer != null)
            {
                _wsServer.stop();
            }
            

            if (_debugModel != null)
            {
                _debugModel.PrepareGaze -= On_PrepareGazeData;
                _debugModel = null;
            }
            if (_etModel != null)
            {
                _etModel.disconnect();

                _etModel.PrepareGaze -= On_PrepareGazeData;

                _etModel = null;
            }
        }

        /// <summary>
        /// Starts the test.
        /// </summary>
        public void StartTest()
        {
            Logger.Log("Start Experiment");
            _running = true;
            _dataCollected = false;
            _test.Started = DateTime.Now.ToString();
            // set listeners
            if (Properties.Settings.Default.UseMouseTracking)
            {
                _debugModel.startTracking();
            }
            else
            {
                _etModel.startTracking();
            }
        }

        /// <summary>
        /// Stops the test.
        /// </summary>
        public void StopTest()
        {
            Logger.Log("Stop Experiment");
            _test.Stopped = DateTime.Now.ToString();
            _running = false;
            // stop server etc.

            if (Properties.Settings.Default.UseMouseTracking)
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

            //start timer to check if saving is possible
            saveTimer.Start();
        }

        /// <summary>
        /// Prepares the GazeData and requests the data over the websocket server.
        /// </summary>
        /// <param name="source">Source from which the event gets triggered.</param>
        /// <param name="e">Data about the gaze.</param>
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

        /// <summary>
        /// Prepares the GazeData with the TestModel.
        /// </summary>
        /// <param name="timestamp">Timestamp on which the gaze was requested</param>
        /// <param name="x">X coordinates of the gaze</param>
        /// <param name="y">Y coordinates of the gaze</param>
        public String PrepareGazeData(String timestamp, double x, double y)
        {
            if (this.Running)
            {
                return _test.PrepareGazeData(timestamp, x, y);
            }
            return null;
        }

        /// <summary>
        /// Prepares the GazeData with the TestModel.
        /// </summary>
        /// <param name="timestamp">Timestamp on which the gaze was requested</param>
        /// <param name="leftX">X coordinates of the left eye of the gaze</param>
        /// <param name="leftY">Y coordinates of the left eye of the gaze</param>
        /// <param name="rightX">X coordinates of the right eye of the gaze</param>
        /// <param name="rightY">Y coordinates of the right eye of the gaze</param>
        public String PrepareGazeData(String timestamp, double leftX, double leftY, double rightX, double rightY)
        {
            if (this.Running)
            {
                return _test.PrepareGazeData(timestamp, leftX, leftY, rightX, rightY);
            }
            return null;
        }

        /// <summary>
        /// Assigns a gaze to the webpage given webpage.
        /// </summary>
        /// <param name="uniqueId">Uniqueid of the gaze to assign</param>
        /// <param name="url">Url of webpage to assign the gaze to</param>
        /// <returns></returns>
        public Boolean AssignGazeToWebpage(String uniqueId, String url)
        {
            _dataCollected = true;
            return _test.AssignGazeToWebpage(uniqueId, url);
        }

        /// <summary>
        /// Assigns a gaze to the webpage given webpage.
        /// </summary>
        /// <param name="gazeModel">Gaze to assign to webpage</param>
        /// <param name="url">Url of webpage to assign the gaze to</param>
        /// <returns></returns>
        public Boolean AssignGazeToWebpage(GazeModel gazeModel, String url)
        {
            _dataCollected = true;
            return _test.AssignGazeToWebpage(gazeModel, url);
        }

        /// <summary>
        /// Dispose of gaze data (since its an error)
        /// </summary>
        /// <param name="uniqueId">Uniqueid of the gaze to dispose</param>
        /// <returns></returns>
        public Boolean DisposeOfGazeData(String uniqueId)
        {
            return _test.DisposeOfGazeData(uniqueId);
        }

        /// <summary>
        /// Dispose of gaze data (since its an error)
        /// </summary>
        /// <param name="gazeModel">Gaze to dispose</param>
        /// <returns></returns>
        public Boolean DisposeOfGazeData(GazeModel gazeModel)
        {
            return _test.DisposeOfGazeData(gazeModel);
        }

        /// <summary>
        /// Get the GazeModel to an uniqueid
        /// </summary>
        /// <param name="uniqueId">UniquieId of the gaze which is searched</param>
        /// <returns></returns>
        public GazeModel GetGazeModel(String uniqueId)
        {
            return _test.GetGazeModel(uniqueId);
        }

        /// <summary>
        /// Check if all data is there and save if it is.
        /// </summary>
        /// <param name="sender">Object from which the method is called</param>
        /// <param name="e">Event</param>
        public void CheckForSave(object sender, ElapsedEventArgs e)
        {
            if (_test.CheckForSave())
            {
                saveTimer.Stop();
                SaveTestrun(this, new SaveTestrunEvent());
            }
        }

        /// <summary>
        /// Callback for Message Sent Event.
        /// </summary>
        /// <param name="sender">Object from which the event gets triggered.</param>
        /// <param name="e">Data which message was sent and when.</param>
        public void On_MessageSent(object sender, MessageSentEvent e)
        {
            _test.MessageSent(e.UID, e.SentTimestamp);
        }

        /// <summary>
        /// Callback for Update WS Connection Count Event
        /// </summary>
        /// <param name="sender">Object from which the event gets triggered.</param>
        /// <param name="e">Data about the number of connections</param>
        public void On_UpdateConnectionCount(object sender, UpdateWSConnectionCountEvent e)
        {
            UpdateWSConnectionCount(sender, e);   
        }
    }
}
