using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using WebAnalyzer.Util;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class ExperimentWizardObj : BaseInteractionObject
    {


        public void back()
        {
            Browser.Back();
        }

        public void createExperiment()
        {
            string page = string.Format("{0}UI/HTMLResources/html/popup/experiment_wizard/create_experiment.html", Utilities.GetAppLocation());
            Browser.Load(page);
        }


        public void selectExperimentToLoad()
        {
            // done in a new thread (kinda is a hack) because of the STAThread problem.
            //http://stackoverflow.com/questions/6860153/exception-when-using-folderbrowserdialog
            string selectedPath = "";
            var t = new Thread((ThreadStart)(() =>
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.SelectedPath = Properties.Settings.Default.Datalocation;
                fbd.RootFolder = System.Environment.SpecialFolder.MyComputer;
                fbd.ShowNewFolderButton = true;
                if (fbd.ShowDialog() == DialogResult.Cancel)
                    return;

                selectedPath = fbd.SelectedPath;
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            Logger.Log(selectedPath);
        }
    }
}
