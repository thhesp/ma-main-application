using System;
using System.Text;

using WebAnalyzer.Experiment;
using WebAnalyzer.Server;
using WebAnalyzer.Util;

namespace WebAnalyzer.EyeTracking
{

    /// <summary>
    /// Zusammenfassungsbeschreibung für EyeTrackingModel
    /// </summary>
    public class EyeTrackingModel
    {

        private static EyeTrackingModel instance;

        public static EyeTrackingModel getInstance()
        {
            if (instance == null)
            {
                instance = new EyeTrackingModel();
            }

            return instance;
        }



        public EyeTrackingController ETDevice;
        public EyeTrackingController.CalibrationStruct m_CalibrationData;
        public EyeTrackingController.AccuracyStruct m_AccuracyData;
        public EyeTrackingController.SampleStruct m_SampleData;
        public EyeTrackingController.EventStruct m_EventData;


        // callback routine declaration
        public delegate void CalibrationCallback(EyeTrackingController.CalibrationPointStruct calibrationPointData);
        public delegate void GetSampleCallback(EyeTrackingController.SampleStruct sampleData);
        public delegate void GetEventCallback(EyeTrackingController.EventStruct eventData);

        // callback function instances
        CalibrationCallback m_CalibrationCallback;
        GetSampleCallback m_SampleCallback;
        GetEventCallback m_EventCallback;



        private EyeTrackingModel()
        {
            ETDevice = new EyeTrackingController();

            m_CalibrationCallback = new CalibrationCallback(CalibrationCallbackFunction);
            m_SampleCallback = new GetSampleCallback(GetSampleCallbackFunction);
            m_EventCallback = new GetEventCallback(GetEventCallbackFunction);
        }

        public Boolean connect(String sendip, String sendport, String receiveip, String receiveport)
        {
            Logger.Log("Connect with "+sendip+":"+sendport+" to "+receiveip+":"+receiveport);

            try
            {

                ETDevice.iV_SetCalibrationCallback(m_CalibrationCallback);
                ETDevice.iV_SetSampleCallback(m_SampleCallback);
                ETDevice.iV_SetEventCallback(m_EventCallback);

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
                Logger.Log(exc.Message);
                // log error?
                //logger1.Text = "Exception during iV_Connect: " + exc.Message;
                return false;
            }
        }


        /// <summary>
        /// Connect to the Eye Tracking Server.
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



            return connect("127.0.0.1", "4444", "127.0.0.1", "5555");
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

        #region CallbackFunctions

        // callback functions
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

           Logger.Log(data);
        }


        void GetEventCallbackFunction(EyeTrackingController.EventStruct eventData)
        {
            String data = "Data from EventCallback - eye: " + eventData.eye.ToString() +
                " Event: " + eventData.eventType + " startTime: " + eventData.startTime.ToString() +
                " End:" + eventData.endTime.ToString() + " duration:" + eventData.duration.ToString() +
                " PosX:" + eventData.positionX.ToString() + " PosY:" + eventData.positionY.ToString();


            String uniqueId = ExperimentController.getInstance().PreparePositionData(eventData.positionX, eventData.positionY, eventData.startTime.ToString(), eventData.endTime.ToString(), eventData.duration.ToString());
            ConnectionManager.getInstance().RequestData(uniqueId, eventData.positionX, eventData.positionY);


            Logger.Log(data);
        }

        void CalibrationCallbackFunction(EyeTrackingController.CalibrationPointStruct calibrationPointData)
        {
            String data = "Data from CalibrationCallback - Number:" + calibrationPointData.number + " PosX:" + calibrationPointData.positionx + " PosY:" + calibrationPointData.positiony;

            Logger.Log(data);
        }

        #endregion
    }
}