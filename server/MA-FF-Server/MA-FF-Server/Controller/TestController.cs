using System;
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
using WebAnalyzer.Models.Base;

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
        ///  Event for updating the service stati.
        /// </summary>
        public event UpdateServiceStatiEventHandler UpdateServiceStati;

        /// <summary>
        /// Reference to the test data for current test.
        /// </summary>
        private TestModel _test;

        /// <summary>
        /// Reference to the used trackingModel;
        /// </summary>
        /// <remarks>Can be eyetracker or mousetracker.</remarks>
        private BaseTrackingModel _trackingModel;


        /// <summary>
        /// Boolean for indicating if there was a problem for the WebSocket Server.
        /// </summary>
        private Boolean _WSWarning = false;

        /// <summary>
        /// Reference to the Websocket Server.
        /// </summary>
        private WebsocketServer _wsServer;

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
        private System.Timers.Timer _saveTimer;

        /// <summary>
        /// Reference to an timer used for checking if services are running and are connected.
        /// </summary>
        private System.Timers.Timer _serviceTimer;

        /// <summary>
        /// Contains the raw data of the tracking device, without any extra informations.
        /// </summary>
        private RawTrackingData _rawData = new RawTrackingData();

        /// <summary>
        /// Constructor which creates the TestModel, prepares all Servies (Websocket and Tracking) and creates the timer.
        /// </summary>
        public TestController()
        {
            _test = new TestModel();
            InitialiseServices();
            CreateServiceTimer();
            CreateSaveTimer();
        }


        /// <summary>
        /// Destructor to ensure all services get stopped.
        /// </summary>
        ~TestController()
        {
            _serviceTimer.Stop();
            _saveTimer.Stop();
            EndServices(); 
        }

        /// <summary>
        /// Creates the Timer used for checking if services are working correctly.
        /// </summary>
        private void CreateServiceTimer()
        {
            _serviceTimer = new System.Timers.Timer();
            _serviceTimer.Enabled = true;
            _serviceTimer.Interval = Properties.Settings.Default.ServicesCheckIntervall;
            _serviceTimer.Elapsed += new ElapsedEventHandler(CheckServices);
        }

        /// <summary>
        /// Creates the Timer used when checking if data can now be saved.
        /// </summary>
        private void CreateSaveTimer()
        {
            _saveTimer = new System.Timers.Timer();
            _saveTimer.Enabled = true;
            _saveTimer.Interval = Properties.Settings.Default.TestrunSaveCheckIntervall;
            _saveTimer.Elapsed += new ElapsedEventHandler(CheckForSave);
        }

        /// <summary>
        /// Interface to access the TestModel
        /// </summary>
        public TestModel Test
        {
            get { return _test; }
        }

        /// <summary>
        /// Interface to access the collected raw data.
        /// </summary>
        public RawTrackingData RawData
        {
            get { return _rawData; }
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
                return _trackingModel.ConnectionStatus;
            }
        }

        /// <summary>
        /// Methods for Refreshing Services.
        /// </summary>
        /// <remarks>Only calls EndServices and then InitialiseServices.</remarks>
        public void RefreshServices()
        {
            EndServices();
            InitialiseServices();
        }

        /// <summary>
        /// Initialises all Websocket Server and Tracking Interface for the test.
        /// </summary>
        /// <remarks>Uses the settings to choose what to prepare and what to use for preparing.</remarks>
        public void InitialiseServices()
        {
            Logger.Log("Initialising Services");

            InitialiseWSServer();

            InitialiseTrackingComponent();
        }

        /// <summary>
        /// Initialises the WebSocket Server
        /// </summary>
        private void InitialiseWSServer()
        {
            Logger.Log("Initialising WebSocket Server");

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
        }

        /// <summary>
        /// Initialises the TrackingComponent
        /// </summary>
        private void InitialiseTrackingComponent()
        {
            Logger.Log("Initialising Tracking Component");
            try
            {
                _trackingModel = BaseTrackingModel.GetTrackingModel();

                _trackingModel.PrepareGaze += On_PrepareGazeData;

                _trackingModel.AddTrackingEvent += On_TrackingEvent;

                _trackingModel.connect();


            }
            catch (Exception e)
            {
                Logger.Log("TrackingException: " + e.Source + "-" + e.Message);

                if (_trackingModel != null)
                {
                    _trackingModel.ConnectionStatus = ExperimentObject.CONNECTION_STATUS.warning;
                }
            }

            if (_trackingModel.ConnectionStatus == ExperimentObject.CONNECTION_STATUS.connected)
            {
                CheckTrackingFrequency();
            }
        }

        /// <summary>
        /// Checks if tracking frequency and ws message interval values can work with the current settings.
        /// </summary>
        private void CheckTrackingFrequency(){
        
            double trackingInterval = _trackingModel.getTrackingFrequency();

            Logger.Log("Trackingrate: " + trackingInterval);

            int messageSentInterval = Properties.Settings.Default.WSMessageDelay;

            Logger.Log("Message Interval: " + messageSentInterval);
            Logger.Log("Tracking interval: " + trackingInterval);

            if (!Properties.Settings.Default.ForceMessageDelaySettings)
            {
                if (messageSentInterval > trackingInterval)
                {
                    // message interval to big
                    Logger.Log("Message interval bigger than tracking interval.");

                    // calculate new value
                    messageSentInterval = (int)(trackingInterval / 2);
                }
                else
                {
                    // message interval smaller
                    Logger.Log("Message interval smaller than tracking interval.");

                    if (messageSentInterval > (trackingInterval / 2))
                    {
                        Logger.Log("Message interval not small enough");
                        // calculate new value
                        messageSentInterval = (int)(trackingInterval / 2);
                    }
                }

                Logger.Log("Updated Message Interval: " + messageSentInterval);
            }

            _test.MessageSentInterval = messageSentInterval;
            _test.TrackingInterval = trackingInterval;

        }

        /// <summary>
        /// Stops all Websocket Server and Tracking Interface.
        /// </summary>
        private void EndServices()
        {
            if (_running)
            {
                StopTest();
            }

            if (_wsServer != null)
            {
                _wsServer.stop();

                _wsServer = null;
            }

            _trackingModel.PrepareGaze -= On_PrepareGazeData;

            _trackingModel.AddTrackingEvent -= On_TrackingEvent;

            _trackingModel = null;
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

            if (_trackingModel != null)
            {
                _trackingModel.startTracking();
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
            if (_trackingModel != null)
            {
                _trackingModel.stopTracking();
            }

            //start timer to check if saving is possible
            _saveTimer.Start();
        }

        /// <summary>
        /// Prepares the GazeData and requests the data over the websocket server.
        /// </summary>
        /// <param name="source">Source from which the event gets triggered.</param>
        /// <param name="e">Data about the gaze.</param>
        private void On_PrepareGazeData(object source, PrepareGazeDataEvent e)
        {

            String uniqueId = PrepareGazeData(e.LeftEye, e.RightEye);

            if (e.LeftEye.X == e.RightEye.X && e.LeftEye.Y == e.RightEye.Y)
            {
                _wsServer.RequestData(uniqueId, e.LeftEye.X, e.LeftEye.Y);
            }
            else
            {
                _wsServer.RequestData(uniqueId, e.LeftEye.X, e.LeftEye.Y, e.RightEye.X, e.RightEye.Y);
            }
        }

        /// <summary>
        /// Prepares the GazeData with the TestModel.
        /// </summary>
        /// <param name="leftEye">Data about the left Eye</param>
        /// <param name="rightEye">Data about the right Eye</param>
        public String PrepareGazeData(BaseTrackingData leftEye, BaseTrackingData rightEye)
        {
            if (this.Running)
            {
                _rawData.AddRawGaze(leftEye, rightEye);
                return _test.PrepareGazeData(leftEye, rightEye);
            }
            return null;
        }

        /// <summary>
        /// Assigns a gaze to the webpage given webpage.
        /// </summary>
        /// <param name="uniqueId">Uniqueid of the gaze to assign</param>
        /// <param name="url">Url of webpage to assign the gaze to</param>
        /// <param name="connectionUID">The Uniqueid of the connection to correctly identify the visited webpage</param>
        /// <returns></returns>
        public Boolean AssignGazeToWebpage(String uniqueId, String url, String connectionUID)
        {
            _dataCollected = true;
            return _test.AssignGazeToWebpage(uniqueId, url, connectionUID);
        }

        /// <summary>
        /// Assigns a gaze to the webpage given webpage.
        /// </summary>
        /// <param name="gazeModel">Gaze to assign to webpage</param>
        /// <param name="url">Url of webpage to assign the gaze to</param>
        /// <param name="connectionUID">The Uniqueid of the connection to correctly identify the visited webpage</param>
        /// <returns></returns>
        public Boolean AssignGazeToWebpage(GazeModel gazeModel, String url, String connectionUID)
        {
            _dataCollected = true;
            return _test.AssignGazeToWebpage(gazeModel, url, connectionUID);
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
                _saveTimer.Stop();
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

        /// <summary>
        /// Callback for Update WS Change Webpage Event
        /// </summary>
        /// <param name="source">Object from which the event gets triggered.</param>
        /// <param name="e">Data about the webpage</param>
        public void On_AddWebpage(object source, AddWebpageEvent e)
        {
            Logger.Log("Add Webpage: " + e.URL);
            if (e.URL != "")
            {
                WebpageModel pageModel = new WebpageModel(e.URL, e.ConnectionUID, e.Timestamp);

                pageModel.WindowHeight = e.WindowHeight;
                pageModel.WindowWidth = e.WindowWidth;

                _test.AddWebpage(pageModel);
            }
        }

        /// <summary>
        /// Event which gets triggered from the tracking model when an event happend.
        /// </summary>
        /// <param name="source">Object from which the event gets triggered.</param>
        /// <param name="e">Data about the event which happened.</param>
        /// <remarks>Used for collecting data about fixations from the EyeTracker.</remarks>
        public void On_TrackingEvent(object source, AddTrackingEvent e)
        {
            _rawData.AddEvent(e.Event);
        }

        /// <summary>
        /// Event which gets triggered when an event from the browser gets received.
        /// </summary>
        /// <param name="source">Object from which the event gets triggered.</param>
        /// <param name="e">Data about the event which happened.</param>
        /// <remarks>Used for collecting the events sent from the browser.</remarks>
        public void On_AddBrowserEvent(object source, AddBrowserEvent e)
        {
            if (_test != null)
            {
                _test.AssignEventToWebpage(e.EventModel, e.EventModel.URL, e.ConnectionUID);
            }
        }

        /// <summary>
        /// Event which gets triggered to add data to the current testrun
        /// </summary>
        /// <param name="source">Object from which the event gets triggered.</param>
        /// <param name="e">Data about the testrun.</param>
        public void On_AddTestrunData(object source, AddTestrunDataEvent e)
        {
            if (_test != null)
            {
                _test.Label = e.Label;
                _test.Protocol = e.Protocol;

                // save testrun if data was already saved
                if(_dataCollected && !_saveTimer.Enabled){
                    SaveTestrun(this, new SaveTestrunEvent());
                }
            }
        }

        /// <summary>
        /// Checks if all needed services are running and okay. If not tries to start them/ connect.
        /// </summary>
        /// <param name="sender">Object from which the method is called</param>
        /// <param name="e">Event</param>
        public void CheckServices(object sender, ElapsedEventArgs e)
        {
            //check if services are running
            bool update = false;

            //websocket server
            if (_WSWarning)
            {
                InitialiseWSServer();
                update = true;
            }

            //tracking component...
            if (_trackingModel != null && 
                _trackingModel.ConnectionStatus != ExperimentObject.CONNECTION_STATUS.connected)
            {
                InitialiseTrackingComponent();
                update = true;
            }

            if (update)
            {
                //trigger update event
                UpdateServiceStati(this, new UpdateServiceStatiEvent());
            }
        }
    }
}
