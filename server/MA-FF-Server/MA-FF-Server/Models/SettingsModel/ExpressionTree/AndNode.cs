using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.SettingsModel.ExpressionTree
{
    public class AndNode : Node
    {
        public AndNode(List<Node> children)
            : base(Node.NODE_TYPES.AND, children)
        {

        }

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
