using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.SettingsModel;
using System.Windows.Forms;
using WebAnalyzer.Util;
using WebAnalyzer.Models.SettingsModel.ExpressionTree;

using WebAnalyzer.Events;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class TestrunControl : BaseInteractionObject
    {

        public event TestrunEventHandler Testrun;

        private TestrunForm _form;

        private Boolean _running = false;

        public TestrunControl(TestrunForm form)
        {
            _form = form;
        }


        public void endTest()
        {

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

        }

    }
}
