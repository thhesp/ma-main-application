using System;
using System.Windows.Forms;

using WebAnalyzer.Server;
using WebAnalyzer.EyeTracking;
using WebAnalyzer.Experiment;

namespace WebAnalyzer.UI
{
    public partial class TestForm : Form
    {
        public TestForm()
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

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            EyeTrackingModel.getInstance().disconnect();
        }

        private void btnConnectLocal_Click(object sender, EventArgs e)
        {
            EyeTrackingModel.getInstance().connectLocal();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

            EyeTrackingModel.getInstance().connect(inputIP.Text,inputPort.Text,inputSendIP.Text,inputSendPort.Text);
        }
    }
}
