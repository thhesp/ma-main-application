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
    public class RuleControl : BaseInteractionObject
    {

        public event CreateRuleEventtHandler CreateRule;

        private EditRuleForm _form;

        private SettingsRule _rule;

        private Boolean _create;

        private TreeGenerator _generator;

        public RuleControl(EditRuleForm form, SettingsRule rule, Boolean create)
        {
            _form = form;
            _rule = rule;
            _create = create;
            _generator = new TreeGenerator();
        }

        public Boolean creatingNewRule()
        {
            return _create;
        }

        public void saveRule()
        {
            //_rule.RuleRoot = _generator.generate();
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

        public Boolean getCaseSensitive()
        {
            return _rule.CaseSensitive;
        }

        public void setCaseSensitive(Boolean caseSensitive)
        {
            _rule.CaseSensitive = caseSensitive;
        }

        public void addTagConstraint(String type, String value)
        {
            Logger.Log("Tag Constraint: " + type + " - " + value);
        }

        public void addIDConstraint(String type, String value)
        {
            Logger.Log("ID Constraint: " + type + " - " + value);
        }

        public void addClassConstraint(String type, String value)
        {
            Logger.Log("Class Constraint: " + type + " - " + value);
        }

        public void setFirstSelect(String value)
        {

        }

        public void setSecondSelect(String value)
        {

        }
    }
}
