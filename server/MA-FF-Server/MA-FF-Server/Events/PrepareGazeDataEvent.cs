using System;

using WebAnalyzer.Util;

namespace WebAnalyzer.Events
{
    public delegate void PrepareGazeEventHandler(object sender, PrepareGazeDataEvent e);

    public class PrepareGazeDataEvent : EventArgs
    {

        private String _timestamp;

        private double _leftX, _leftY, _rightX, _rightY;

        public PrepareGazeDataEvent(String timestamp, double leftX, double leftY, double rightX, double rightY)
        {
            _timestamp = timestamp;
            _leftX = leftX;
            _leftY = leftY;
            _rightX = rightX;
            _rightY = rightY;

        }


        public PrepareGazeDataEvent(double leftX, double leftY, double rightX, double rightY)
        {
            _timestamp = Timestamp.GetMillisecondsUnixTimestamp();
            _leftX = leftX;
            _leftY = leftY;
            _rightX = rightX;
            _rightY = rightY;

        }

        public String GazeTimestamp
        {
            get { return _timestamp; }
        }

        public double LeftX
        {
            get { return _leftX; }
        }

        public double LeftY
        {
            get { return _leftY; }
        }

        public double RightX
        {
            get { return _rightX; }
        }

        public double RightY
        {
            get { return _rightY; }
        }
    }
}
