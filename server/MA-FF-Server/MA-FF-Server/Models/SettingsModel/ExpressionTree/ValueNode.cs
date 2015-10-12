using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;
using System.Xml;

namespace WebAnalyzer.Models.SettingsModel.ExpressionTree
{
    /// <summary>
    /// Value node
    /// </summary>
    /// <remarks>
    /// Always at the leafes of the rule tree.
    /// </remarks>
    public class ValueNode : Node
    {
        /// <summary>
        /// The different value types.
        /// </summary>
        public enum VALUE_TYPES { Tag, Class, ID };

        /// <summary>
        /// the value of the node
        /// </summary>
        private String _value;

        /// <summary>
        /// The value type of the node
        /// </summary>
        private VALUE_TYPES _valueType;

        /// <summary>
        /// Empty Constructor for loading from xml
        /// </summary>
        public ValueNode()
            : base(Node.NODE_TYPES.VALUE)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="valueType">Type of the node</param>
        /// <param name="value">Value of the node</param>
        public ValueNode(VALUE_TYPES valueType, String value) : base(Node.NODE_TYPES.VALUE)
        {
            _valueType = valueType;
            _value = value;
        }

        /// <summary>
        /// Getter/ Setter for the value of the node
        /// </summary>
        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Getter/ Setter for the value type of the node
        /// </summary>
        public VALUE_TYPES ValueType
        {
            get { return _valueType; }
            set { _valueType = value; }
        }

        /// <summary>
        /// Method for evaluating if the element fits the node rule
        /// </summary>
        /// <param name="el">Element to check</param>
        /// <returns></returns>
        public override bool Evaluate(DOMElementModel el)
        {
            String loweredValue = Value.ToLower();
            if (VALUE_TYPES.Tag == ValueType)
            {
                return el.Tag.ToLower() == loweredValue;
            }
            else if (VALUE_TYPES.ID == ValueType)
            {
                return  el.ID.ToLower() == loweredValue;
            }
            else if (VALUE_TYPES.Class == ValueType)
            {
                return el.GetClasses().Contains(loweredValue, StringComparer.OrdinalIgnoreCase);
            }

            return false;
        }

        /// <summary>
        /// Method for evaluating case sensitive if the element fits the node rule
        /// </summary>
        /// <param name="el">Element to check</param>
        /// <returns></returns>
        public override bool EvaluateCaseSensitive(DOMElementModel el)
        {
            if (VALUE_TYPES.Tag == ValueType)
            {
                return el.Tag == Value;
            }
            else if (VALUE_TYPES.ID == ValueType)
            {
                return  el.ID == Value;
            }
            else if (VALUE_TYPES.Class == ValueType)
            {
                return el.GetClasses().Contains(Value);
            }

            return false;
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public override XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode node = xmlDoc.CreateElement("node");

            XmlAttribute nodeType = xmlDoc.CreateAttribute("type");

            nodeType.Value = NodeType.ToString();

            node.Attributes.Append(nodeType);

            XmlAttribute valueType = xmlDoc.CreateAttribute("value-type");

            valueType.Value = ValueType.ToString();

            node.Attributes.Append(valueType);

            XmlAttribute value = xmlDoc.CreateAttribute("value");

            value.Value = Value.ToString();

            node.Attributes.Append(value);

            return node;
        }

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="nodeNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
        public static new ValueNode LoadFromXML(XmlNode nodeNode)
        {

            ValueNode node = new ValueNode();

            foreach (XmlAttribute attr in nodeNode.Attributes)
            {
                switch (attr.Name)
                {
                    case "value-type":
                        node.ValueType = (VALUE_TYPES)Enum.Parse(typeof(VALUE_TYPES), attr.Value);
                        break;
                    case "value":
                        node.Value = attr.Value;
                        break;
                }
            }

            return node;
        }

        /// <summary>
        /// Creates a copy of the given value node
        /// </summary>
        /// <param name="orig">value node to copy</param>
        /// <returns></returns>
        public static ValueNode Copy(ValueNode orig)
        {
            ValueNode copy = new ValueNode();

            copy.Value = orig.Value;

            copy.ValueType = orig.ValueType;

            return copy;
        }
    }
}
