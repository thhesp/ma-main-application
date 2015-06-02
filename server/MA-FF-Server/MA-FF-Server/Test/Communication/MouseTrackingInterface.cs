using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Experiment;
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
            //Logger.Log("Positiontracked Event: " + e.X + " / " + e.Y);

            String uniqueId = ExperimentController.getInstance().PrepareGazeData(Timestamp.GetMillisecondsUnixTimestamp(), e.X, e.Y, e.X, e.Y);
            ConnectionManager.getInstance().RequestData(uniqueId, e.X, e.Y);

            //ConnectionManager.getInstance().RequestData(uniqueId, e.X, e.Y, e.X, e.Y);
        }

    }
}
