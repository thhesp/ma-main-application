using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using WebAnalyzer.Util;
using WebAnalyzer.Models.Base;
using WebAnalyzer.Events;
using WebAnalyzer.Controller;

using System.IO;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class SelectTestrunControl : BaseInteractionObject
    {

        public event SelectTestrunToLoadEventHandler SelectTestrun;

        private SelectTestrunForm _form;

        private List<String> _testrunPaths = new List<String>();
        private List<String> _testrunCreatedTimestamps = new List<String>();


        public SelectTestrunControl(SelectTestrunForm form, ExperimentModel exp, ExperimentParticipant participant)
        {
            _form = form;

            extractTestrunData(exp, participant);
        }

        public void selectTestrun(String created, String path)
        {
            SelectTestrun(this, new SelectTestrunToLoadEvent(created, path));

            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.OK;
            });
        }

        private void extractTestrunData(ExperimentModel exp, ExperimentParticipant participant)
        {
            String path = LoadController.GetTestdataLocation(exp, participant);

            Logger.Log("Rawdata path: " + path);

            String[] files = Directory.GetFiles(path);

            for (int i = 0; i < files.Length; i++)
            {

                if (LoadController.ValidateTestrunFile(files[i]))
                {
                    _testrunPaths.Add(files[i]);

                    String filename = Path.GetFileNameWithoutExtension(files[i]);

                    String created = Timestamp.GetCreatedFromFilename(filename);

                    _testrunCreatedTimestamps.Add(created);
                }
            }

        }

        public String[] testrunPaths()
        {
            return _testrunPaths.ToArray<String>();
        }

        public String[] testrunDates()
        {
            return _testrunCreatedTimestamps.ToArray<String>();
        }
    }
}
