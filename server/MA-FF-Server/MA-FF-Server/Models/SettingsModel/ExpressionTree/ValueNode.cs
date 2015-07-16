using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;
using System.Xml;

namespace WebAnalyzer.Models.SettingsModel.ExpressionTree
{
    public class ValueNode : Node
    {

        public enum VALUE_TYPES { Tag, Class, ID };

        private String _value;
        private VALUE_TYPES _valueType;

        public ValueNode()
            : base(Node.NODE_TYPES.VALUE)
        {

        }

        public ValueNode(VALUE_TYPES valueType, String value) : base(Node.NODE_TYPES.VALUE)
        {
            _valueType = valueType;
            _value = value;
        }

        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public VALUE_TYPES ValueType
        {
            get { return _valueType; }
            set { _valueType = value; }
        }

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

        public static ValueNode LoadFromXML(XmlNode nodeNode)
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
    }
}
