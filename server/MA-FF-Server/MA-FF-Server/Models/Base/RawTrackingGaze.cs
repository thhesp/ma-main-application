using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.Base
{
    public class RawTrackingGaze
    {
        private EyeTrackingData _leftEye;
        private EyeTrackingData _rightEye;

        public RawTrackingGaze(String callbackTimestamp, double leftX, double leftY, double rightX, double rightY)
        {
            _leftEye = new EyeTrackingData(leftX, leftY, callbackTimestamp);
            _rightEye = new EyeTrackingData(rightX, rightY, callbackTimestamp);
        }


        public RawTrackingGaze(String callbackTimestamp, double x, double y)
        {
            _leftEye = new EyeTrackingData(x, y, callbackTimestamp);
            _rightEye = _leftEye;
        }
    }
}
