﻿using System;
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
        /// Timer to simulate data every x ms.
        /// </summary>
        private StopwatchTimer timer;

        /// <summary>
        /// Event which is used for sending data to the test controller.
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
            timer = new StopwatchTimer(Properties.Settings.Default.MouseTrackingInterval, timer_Tick);
        }

        /// <summary>
        /// Function which starts the tracking of the mouse.
        /// </summary>
        public void startTracking()
        {
            Logger.Log("Start MouseTracking");
            timer.Start();
        }

        /// <summary>
        /// Function which stops the tracking of the mouse.
        /// </summary>
        public void stopTracking()
        {
            Logger.Log("Stop MouseTracking");
            this.timer.Stop();
        }

        /// <summary>
        /// Callback which gets called everytime the timer ends.
        /// Sends the data with the PrepareGaze Event to the controller.
        /// </summary>
        private void timer_Tick()
        {
            PrepareGaze(this, new PrepareGazeDataEvent(Cursor.Position.X, Cursor.Position.Y, Cursor.Position.X, Cursor.Position.Y));
        }
    }
}