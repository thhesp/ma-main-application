using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void serverStart_Click(object sender, EventArgs e)
        {
            Server.getInstance().start();
        }

        private void serverStop_Click(object sender, EventArgs e)
        {
            Server.getInstance().stop();
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
