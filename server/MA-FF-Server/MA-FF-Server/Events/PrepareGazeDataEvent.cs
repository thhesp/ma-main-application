using System;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Util;

namespace WebAnalyzer.Events
{
    public delegate void PrepareGazeEventHandler(object sender, PrepareGazeDataEvent e);

    public class PrepareGazeDataEvent : EventArgs
    {
        private EyeTrackingData _leftEye, _rightEye;

        public PrepareGazeDataEvent(EyeTrackingData leftEye, EyeTrackingData rightEye)
        {
            _leftEye = leftEye;
            _rightEye = rightEye;
        }

        public EyeTrackingData LeftEye
        {
            get { return _leftEye; }
        }

        public EyeTrackingData RightEye
        {
            get { return _rightEye; }
        }
    }
}
