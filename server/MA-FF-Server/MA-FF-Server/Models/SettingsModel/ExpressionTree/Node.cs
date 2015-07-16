using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.SettingsModel.ExpressionTree
{
    public abstract class Node
    {

        public enum NODE_TYPES { NONE = -1, VALUE = 0, NOT = 1, AND = 2, OR = 3, XOR = 4 };

        protected Node _leftChild;
        protected Node _rightChild;

        protected NODE_TYPES _type = NODE_TYPES.NONE;

        public Node()
        {

        }

        public Node(NODE_TYPES type)
        {
            _type = type;
        }

        public Node(Node leftChild, Node rightChild)
        {
            _leftChild = leftChild;
            _rightChild = rightChild;
        }

        public Node(NODE_TYPES type, Node leftChild, Node rightChild)
        {
            _type = type;
            _leftChild = leftChild;
            _rightChild = rightChild;
        }

        public NODE_TYPES NodeType{
            get { return _type; }
            set { _type = value; }
        }

        public Node LeftChild
        {
            get { return _leftChild; }
            set { _leftChild = value; }
        }

        public Node RightChild
        {
            get { return _rightChild; }
            set { _rightChild = value; }
        }

        public virtual XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode node = xmlDoc.CreateElement("node");

            XmlAttribute nodeType = xmlDoc.CreateAttribute("type");

            nodeType.Value = NodeType.ToString();

            node.Attributes.Append(nodeType);

            if (LeftChild != null)
            {
                node.AppendChild(LeftChild.ToXML(xmlDoc));
            }

            if (RightChild != null)
            {
                node.AppendChild(RightChild.ToXML(xmlDoc));
            }

            return node;
        }

        public static Node LoadFromXML(XmlNode nodeNode)
        {

            NODE_TYPES nodeType = ExtractNodeType(nodeNode);

            if (nodeType == NODE_TYPES.VALUE)
            {
                return ValueNode.LoadFromXML(nodeNode);
            }
            else
            {
                return CreateNodeFromXML(nodeNode, nodeType);
            }
        }

        private static NODE_TYPES ExtractNodeType(XmlNode nodeNode)
        {
            foreach (XmlAttribute attr in nodeNode.Attributes)
            {
                switch (attr.Name)
                {
                    case "type":
                        return (NODE_TYPES)Enum.Parse(typeof(NODE_TYPES), attr.Value);
                }
            }

            return NODE_TYPES.NONE;
        }

        private static Node CreateNodeFromXML(XmlNode nodeNode, NODE_TYPES nodeType)
        {
            Node leftChild = Node.LoadFromXML(nodeNode.ChildNodes.Item(0));

            if (nodeType == NODE_TYPES.NOT)
            {
                return new NotNode(leftChild);
            }
            else
            {
                Node rightChild = Node.LoadFromXML(nodeNode.ChildNodes.Item(1));

                switch (nodeType)
                {
                    case NODE_TYPES.AND:
                        return new AndNode(leftChild, rightChild);
                    case NODE_TYPES.OR:
                        return new OrNode(leftChild, rightChild);
                    case NODE_TYPES.XOR:
                        return new XorNode(leftChild, rightChild);
                }

            }

            return null;
     }

        public abstract Boolean Evaluate(DOMElementModel el);
        public abstract Boolean EvaluateCaseSensitive(DOMElementModel el);

    }
}
