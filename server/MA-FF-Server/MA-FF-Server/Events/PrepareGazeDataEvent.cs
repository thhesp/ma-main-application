using System;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Util;
using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// PrepareGazeEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">PrepareGazeDataEvent which is sent</param>
    public delegate void PrepareGazeEventHandler(object sender, PrepareGazeDataEvent e);

    /// <summary>
    /// Event for sending gaze data from the tracking component to the testcontroller
    /// </summary>
    public class PrepareGazeDataEvent : EventArgs
    {
        /// <summary>
        /// Data about the left and right eye
        /// </summary>
        private BaseTrackingData _leftEye, _rightEye;

        /// <summary>
        /// EventConstructor
        /// </summary>
        /// <param name="leftEye">Trackingdata about the left eye</param>
        /// <param name="rightEye">Trackingdata about the right eye</param>
        /// <remarks>The object classes depend on the tracking component but must extend BaseTrackingData</remarks>
        public PrepareGazeDataEvent(BaseTrackingData leftEye, BaseTrackingData rightEye)
        {
            _leftEye = leftEye;
            _rightEye = rightEye;
        }

        /// <summary>
        /// Getter for the left eye data
        /// </summary>
        public BaseTrackingData LeftEye
        {
            get { return _leftEye; }
        }

        /// <summary>
        /// Getter for the right eye data
        /// </summary>
        public BaseTrackingData RightEye
        {
            get { return _rightEye; }
        }
    }
}
