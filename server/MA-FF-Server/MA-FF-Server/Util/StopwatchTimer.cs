using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace WebAnalyzer.Util
{
    public class StopwatchTimer
    {
        public delegate void Tick();

        private int _durationInMS;
        private Tick _callback;

        private Stopwatch _watch;
        private long _lastTick;

        public StopwatchTimer(int durationInMS, Tick callback)
        {
            _durationInMS = durationInMS;
            _callback = callback;
            _watch = new Stopwatch();
        }

        public void start()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                _watch.Start();
                _lastTick = _watch.ElapsedMilliseconds;

                while (_watch.IsRunning)
                {
                    if (_watch.ElapsedMilliseconds - _lastTick > _durationInMS)
                    {
                        _lastTick = _watch.ElapsedMilliseconds;
                        _callback();
                    }

                }
            }).Start();

        }

        public void stop()
        {
            if (_watch.IsRunning)
            {
                _watch.Stop();
                _watch.Reset();
            }
        }
    }
}
