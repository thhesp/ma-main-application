using System;
using System.Text;

using WebAnalyzer.Server;
using WebAnalyzer.Util;

using WebAnalyzer.Events;
using WebAnalyzer.Models.Base;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.UI.InteractionObjects;

namespace WebAnalyzer.EyeTracking
{

    /// <summary>
    /// This model contains all functionality necessary for using the ET controller for tests.
    /// </summary>
    public class EyeTrackingModel : BaseTrackingModel
    {

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

        public override ExperimentObject.CONNECTION_STATUS connect()
        {
            Boolean sucess = false;

            _connectionStatus = ExperimentObject.CONNECTION_STATUS.disconnected;

            if (Properties.Settings.Default.ETConnectLocal)
            {
                sucess = connectLocal();
            }
            else
            {
                sucess = connect(Properties.Settings.Default.ETSentIP, Properties.Settings.Default.ETSentPort, Properties.Settings.Default.ETReceiveIP, Properties.Settings.Default.ETReceivePort);
            }

            if (sucess)
            {
                _connectionStatus = ExperimentObject.CONNECTION_STATUS.connected;
            }

            return _connectionStatus;
        }

        /// <summary>
        /// Method to connect to an EyeTracking Server.
        /// </summary>
        /// <param name="sendip">The IP of the device from which the EyeTracker sents data.</param>
        /// <param name="sendport">The Port over which the EyeTracker sents data.</param>
        /// <param name="receiveip">The IP of the device from which the EyeTracker receives data.</param>
        /// <param name="receiveport">The Port over which the EyeTracker receives data.</param>
        private Boolean connect(String sendip, int sendport, String receiveip, int receiveport)
        {
            Logger.Log("Connect with "+sendip+":"+sendport+" to "+receiveip+":"+receiveport);

            try
            {

                int ret = ETDevice.iV_Connect(new StringBuilder(sendip), Convert.ToInt32(sendport), new StringBuilder(receiveip), Convert.ToInt32(receiveport));

                if (ret == 1)
                {
                    return true;
                }

            }
            catch (Exception exc)
            {
                Logger.Log("Exception: " + exc.Message);
                _connectionStatus = ExperimentObject.CONNECTION_STATUS.warning;
            }

            return false;
        }


        /// <summary>
        /// Method to connect to an EyeTracking Server locally.
        /// </summary>
        private Boolean connectLocal()
        {
            Logger.Log("Connect local");

            try
            {

                int ret = ETDevice.iV_ConnectLocal();

                if (ret == 1)
                {
                    return true;
                }

            }
            catch (Exception exc)
            {
                Logger.Log("Exception: " + exc.Message);
                _connectionStatus = ExperimentObject.CONNECTION_STATUS.warning;
            }

            return false;
        }

        public override ExperimentObject.CONNECTION_STATUS disconnect()
        {
            Boolean sucess = internDisconnect();

            if (sucess)
            {
                _connectionStatus = ExperimentObject.CONNECTION_STATUS.disconnected;
            }

            return _connectionStatus;
        }

        /// <summary>
        /// Disconnet from the Eye Tracking Server
        /// (Important Note: You must disconnect your Application from the server to avoid critical crashed and problems with the Portsettings)
        /// </summary>
        private Boolean internDisconnect()
        {
            Logger.Log("disconnect");

            int errorID = 0;
            try
            {
                errorID = ETDevice.iV_Disconnect();

                if (errorID == 1)
                {
                    return true;
                }
            }
            catch (System.Exception e)
            {
                Logger.Log("Exception: " + e.Message);

                _connectionStatus = ExperimentObject.CONNECTION_STATUS.warning;
            }

            return false;
        }

        /// <summary>
        /// Starts the Tracking by setting the necessary callbacks.
        /// </summary>
        public override void startTracking()
        {
            if (ETDevice != null)
            {
                //ETDevice.iV_SetCalibrationCallback(m_CalibrationCallback);
                ETDevice.iV_SetSampleCallback(m_SampleCallback);
                ETDevice.iV_SetEventCallback(m_EventCallback);
            }
        }

        /// <summary>
        /// Stops the Tracking by removeing the callbacks.
        /// </summary>
        public override void stopTracking()
        {
            if (ETDevice != null)
            {
                //ETDevice.iV_SetCalibrationCallback(null);
                ETDevice.iV_SetSampleCallback(null);
                ETDevice.iV_SetEventCallback(null);
            }
        }

        /// <summary>
        /// Checks if the EyeTracking Device is connected.
        /// </summary>
        private int isConnected()
        {
            if (ETDevice != null)
            {
                return ETDevice.iV_IsConnected();
            }

            return 101;
        }

        /// <summary>
        /// Function which returns the tracking frequency.
        /// </summary>
        public override double getTrackingFrequency()
        {
            int trackingFrequencyHtz = getTrackingFrequencyInHz();

            //calculate from htz to milliseconds
            double trackingFrequencyMS = 1000.0 / trackingFrequencyHtz;

            return trackingFrequencyMS;
        }

        /// <summary>
        /// Function which returns the tracking frequency in hertz.
        /// </summary>
        private int getTrackingFrequencyInHz()
        {
            EyeTrackingController.SystemInfoStruct systemInfo = new EyeTrackingController.SystemInfoStruct();

            ETDevice.iV_GetSystemInfo(ref systemInfo);

            return systemInfo.samplerate;
        }

        #region CallbackFunctions

        /// <summary>
        /// Callback function for sample Data.
        /// </summary>
        /// <remarks>
        /// The data is for one tracking point. Timestamps in microseconds if normale samplestruct, if 32 bit sample struct its in milliseconds...
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

           EyeTrackingData leftEye = new EyeTrackingData(sampleData.leftEye.gazeX, sampleData.leftEye.gazeY, sampleData.timestamp.ToString());

           leftEye.Diameter = sampleData.leftEye.diam;

           leftEye.EyePositionX = sampleData.leftEye.eyePositionX;
           leftEye.EyePositionY = sampleData.leftEye.eyePositionY;
           leftEye.EyePositionZ = sampleData.leftEye.eyePositionZ;

           EyeTrackingData rightEye = new EyeTrackingData(sampleData.rightEye.gazeX, sampleData.rightEye.gazeY, sampleData.timestamp.ToString());

           leftEye.Diameter = sampleData.rightEye.diam;

           leftEye.EyePositionX = sampleData.rightEye.eyePositionX;
           leftEye.EyePositionY = sampleData.rightEye.eyePositionY;
           leftEye.EyePositionZ = sampleData.rightEye.eyePositionZ;

           triggerGazeEvent(leftEye, rightEye);
        }

        /// <summary>
        /// Callback function for event Data.
        /// </summary>
        /// <remarks>
        /// The data is for mainly fixations. Timestamps in microseconds if normale samplestruct, if 32 bit sample struct its in milliseconds...
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

            triggerTrackingEvent(rawEvent);
        }

        /// <summary>
        /// Callback function for calibration Data.
        /// </summary>
        void CalibrationCallbackFunction(EyeTrackingController.CalibrationPointStruct calibrationPointData)
        {
            String data = "Data from CalibrationCallback - Number:" + calibrationPointData.number + " PosX:" + calibrationPointData.positionX + " PosY:" + calibrationPointData.positionY;

            //Logger.Log(data);
        }

        #endregion
    }
}