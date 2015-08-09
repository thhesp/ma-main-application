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

        public String getRootUID()
        {
            return _rule.RuleRoot.UID;
        }

        public String getRootType()
        {
            if (_rule.RuleRoot is AndNode)
            {
                return "and";
            }
            else if (_rule.RuleRoot is OrNode)
            {
                return "or";
            }

            return "";
        }

        public String[] getChildUIDs(String parentUID)
        {
            Node node = _rule.RuleRoot.FindNode(parentUID);

            String[] childrenUIDS = new String[node.Children.Count];

            for (int i = 0; i < node.Children.Count; i++)
            {
                childrenUIDS[i] = node.Children[i].UID;
            }

            Logger.Log("Children for " + parentUID + " : " + childrenUIDS);

            return childrenUIDS;
        }

        public String getNodeType(String uid)
        {
            Node node = _rule.RuleRoot.FindNode(uid);

            if (node is AndNode)
            {
                return "and";
            }
            else if (node is OrNode)
            {
                return "or";
            }
            else if (node is NotNode)
            {
                return "not";
            }
            else if (node is ValueNode)
            {
                return "value";
            }

            return "";
        }

        public String[] getValueNodeData(String uid)
        {
            String[] data = new String[2];
            
            Node node = _rule.RuleRoot.FindNode(uid);

            if(node is ValueNode){

                ValueNode valNode = (ValueNode) node;

                data[0] = valNode.ValueType.ToString().ToLower();
                data[1] = valNode.Value;

                Logger.Log("NodeData: " + data);

                return data;

            }

            return null;
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
