using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace WebAnalyzer.Util
{   
    
    
    /// <summary>
    /// Class which is a timer with utilises the stopwatch. This enables it to tick every millisecond.
    /// </summary>
    public class StopwatchTimer
    {
        /// <summary>
        /// The delegate for the callback.
        /// </summary>
        public delegate void Tick();

        /// <summary>
        /// The interval in which it should "tick".
        /// </summary>
        private int _intervalInMS;
        /// <summary>
        /// The callback reference.
        /// </summary>
        private Tick _callback;

        /// <summary>
        /// The Stopwatch reference used for the timer.
        /// </summary>
        private Stopwatch _watch;
        /// <summary>
        /// Timestamp of the last tick.
        /// </summary>
        private long _lastTick;

        /// <summary>
        /// Constructor for the stopwatch timer.
        /// </summary>
        /// <param name="intervalInMS">The interval in which the Timer should tick.</param>
        /// <param name="callback">The callback which shall be called on each tick.</param>
        public StopwatchTimer(int intervalInMS, Tick callback)
        {
            _intervalInMS = intervalInMS;
            _callback = callback;
            _watch = new Stopwatch();
        }

        /// <summary>
        /// Starts the timer in a seperate thread.
        /// </summary>
        public void Start()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                _watch.Start();
                _lastTick = _watch.ElapsedMilliseconds;

                while (_watch.IsRunning)
                {
                    if (_watch.ElapsedMilliseconds - _lastTick > _intervalInMS)
                    {
                        _lastTick = _watch.ElapsedMilliseconds;
                        _callback();
                    }

                }
            }).Start();

        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            if (_watch.IsRunning)
            {
                _watch.Stop();
                _watch.Reset();
            }
        }
    }
}
