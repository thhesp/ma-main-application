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
    public partial class MouseTracker : Form
    {
        public MouseTracker()
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
