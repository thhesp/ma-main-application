using System;
using System.Windows.Forms;

using WebAnalyzer.Util;


namespace WebAnalyzer.Test.Communication
{

    /// <summary>
    /// Zusammenfassungsbeschreibung für EyeTrackingModel
    /// </summary>
    public class MouseModel
    {

        private static MouseModel instance;

        private Timer timer;


        public static MouseModel getInstance()
        {
            if (instance == null)
            {
                instance = new MouseModel();
            }

            return instance;
        }

        private MouseModel()
        {
            initTimer();
        }

        private void initTimer()
        {
            this.timer = new Timer();
            this.timer.Tick += new EventHandler(timer_Tick);
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

        public void position()
        {
            Logger.Log("Mouseposition: " + Cursor.Position.X + " / " + Cursor.Position.Y);
            PositionEventArgs args = new PositionEventArgs();
            args.X = Cursor.Position.X;
            args.Y = Cursor.Position.Y;
            OnPositionTracked(args);
        }

        #region CallbackFunctions

        private void timer_Tick(object sender, EventArgs e)
        {
            position();
        }

        protected virtual void OnPositionTracked(PositionEventArgs e)
        {
            EventHandler<PositionEventArgs> handler = PositionTracked;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        public event EventHandler<PositionEventArgs> PositionTracked;
    }

    public class PositionEventArgs : EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}