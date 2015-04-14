using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Server;
using WebAnalyzer.Util;

namespace WebAnalyzer.Test.Communication
{
    class MouseTrackingInterface
    {
        public MouseTrackingInterface() {
            initCallback();
        }

        private void initCallback() {
            MouseModel.getInstance().PositionTracked += c_PositionTracked;
        }

        public void c_PositionTracked(object sender, PositionEventArgs e)
        {
            Logger.Log("Positiontracked Event: " + e.X + " / " + e.Y);

            Object msg = new { command = "request", x = e.X, y = e.Y, timestamp = Timestamp.GetUnixTimestamp() };

            ConnectionManager.getInstance().Broadcast(msg);
        }

    }
}
