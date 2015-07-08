using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebAnalyzer.UI;
using WebAnalyzer.ApplicationSettings;
using WebAnalyzer.Util;

namespace WebAnalyzer.Controller
{
    public class MainController
    {

        public void Start()
        {
            ShowMainUI();
        }

        private void ShowMainUI()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            HTMLUI mainUI = new HTMLUI();
            mainUI.Shown += new System.EventHandler(this.MainUIFinishedLoading);
            Application.Run(mainUI);
        }

        private void MainUIFinishedLoading(object sender, EventArgs e)
        {
            //ShowExperimentWizard();
        }

        private void ShowExperimentWizard()
        {
            Logger.Log("Show experiment wizard?");
            ExperimentWizard experimentWizard = new ExperimentWizard();
            if (experimentWizard.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("user pressed ok");
            }
        }
    }
}
