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

        public enum NODE_TYPES { NONE = -1, VALUE = 0, NOT = 1, AND = 2, OR = 3};

        protected List<Node> _children = new List<Node>();

        protected NODE_TYPES _type = NODE_TYPES.NONE;

        public Node()
        {

        }

        public Node(NODE_TYPES type)
        {
            _type = type;
        }

        public Node(Node child)
        {
            _children.Add(child);
        }

        public Node(List<Node> children)
        {
            _children = children;
        }

        public Node(NODE_TYPES type, List<Node> children)
        {
            _type = type;
            _children = children;
        }

        public NODE_TYPES NodeType{
            get { return _type; }
            set { _type = value; }
        }

        public List<Node> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public virtual XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode node = xmlDoc.CreateElement("node");

            XmlAttribute nodeType = xmlDoc.CreateAttribute("type");

            nodeType.Value = NodeType.ToString();

            node.Attributes.Append(nodeType);

            foreach (Node child in _children)
            {
                node.AppendChild(child.ToXML(xmlDoc));
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
            List<Node> childrenNodes = new List<Node>();

            foreach (XmlNode children in nodeNode.ChildNodes)
            {
                childrenNodes.Add(Node.LoadFromXML(children));
            }

            if (nodeType == NODE_TYPES.NOT)
            {
                return new NotNode(childrenNodes[0]);
            }
            else
            {
                Node rightChild = Node.LoadFromXML(nodeNode.ChildNodes.Item(1));

                switch (nodeType)
                {
                    case NODE_TYPES.AND:
                        return new AndNode(childrenNodes);
                    case NODE_TYPES.OR:
                        return new OrNode(childrenNodes);
                }

            }

            return null;
     }

        public abstract Boolean Evaluate(DOMElementModel el);
        public abstract Boolean EvaluateCaseSensitive(DOMElementModel el);

    }
}
