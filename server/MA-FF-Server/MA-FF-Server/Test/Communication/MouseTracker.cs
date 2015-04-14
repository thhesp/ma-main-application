using System;
using System.Windows.Forms;

using WebAnalyzer.Server;
using WebAnalyzer.Experiment;

namespace WebAnalyzer.Test.Communication
{
    public partial class MouseTracker : Form
    {
        public MouseTracker()
        {
            InitializeComponent();
        }

        private void serverStart_Click(object sender, EventArgs e)
        {
            WebsocketServer.getInstance().start();
            ExperimentController.getInstance().StartExperiment();
        }

        private void serverStop_Click(object sender, EventArgs e)
        {
            WebsocketServer.getInstance().stop();
            ExperimentController.getInstance().StopExperiment();
        }

        private void mousetrackingStart_Click(object sender, EventArgs e)
        {
            MouseModel.getInstance().startTracking();
        }

        private void mousetrackingStop_Click(object sender, EventArgs e)
        {
            MouseModel.getInstance().stopTracking();
        }

    }
}
