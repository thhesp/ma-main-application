using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using WebAnalyzer.Util;
using WebAnalyzer.Models.Base;

using WebAnalyzer.Events;

namespace WebAnalyzer.UI.InteractionObjects
{
    /// <summary>
    /// Interaction object for controlling the testrun
    /// </summary>
    public class TestrunControl : BaseInteractionObject
    {

        /// <summary>
        /// Eventhandler for starting and stopping the testrun
        /// </summary>
        public event TestrunEventHandler Testrun;

        /// <summary>
        /// Eventhandler for selecting a participant
        /// </summary>
        public event SelectParticipantForTestEventHandler SelectParticipant;

        /// <summary>
        /// Eventhandler to adding data (label or protocol) to the testrun
        /// </summary>
        public event AddTestrunDataEventHandler AddTestrunData;

        /// <summary>
        /// Reference to the testrun window
        /// </summary>
        private TestrunForm _form;

        /// <summary>
        /// Reference to the currently loaded experiment
        /// </summary>
        private ExperimentModel _exp;

        /// <summary>
        /// Flag if the test is currently running
        /// </summary>
        private Boolean _running = false;

        /// <summary>
        /// Reference to the selected participant
        /// </summary>
        private ExperimentParticipant _participant;

        /// <summary>
        /// Label of the testrun
        /// </summary>
        private String _label;

        /// <summary>
        /// Protocol for the testrun
        /// </summary>
        private String _protocol;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">Testrun window</param>
        public TestrunControl(TestrunForm form)
        {
            _form = form;
        }

        /// <summary>
        /// Getter / Setter for the currently loaded experiment
        /// </summary>
        public ExperimentModel Experiment
        {
            get { return _exp; }
            set { _exp = value; }
        }

        /// <summary>
        /// Method for updating the label
        /// </summary>
        /// <param name="label">the current label</param>
        /// <remarks>Called from Javascript</remarks>
        public void updateLabel(String label)
        {
            _label = label;
        }

        /// <summary>
        /// Method for updating the protocol
        /// </summary>
        /// <param name="protocol">the current protocol</param>
        /// <remarks>Called from Javascript</remarks>
        public void updateProtocol(String protocol)
        {
            _protocol = protocol;
        }

        /// <summary>
        /// Returns the Label
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from Javascript</remarks>
        public String getLabel()
        {
            return _label;
        }

        /// <summary>
        /// returns the protocol
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from Javascript</remarks>
        public String getProtocol()
        {
            return _protocol;
        }

        /// <summary>
        /// Updates the testrun object with the current label and protocol
        /// </summary>
        /// <remarks>Called from Javascript</remarks>
        public void addTestrunData()
        {
            AddTestrunData(this, new AddTestrunDataEvent(_label, _protocol));
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        /// <remarks>Called from Javascript</remarks>
        public void endTest()
        {

            if (_running)
            {
                Testrun(this, new TestrunEvent(TestrunEvent.EVENT_TYPE.Stop));
            }

            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.OK;
            });
        }

        /// <summary>
        /// Checks if the test is running
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from Javascript</remarks>
        public Boolean testRunning()
        {
            return _running;
        }

        /// <summary>
        /// Returns the Identifier of the participant
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from Javascript</remarks>
        public String getParticipantIdentifier()
        {
            if (_participant != null)
            {
                return _participant.Identifier;
            }

            return "";
        }

        /// <summary>
        /// Starts the testrun
        /// </summary>
        /// <remarks>Called from Javascript</remarks>
        public void startTestrun()
        {
            _running = true;
            Testrun(this, new TestrunEvent(TestrunEvent.EVENT_TYPE.Start));
        }

        /// <summary>
        /// Stops the testrun
        /// </summary>
        /// <remarks>Called from Javascript</remarks>
        public void stopTestrun()
        {
            _running = false;
            Testrun(this, new TestrunEvent(TestrunEvent.EVENT_TYPE.Stop));
        }

        /// <summary>
        /// Opens a new window for selecting the participant
        /// </summary>
        /// <remarks>Called from Javascript</remarks>
        public void selectParticipant()
        {
            _form.BeginInvoke((Action)delegate
            {
                SelectParticipantForm selectParticipant = new SelectParticipantForm(this.Experiment);

                selectParticipant.SelectParticipant += On_SelectParticipant;
                selectParticipant.SelectParticipant += SelectParticipant;

                if (selectParticipant.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //_form.ReloadPage();
                    
                }
            });
        }

        /// <summary>
        /// Callback when a participant was selected
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void On_SelectParticipant(object source, SelectParticipantForTestEvent e)
        {
            Logger.Log("Selected Participant " + e.UID);

            _participant = _exp.GetParticipantByUID(e.UID);

            _form.ReloadPage();
        }

        /// <summary>
        /// Displays the saving indicator
        /// </summary>
        public void ShowSaveIndicator()
        {
            Logger.Log("show save indicator");
            EvaluteJavaScript("showSaveIndicator();");

        }

        /// <summary>
        /// Hides the saving indicator
        /// </summary>
        public void HideSaveIndicator()
        {
            Logger.Log("hide save indicator");
            EvaluteJavaScript("hideSaveIndicator();");
        }
    }
}
