using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

using WebAnalyzer.Models.Base;
using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.SettingsModel.ExpressionTree
{
    /// <summary>
    /// Abstract base class for all tree nodes
    /// </summary>
    public abstract class Node : UIDBase
    {
        /// <summary>
        /// All implemented node types
        /// </summary>
        public enum NODE_TYPES { NONE = -1, VALUE = 0, NOT = 1, AND = 2, OR = 3 };

        /// <summary>
        /// Children of the node
        /// </summary>
        protected List<Node> _children = new List<Node>();

        /// <summary>
        /// Type of the node
        /// </summary>
        protected NODE_TYPES _type = NODE_TYPES.NONE;

        /// <summary>
        /// Empty constructor for loading from xml
        /// </summary>
        public Node()
            : base()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type of the node</param>
        public Node(NODE_TYPES type)
            : base()
        {
            _type = type;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="child">Child of the Node</param>
        public Node(Node child)
            : base()
        {
            _children.Add(child);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="children">Children of the node</param>
        public Node(List<Node> children)
            : base()
        {
            _children = children;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type of the Node</param>
        /// <param name="children">Children of the node</param>
        public Node(NODE_TYPES type, List<Node> children)
            : base()
        {
            _type = type;
            _children = children;
        }

        /// <summary>
        /// Getter / Setter for the node type
        /// </summary>
        public NODE_TYPES NodeType
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Getter / Setter for the node children
        /// </summary>
        public List<Node> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        /// <summary>
        /// Returns the node by uid
        /// </summary>
        /// <param name="uid">UID of the node</param>
        /// <returns></returns>
        public Node FindNode(String uid)
        {
            if (this.UID == uid)
            {
                return this;
            }

            foreach (Node child in _children)
            {
                Node node = child.FindNode(uid);

                if (node != null)
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="nodeNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
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

        /// <summary>
        /// Extracts the node type from the given XMLNode
        /// </summary>
        /// <param name="nodeNode">XMLNode with node data</param>
        /// <returns></returns>
        private static NODE_TYPES ExtractNodeType(XmlNode nodeNode)
        {
            if (nodeNode != null)
            {
                foreach (XmlAttribute attr in nodeNode.Attributes)
                {
                    switch (attr.Name)
                    {
                        case "type":
                            return (NODE_TYPES)Enum.Parse(typeof(NODE_TYPES), attr.Value);
                    }
                }
            }

            return NODE_TYPES.NONE;
        }

        /// <summary>
        /// Creates the node from the XMLNode and the nodeType
        /// </summary>
        /// <param name="nodeNode">Data about the node in XMLNode</param>
        /// <param name="nodeType">Type of the node to create</param>
        /// <returns></returns>
        private static Node CreateNodeFromXML(XmlNode nodeNode, NODE_TYPES nodeType)
        {
            if (nodeNode != null && nodeNode.ChildNodes != null)
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
            }

            return null;
        }

        /// <summary>
        /// Creates a copy of the given node
        /// </summary>
        /// <param name="orig">node to copy</param>
        /// <returns></returns>
        public static Node Copy(Node orig)
        {
            NODE_TYPES nodeType = orig._type;

            Node copy = CreateNode(nodeType, orig);

            foreach (Node child in orig.Children)
            {
                copy.Children.Add(Node.Copy(child));
            }

            return copy;
        }

        /// <summary>
        /// Creates a node from the node type and the original data
        /// </summary>
        /// <param name="nodeType">Type of the node</param>
        /// <param name="orig">original node</param>
        /// <returns></returns>
        private static Node CreateNode(NODE_TYPES nodeType, Node orig)
        {
            switch (nodeType)
            {

                case NODE_TYPES.AND:
                    return new AndNode();
                case NODE_TYPES.NOT:
                    return new NotNode();
                case NODE_TYPES.OR:
                    return new OrNode();
                case NODE_TYPES.VALUE:
                    return ValueNode.Copy((ValueNode) orig);
            }

            return null;
        }

        /// <summary>
        /// Method for evaluating if the element fits the node rule
        /// </summary>
        /// <param name="el">Element to check</param>
        /// <returns></returns>
        public abstract Boolean Evaluate(DOMElementModel el);

        /// <summary>
        /// Method for evaluating case sensitive if the element fits the node rule
        /// </summary>
        /// <param name="el">Element to check</param>
        /// <returns></returns>
        public abstract Boolean EvaluateCaseSensitive(DOMElementModel el);

    }
}
