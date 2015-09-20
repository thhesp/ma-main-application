using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Events;
using WebAnalyzer.UI.InteractionObjects;

namespace WebAnalyzer.Models.Base
{
    public abstract class BaseTrackingModel
    {

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
