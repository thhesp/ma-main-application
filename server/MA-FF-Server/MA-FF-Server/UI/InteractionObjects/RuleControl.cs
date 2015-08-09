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

        public String createRuleRoot(String type)
        {
            Logger.Log("Ruleroot type: " + type);
            return setRuleRoot(CreateNode(type));
        }

        public String addConditionNodeToParent(String parentUID, String type)
        {
            Node child = CreateNode(type);

            if (child != null)
            {
                Node parent = _rule.RuleRoot.FindNode(parentUID);

                if (parent != null)
                {
                    parent.Children.Add(child);

                    return child.UID;
                }
                
            }

            return null;
        }

        public void addValueNodeToParent(String parentUID, String valueType, String value)
        {
            Node child = new ValueNode(GetValueType(valueType), value);

            if (child != null)
            {
                Node parent = _rule.RuleRoot.FindNode(parentUID);

                if (parent != null)
                {
                    parent.Children.Add(child);
                }

            }
        }

        public ValueNode.VALUE_TYPES GetValueType(String valueType)
        {
            switch (valueType)
            {
                case "tag":
                    return ValueNode.VALUE_TYPES.Tag;
                case "id":
                    return ValueNode.VALUE_TYPES.ID;
                case "class":
                    return ValueNode.VALUE_TYPES.Class;
            }

            return ValueNode.VALUE_TYPES.Tag;
        }

        public Node CreateNode(String type)
        {
            switch (type)
            {
                case "and":
                    return new AndNode();
                case "or":
                    return new OrNode();
                case "not":
                    return new NotNode();
            }

            return null;
        }

        public String setRuleRoot(Node root)
        {
            _rule.RuleRoot = root;

            return root.UID;
        }

    }
}
