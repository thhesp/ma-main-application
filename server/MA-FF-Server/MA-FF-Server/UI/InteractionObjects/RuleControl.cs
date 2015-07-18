using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.SettingsModel;
using System.Windows.Forms;

using WebAnalyzer.Events;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class RuleControl : BaseInteractionObject
    {

        public event CreateRuleEventtHandler CreateRule;

        private EditRuleForm _form;

        private SettingsRule _rule;

        private Boolean _create;

        public RuleControl(EditRuleForm form, SettingsRule rule, Boolean create)
        {
            _form = form;
            _rule = rule;
            _create = create;
        }


        public Boolean creatingNewRule()
        {
            return _create;
        }

        public void saveRule()
        {
            if (_create)
            {
                CreateRule(this, new CreateRuleEvent(_rule));
            }

            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.OK;
            });
        }

        public void cancel()
        {
            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.Abort;
            });
        }
    }
}
