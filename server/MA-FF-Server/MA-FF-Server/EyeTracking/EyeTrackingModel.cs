using System;
using System.Text;

using WebAnalyzer.Server;
using WebAnalyzer.Util;

using WebAnalyzer.Events;
using WebAnalyzer.Models.Base;

namespace WebAnalyzer.EyeTracking
{

    /// <summary>
    /// This model contains all functionality necessary for using the ET controller for tests.
    /// </summary>
    public class EyeTrackingModel
    {
        /// <summary>
        /// Event which is used for sending data to the test controller.
        /// </summary>
        public event PrepareGazeEventHandler PrepareGaze;

        /// <summary>
        /// Event which is used for sending events to the test controller.
        /// </summary>
        public event AddTrackingEventHandler AddTrackingEvent;

        /// <summary>
        /// Object of the EyeTracking Controller. Used for interacting with the EyeTracker.
        /// </summary>
        private EyeTrackingController ETDevice;


        // callback routine declaration
        private delegate void CalibrationCallback(EyeTrackingController.CalibrationPointStruct calibrationPointData);
        private delegate void GetSampleCallback(EyeTrackingController.SampleStruct sampleData);
        private delegate void GetEventCallback(EyeTrackingController.EventStruct eventData);

        // callback function instances
        private CalibrationCallback m_CalibrationCallback;
        private GetSampleCallback m_SampleCallback;
        private GetEventCallback m_EventCallback;

        /// <summary>
        /// Constructor which sets the ETDevice Object and some Callbacks.
        /// </summary>
        public EyeTrackingModel()
        {
            ETDevice = new EyeTrackingController();

            m_CalibrationCallback = new CalibrationCallback(CalibrationCallbackFunction);
            m_SampleCallback = new GetSampleCallback(GetSampleCallbackFunction);
            m_EventCallback = new GetEventCallback(GetEventCallbackFunction);
        }

