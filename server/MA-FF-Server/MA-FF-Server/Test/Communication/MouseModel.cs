using System;
using System.Timers;
using System.Windows.Forms;

using WebAnalyzer.Util;
using WebAnalyzer.Events;


namespace WebAnalyzer.Test.Communication
{

    /// <summary>
    /// Zusammenfassungsbeschreibung für EyeTrackingModel
    /// </summary>
    public class MouseModel
    {

        private System.Timers.Timer timer;

        public event PrepareGazeEventHandler PrepareGaze;

        public MouseModel()
        {
            initTimer();
        }

        private void initTimer()
        {
            this.timer = new System.Timers.Timer();
            this.timer.Enabled = true;
            this.timer.Elapsed += new ElapsedEventHandler(timer_Tick);
            this.timer.Interval = 100;
        }

        public void startTracking()
        {
            Logger.Log("Start MouseTracking");
            this.timer.Start();
        }


        public void stopTracking()
        {
            Logger.Log("Stop MouseTracking");
            this.timer.Stop();
        }

        private void position()
        {
            //Logger.Log("Mouseposition: " + Cursor.Position.X + " / " + Cursor.Position.Y);
            PrepareGaze(this, new PrepareGazeDataEvent(Cursor.Position.X, Cursor.Position.Y, Cursor.Position.X, Cursor.Position.Y));
        }

        private void timer_Tick(object sender, ElapsedEventArgs e)
        {
            position();
        }
    }
}