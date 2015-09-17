using System;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Util;
using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Events
{
    public delegate void PrepareGazeEventHandler(object sender, PrepareGazeDataEvent e);

    public class PrepareGazeDataEvent : EventArgs
    {
        private BaseTrackingData _leftEye, _rightEye;

        public PrepareGazeDataEvent(BaseTrackingData leftEye, BaseTrackingData rightEye)
        {
            _leftEye = leftEye;
            _rightEye = rightEye;
        }

        public BaseTrackingData LeftEye
        {
            get { return _leftEye; }
        }

        public BaseTrackingData RightEye
        {
            get { return _rightEye; }
        }
    }
}
