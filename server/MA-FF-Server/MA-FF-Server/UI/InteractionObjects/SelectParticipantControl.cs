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
    public class SelectParticipantControl : BaseInteractionObject
    {

        public event SelectParticipantForTestEventHandler SelectParticipant;

        private SelectParticipantForm _form;

        private ExperimentModel _exp;

        public SelectParticipantControl(SelectParticipantForm form)
        {
            _form = form;
        }

        public ExperimentModel Experiment
        {
            get { return _exp; }
            set { _exp = value; }
        }


        public void selectParticipant(String uid)
        {
            SelectParticipant(this, new SelectParticipantForTestEvent(uid));

            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.OK;
            });
        }

        public String[] participantArray()
        {
            if (_exp != null)
                return _exp.GetParticipantArray();

            return null;
        }

        public String[] participantUIDs()
        {
            if (_exp != null)
                return _exp.GetParticipantUIDs();

            return null;
        }
    }
}
