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
    public class TestrunControl : BaseInteractionObject
    {

        public event TestrunEventHandler Testrun;
        public event SelectParticipantForTestEventHandler SelectParticipant;
        public event AddTestrunDataEventHandler AddTestrunData;

        private TestrunForm _form;
        private ExperimentModel _exp;

        private Boolean _running = false;

        private ExperimentParticipant _participant;


        private String _label;
        private String _protocol;

        public TestrunControl(TestrunForm form)
        {
            _form = form;
        }

        public ExperimentModel Experiment
        {
            get { return _exp; }
            set { _exp = value; }
        }

        public void updateLabel(String label)
        {
            _label = label;
        }

        public void updateProtocol(String protocol)
        {
            _protocol = protocol;
        }

        public String getLabel()
        {
            return _label;
        }

        public String getProtocol()
        {
            return _protocol;
        }

        public void addTestrunData()
        {
            AddTestrunData(this, new AddTestrunDataEvent(_label, _protocol));
        }

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

        public Boolean testRunning()
        {
            return _running;
        }

        public String getParticipantIdentifier()
        {
            if (_participant != null)
            {
                return _participant.Identifier;
            }

            return "";
        }

        public void startTestrun()
        {
            _running = true;
            Testrun(this, new TestrunEvent(TestrunEvent.EVENT_TYPE.Start));
        }

        public void stopTestrun()
        {
            _running = false;
            Testrun(this, new TestrunEvent(TestrunEvent.EVENT_TYPE.Stop));
        }

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

        public void On_SelectParticipant(object source, SelectParticipantForTestEvent e)
        {
            Logger.Log("Selected Participant " + e.UID);

            _participant = _exp.GetParticipantByUID(e.UID);

            _form.ReloadPage();
        }

        public void ShowSaveIndicator()
        {
            Logger.Log("show save indicator");
            EvaluteJavaScript("showSaveIndicator();");

        }

        public void HideSaveIndicator()
        {
            Logger.Log("hide save indicator");
            EvaluteJavaScript("hideSaveIndicator();");
        }
    }
}
