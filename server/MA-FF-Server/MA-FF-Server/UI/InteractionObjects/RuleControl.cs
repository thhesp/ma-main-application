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
    /// <summary>
    /// Interaction object for creating/ editing setting rules
    /// </summary>
    public class RuleControl : BaseInteractionObject
    {
        /// <summary>
        /// Eventhandler for creating a new rule
        /// </summary>
        public event CreateRuleEventtHandler CreateRule;

        /// <summary>
        /// Reference to the edit rule window
        /// </summary>
        private EditRuleForm _form;

        /// <summary>
        /// Reference to the setting rule
        /// </summary>
        private SettingsRule _rule;

        /// <summary>
        /// Flag if the rule is new
        /// </summary>
        private Boolean _create;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">Reference to the edit rule window</param>
        /// <param name="rule">Reference to the settings rule</param>
        /// <param name="create">Is the rule new</param>
        public RuleControl(EditRuleForm form, SettingsRule rule, Boolean create)
        {
            _form = form;
            _rule = rule;
            _create = create;
        }

        /// <summary>
        /// Returns if the rule is new
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public Boolean creatingNewRule()
        {
            return _create;
        }

        /// <summary>
        /// Used for saving the rule
        /// </summary>
        /// <remarks>Called from javascript</remarks>
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

        /// <summary>
        /// Used for canceling the process
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void cancel()
        {
            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.Abort;
            });
        }

        /// <summary>
        /// Returns if the rule is case sensitive or not
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public Boolean getCaseSensitive()
        {
            return _rule.CaseSensitive;
        }

        /// <summary>
        /// Updates if the rule is case sensitive or not
        /// </summary>
        /// <param name="caseSensitive">The new value</param>
        /// <remarks>Called from javascript</remarks>
        public void setCaseSensitive(Boolean caseSensitive)
        {
            _rule.CaseSensitive = caseSensitive;
        }

        /// <summary>
        /// Creates the rule root
        /// </summary>
        /// <param name="type">Nodetype of the rule root</param>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String createRuleRoot(String type)
        {
            Logger.Log("Ruleroot type: " + type);
            return setRuleRoot(CreateNode(type));
        }

        /// <summary>
        /// Returns the UID of the rule root
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String getRootUID()
        {
            return _rule.RuleRoot.UID;
        }

        /// <summary>
        /// Returns the node type of the rule root
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
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

        /// <summary>
        /// Returns an array of uids for the given node UID
        /// </summary>
        /// <param name="parentUID">UID of the parent node</param>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
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

        /// <summary>
        /// Returns the node type to the given uid
        /// </summary>
        /// <param name="uid">UID of a node</param>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
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

        /// <summary>
        /// Returns the data for the value node
        /// </summary>
        /// <param name="uid">UID of a value node</param>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
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

        /// <summary>
        /// Adds a condition node to the given parent
        /// </summary>
        /// <param name="parentUID">UID of the parent</param>
        /// <param name="type">Type of the condition node</param>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
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

        /// <summary>
        /// Adds a value node to the given parent
        /// </summary>
        /// <param name="parentUID">UID of the parent</param>
        /// <param name="valueType">ValueType of Valuenode</param>
        /// <param name="value">Value of Valuenode</param>
        /// <remarks>Called from javascript</remarks>
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

        /// <summary>
        /// Returns the Value type as an enumeration object
        /// </summary>
        /// <param name="valueType">Valuetype as string</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a condition node from a type
        /// </summary>
        /// <param name="type">Nodetype</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets the rule root of the setting node
        /// </summary>
        /// <param name="root">New rule root</param>
        /// <returns></returns>
        public String setRuleRoot(Node root)
        {
            _rule.RuleRoot = root;

            return root.UID;
        }

    }
}
