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
    /// <summary>
    /// Abstract base class for all tracking components
    /// </summary>
    public abstract class BaseTrackingModel
    {
        /// <summary>
        /// Enumeration of the implemented tracking components
        /// </summary>
        public enum TRACKING_MODELS 
        {
            eye,
            mouse 
        };

        /// <summary>
        /// Returns the tracking component based on the application settings
        /// </summary>
        /// <returns>Tracking Component</returns>
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

        /// <summary>
        /// Representation of the Status of the tracking component
        /// </summary>
        protected ExperimentObject.CONNECTION_STATUS _connectionStatus = ExperimentObject.CONNECTION_STATUS.disconnected;

        /// <summary>
        /// Getter/ Setter for the connection status
        /// </summary>
        public ExperimentObject.CONNECTION_STATUS ConnectionStatus
        {
            get { return _connectionStatus; }
            set { _connectionStatus = value; }
        }

        /// <summary>
        /// Abstract method for connecting the tracking component
        /// </summary>
        /// <returns></returns>
        public abstract ExperimentObject.CONNECTION_STATUS connect();

        /// <summary>
        /// Abstract method for disconnecting the tracking component
        /// </summary>
        /// <returns></returns>
        public abstract ExperimentObject.CONNECTION_STATUS disconnect();

        /// <summary>
        /// Abstract method for starting the tracking
        /// </summary>
        public abstract void startTracking();

        /// <summary>
        /// Abstract method for stopping the tracking
        /// </summary>
        public abstract void stopTracking();

        /// <summary>
        /// Abstract method for getting the tracking frequency of the component
        /// </summary>
        public abstract double getTrackingFrequency();

        /// <summary>
        /// Method for triggering the prepare gaze event with the given data
        /// </summary>
        /// <param name="leftEye">Data about the left eye</param>
        /// <param name="rightEye">Data about the right eye</param>
        protected void triggerGazeEvent(BaseTrackingData leftEye, BaseTrackingData rightEye)
        {
            PrepareGaze(this, new PrepareGazeDataEvent(leftEye, rightEye));
        }

        /// <summary>
        /// Method for triggering the add tracking event event with the given data
        /// </summary>
        /// <param name="rawEvent">Data about the tracking event</param>
        protected void triggerTrackingEvent(RawTrackingEvent rawEvent)
        {
            AddTrackingEvent(this, new AddTrackingEvent(rawEvent));
        }
    }
}
