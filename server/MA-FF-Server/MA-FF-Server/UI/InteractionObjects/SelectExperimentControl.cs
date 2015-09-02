using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using WebAnalyzer.Util;
using WebAnalyzer.Models.Base;
using System.IO;

using WebAnalyzer.Controller;
using WebAnalyzer.Events;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class SelectExperimentControl : BaseInteractionObject
    {

        public event SelectExperimentEventHandler SelectExperiment;

        private SelectExperimentForm _form;

        private List<String> _experimentNames = new List<String>();
        private List<String> _experimentPaths = new List<String>();

        public SelectExperimentControl(SelectExperimentForm form)
        {
            _form = form;
            createExperimentList();
        }

        private void createExperimentList()
        {
            String path = Properties.Settings.Default.Datalocation;

            String[] directories = Directory.GetDirectories(path);

            for (int i = 0; i < directories.Length; i++)
            {

                if (LoadController.ValidateExperimentFolder(directories[i]))
                {
                    _experimentPaths.Add(directories[i]);

                    _experimentNames.Add(directories[i].Remove(0, path.Length));
                }
            }
        }

        public void selectExperiment(String path)
        {
            Logger.Log("Select experiment: " + path);

            SelectExperiment(this, new SelectExperimentEvent(path));

            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.OK;
            });
        }

        public String[] getExperimentNames()
        {
            return _experimentNames.ToArray<String>();
        }

        public String[] getExperimentPaths()
        {
            return _experimentPaths.ToArray<String>();
        }
    }
}
