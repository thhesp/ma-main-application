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
    /// <summary>
    /// Class for selecting a testrun
    /// </summary>
    public class SelectTestrunControl : BaseInteractionObject
    {

        /// <summary>
        /// Eventhandler for selecting the testrun
        /// </summary>
        public event SelectTestrunToLoadEventHandler SelectTestrun;

        /// <summary>
        /// Reference to the testrun selection form
        /// </summary>
        private SelectTestrunForm _form;

        /// <summary>
        /// List of all paths to the testruns of the selected participant
        /// </summary>
        private List<String> _testrunPaths = new List<String>();

        /// <summary>
        /// List of all creation timestamps to the testruns of the selected participant
        /// </summary>
        private List<String> _testrunCreatedTimestamps = new List<String>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">Reference to the testrun selection form</param>
        /// <param name="exp">Reference to the currently loaded experiment</param>
        /// <param name="participant">Reference to the currently select participant</param>
        public SelectTestrunControl(SelectTestrunForm form, ExperimentModel exp, ExperimentParticipant participant)
        {
            _form = form;

            extractTestrunData(exp, participant);
        }

        /// <summary>
        /// Used for selecting a testrun
        /// </summary>
        /// <param name="created">The creation timestamp of the testrun</param>
        /// <param name="path">The path to the testrun data</param>
        /// <remarks>
        /// Called from javascript
        /// </remarks>
        public void selectTestrun(String created, String path)
        {
            SelectTestrun(this, new SelectTestrunToLoadEvent(created, path));

            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.OK;
            });
        }

        /// <summary>
        /// Fills the testrunCreatedTimestamps and testrunPaths lists
        /// </summary>
        /// <param name="exp">The currently loaded experiment</param>
        /// <param name="participant">The currently selected participant</param>
        private void extractTestrunData(ExperimentModel exp, ExperimentParticipant participant)
        {
            String path = LoadController.GetTestdataLocation(exp, participant);

            Logger.Log("Testdata path: " + path);

            String[] files = Directory.GetFiles(path);

            for (int i = 0; i < files.Length; i++)
            {

                if (LoadController.ValidateXMLFile(files[i]))
                {
                    _testrunPaths.Add(files[i]);

                    String filename = Path.GetFileNameWithoutExtension(files[i]);

                    String created = Timestamp.GetCreatedFromFilename(filename);

                    _testrunCreatedTimestamps.Add(created);
                }
            }

        }

        /// <summary>
        /// Returns an array of testrun paths
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Called from javascript
        /// </remarks>
        public String[] testrunPaths()
        {
            return _testrunPaths.ToArray<String>();
        }

        /// <summary>
        /// Returns an array of testrun dates
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Called from javascript
        /// </remarks>
        public String[] testrunDates()
        {
            return _testrunCreatedTimestamps.ToArray<String>();
        }
    }
}
