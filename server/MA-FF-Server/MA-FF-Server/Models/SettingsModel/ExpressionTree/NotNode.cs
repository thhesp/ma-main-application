using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.SettingsModel.ExpressionTree
{

    /// <summary>
    /// Represents an not condition in the tree
    /// </summary>
    public class NotNode : Node
    {

        /// <summary>
        /// Empty constructor for loading from xml
        /// </summary>
        public NotNode()
            : base(Node.NODE_TYPES.NOT)
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="child">Child of the node</param>
        public NotNode(Node child)
            : base(Node.NODE_TYPES.NOT)
        {
            _children.Add(child);
        }

        /// <summary>
        /// Method for evaluating if the element fits the node rule
        /// </summary>
        /// <param name="el">Element to check</param>
        /// <returns></returns>
        public override bool Evaluate(DOMElementModel el)
        {
            return !_children[0].Evaluate(el);
        }

        /// <summary>
        /// Method for evaluating case sensitive if the element fits the node rule
        /// </summary>
        /// <param name="el">Element to check</param>
        /// <returns></returns>
        public override bool EvaluateCaseSensitive(DOMElementModel el)
        {
            return !_children[0].EvaluateCaseSensitive(el);
        }
    }
}
