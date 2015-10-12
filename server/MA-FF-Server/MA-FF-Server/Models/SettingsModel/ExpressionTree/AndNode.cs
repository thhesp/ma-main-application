using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.SettingsModel.ExpressionTree
{
    /// <summary>
    /// Represents an and condition in the tree
    /// </summary>
    public class AndNode : Node
    {

        /// <summary>
        /// Empty constructor for loading from xml
        /// </summary>
        public AndNode()
            : base(Node.NODE_TYPES.AND)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="children">Children of the node</param>
        public AndNode(List<Node> children)
            : base(Node.NODE_TYPES.AND, children)
        {

        }

        /// <summary>
        /// Method for evaluating if the element fits the node rule
        /// </summary>
        /// <param name="el">Element to check</param>
        /// <returns></returns>
        public override bool Evaluate(DOMElementModel el)
        {
            foreach (Node child in Children)
            {
                if (!child.Evaluate(el))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Method for evaluating case sensitive if the element fits the node rule
        /// </summary>
        /// <param name="el">Element to check</param>
        /// <returns></returns>
        public override bool EvaluateCaseSensitive(DOMElementModel el)
        {
            foreach (Node child in Children)
            {
                if (!child.EvaluateCaseSensitive(el))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
