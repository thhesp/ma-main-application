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

        public enum NODE_TYPES { VALUE = 0, NOT = 1, AND = 2, OR = 3, XOR = 4 };

        protected Node _leftChild;
        protected Node _rightChild;

        protected NODE_TYPES _type;

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

            NODE_TYPES nodeType;

            /*foreach (XmlAttribute attr in nodeNode.Attributes)
            {
                switch (attr.Name)
                {
                    case "type":
                        
                        break;
                }
            }

            foreach (XmlNode child in nodeNode.ChildNodes)
            {
                Node node = Node.LoadFromXML(child);

                if (node != null)
                {
                    rule.RuleRoot = node;
                }
            }*/


            return null;
        }

        public abstract Boolean Evaluate(DOMElementModel el);
        public abstract Boolean EvaluateCaseSensitive(DOMElementModel el);

    }
}