        /// <summary>
        /// Method to connect to an EyeTracking Server.
        /// </summary>
        /// <param name="sendip">The IP of the device from which the EyeTracker sents data.</param>
        /// <param name="sendport">The Port over which the EyeTracker sents data.</param>
        /// <param name="receiveip">The IP of the device from which the EyeTracker receives data.</param>
        /// <param name="receiveport">The Port over which the EyeTracker receives data.</param>
        public Boolean connect(String sendip, int sendport, String receiveip, int receiveport)
        {
            Logger.Log("Connect with "+sendip+":"+sendport+" to "+receiveip+":"+receiveport);

            try
            {

                int ret = ETDevice.iV_Connect(new StringBuilder(sendip), Convert.ToInt32(sendport), new StringBuilder(receiveip), Convert.ToInt32(receiveport));

                if (ret == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception exc)
            {
                Logger.Log("Exception: " + exc.Message);
                // log error?
                //logger1.Text = "Exception during iV_Connect: " + exc.Message;
                return false;
            }
        }


        /// <summary>
        /// Method to connect to an EyeTracking Server locally.
        /// </summary>
        public Boolean connectLocal()
        {
            Logger.Log("connectLocal");

            /*int errorID = 0;
            try
            {
                ETDevice.iV_SetCalibrationCallback(m_CalibrationCallback);
                ETDevice.iV_SetSampleCallback(m_SampleCallback);
                ETDevice.iV_SetEventCallback(m_EventCallback);

                errorID = ETDevice.iV_ConnectLocal();
            }
            catch (System.Exception e)
            {
                Logger.Log(e.Message);
            }

            Logger.Log("ErrorID: " + errorID);*/



            return connect("127.0.0.1", 4444, "127.0.0.1", 5555);
        }

        /// <summary>
        /// Disconnet from the Eye Tracking Server
        /// (Important Note: You must disconnect your Application from the server to avoid critical crashed and problems with the Portsettings)
        /// </summary>
        public int disconnect()
        {
            Logger.Log("disconnect");

            int errorID = 0;
            try
            {
                errorID = ETDevice.iV_Disconnect();
            }
            catch (System.Exception e)
            {
                Logger.Log(e.Message);
            }

            return errorID;
        }

        /// <summary>
        /// Starts the Tracking by setting the necessary callbacks.
        /// </summary>
        public void startTracking()
        {
            if (ETDevice != null)
            {
                //ETDevice.iV_SetCalibrationCallback(m_CalibrationCallback);
                ETDevice.iV_SetSampleCallback(m_SampleCallback);
                //ETDevice.iV_SetEventCallback(m_EventCallback);
            }
        }

        /// <summary>
        /// Stops the Tracking by removeing the callbacks.
        /// </summary>
        public void stopTracking()
        {
            if (ETDevice != null)
            {
                //ETDevice.iV_SetCalibrationCallback(null);
                ETDevice.iV_SetSampleCallback(null);
                //ETDevice.iV_SetEventCallback(null);
            }
        }

        /// <summary>
        /// Checks if the EyeTracking Device is connected.
        /// </summary>
        public int isConnected()
        {
            if (ETDevice != null)
            {
                return ETDevice.iV_IsConnected();
            }

            return 101;
        }

        #region CallbackFunctions

        /// <summary>
        /// Callback function for sample Data.
        /// </summary>
        /// <remarks>
        /// The data is for one tracking point.
        /// </remarks>
        void GetSampleCallbackFunction(EyeTrackingController.SampleStruct sampleData)
        {
           String data = "Data from SampleCallback - timestamp: " + sampleData.timestamp.ToString() +
                " - GazeRX: " + sampleData.rightEye.gazeX.ToString() +
                " - GazeRY: " + sampleData.rightEye.gazeY.ToString() +
                " - GazeLX: " + sampleData.leftEye.gazeX.ToString() +
                " - GazeLY: " + sampleData.leftEye.gazeY.ToString() +
                " - DiamRX: " + sampleData.rightEye.diam.ToString() +
                " - DiamLX: " + sampleData.leftEye.diam.ToString() +
                " - DistanceR: " + sampleData.rightEye.eyePositionZ.ToString() +
                " - DistanceL: " + sampleData.leftEye.eyePositionZ.ToString();

           PrepareGaze(this, new PrepareGazeDataEvent(sampleData.timestamp.ToString(), sampleData.leftEye.gazeX, sampleData.leftEye.gazeY, sampleData.rightEye.gazeX, sampleData.rightEye.gazeY));
        }

        /// <summary>
        /// Callback function for event Data.
        /// </summary>
        /// <remarks>
        /// The data is for mainly fixations.
        /// </remarks>
        void GetEventCallbackFunction(EyeTrackingController.EventStruct eventData)
        {
            String data = "Data from EventCallback - eye: " + eventData.eye.ToString() +
                " Event: " + eventData.eventType + " startTime: " + eventData.startTime.ToString() +
                " End:" + eventData.endTime.ToString() + " duration:" + eventData.duration.ToString() +
                " PosX:" + eventData.positionX.ToString() +
                " PosY:" + eventData.positionY.ToString();

            RawTrackingEvent rawEvent = new RawTrackingEvent();

            rawEvent.Eye = eventData.eye.ToString();

            rawEvent.EventType = eventData.eventType.ToString();

            rawEvent.StartTime = eventData.startTime.ToString();

            rawEvent.EndTime = eventData.endTime.ToString();

            rawEvent.Duration = eventData.duration.ToString();

            rawEvent.X = eventData.positionX;

            rawEvent.Y = eventData.positionY;

            AddTrackingEvent(this, new AddTrackingEvent(rawEvent));
        }

        /// <summary>
        /// Callback function for calibration Data.
        /// </summary>
        void CalibrationCallbackFunction(EyeTrackingController.CalibrationPointStruct calibrationPointData)
        {
            String data = "Data from CalibrationCallback - Number:" + calibrationPointData.number + " PosX:" + calibrationPointData.positionx + " PosY:" + calibrationPointData.positiony;

            //Logger.Log(data);
        }

        #endregion
    }
}