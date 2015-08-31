using System;
using System.Timers;
using System.Windows.Forms;

using WebAnalyzer.Util;
using WebAnalyzer.Events;


namespace WebAnalyzer.Test.Communication
{

    /// <summary>
    /// Mousemodel used for testing when no eyetracker is available.
    /// </summary>
    public class MouseModel
    {
        /// <summary>
        /// Interval in milliseconds used for the timer.
        /// </summary>
        private static int INTERVAL_IN_MS = 10;

        /// <summary>
        /// Timer to simulate data every x ms.
        /// </summary>
        private System.Timers.Timer timer;

        /// <summary>
        /// Event which is used for sending data to the controller.
        /// </summary>
        public event PrepareGazeEventHandler PrepareGaze;

        /// <summary>
        /// Constructor which initialises the everything needed.
        /// </summary>
        public MouseModel()
        {
            initTimer();
        }

        /// <summary>
        /// Initialises the timmer and sets the interval.
        /// </summary>
        private void initTimer()
        {
            this.timer = new System.Timers.Timer();
            this.timer.Enabled = true;
            this.timer.Interval = INTERVAL_IN_MS;
        }

        /// <summary>
        /// Function which starts the tracking of the mouse.
        /// </summary>
        public void startTracking()
        {
            Logger.Log("Start MouseTracking");
            this.timer.Elapsed += new ElapsedEventHandler(timer_Tick);
            this.timer.Start();
        }

        /// <summary>
        /// Function which stops the tracking of the mouse.
        /// </summary>
        public void stopTracking()
        {
            Logger.Log("Stop MouseTracking");
            this.timer.Elapsed -= new ElapsedEventHandler(timer_Tick);
            this.timer.Stop();
        }

        /// <summary>
        /// Callback which gets called everytime the timer ends.
        /// Sends the data with the PrepareGaze Event to the controller.
        /// </summary>
        private void timer_Tick(object sender, ElapsedEventArgs e)
        {
            PrepareGaze(this, new PrepareGazeDataEvent(Cursor.Position.X, Cursor.Position.Y, Cursor.Position.X, Cursor.Position.Y));
        }
    }
}