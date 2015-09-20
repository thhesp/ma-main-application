using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebAnalyzer.Events;
using WebAnalyzer.UI.InteractionObjects;

using WebAnalyzer.EyeTracking;
using WebAnalyzer.Test.Communication;
using WebAnalyzer.Util;

namespace WebAnalyzer.Models.Base
{
    public abstract class BaseTrackingModel
    {
        public enum TRACKING_MODELS 
        {
            eye,
            mouse 
        };

        public static BaseTrackingModel GetTrackingModel()
        {
            Logger.Log("Tracking Type: " + Properties.Settings.Default.TrackingModelType);

            TRACKING_MODELS model = (TRACKING_MODELS) Enum.Parse(typeof(TRACKING_MODELS), Properties.Settings.Default.TrackingModelType);

            switch (model)
            {
                case TRACKING_MODELS.eye:
                    return new EyeTrackingModel();

                case TRACKING_MODELS.mouse:
                    return new MouseModel();
            }

            return null;
        }

        /// <summary>
        /// Event which is used for sending data to the test controller.
        /// </summary>
        public event PrepareGazeEventHandler PrepareGaze;

        /// <summary>
        /// Event which is used for sending events to the test controller.
        /// </summary>
        public event AddTrackingEventHandler AddTrackingEvent;

        protected ExperimentObject.CONNECTION_STATUS _connectionStatus = ExperimentObject.CONNECTION_STATUS.disconnected;


        public ExperimentObject.CONNECTION_STATUS ConnectionStatus
        {
            get { return _connectionStatus; }
            set { _connectionStatus = value; }
        }

        public abstract ExperimentObject.CONNECTION_STATUS connect();

        public abstract ExperimentObject.CONNECTION_STATUS disconnect();

        public abstract void startTracking();

        public abstract void stopTracking();

        protected void triggerGazeEvent(BaseTrackingData leftEye, BaseTrackingData rightEye)
        {
            PrepareGaze(this, new PrepareGazeDataEvent(leftEye, rightEye));
        }

        protected void triggerTrackingEvent(RawTrackingEvent rawEvent)
        {
            AddTrackingEvent(this, new AddTrackingEvent(rawEvent));
        }
    }
}
