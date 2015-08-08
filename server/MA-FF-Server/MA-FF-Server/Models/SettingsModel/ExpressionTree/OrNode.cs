using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.SettingsModel.ExpressionTree
{
    public class OrNode : Node
    {

        public OrNode(List<Node> children)
            : base(Node.NODE_TYPES.OR, children)
        {

        }

        public override bool Evaluate(DOMElementModel el)
        {
            foreach(Node child in Children){
                if (child.Evaluate(el))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool EvaluateCaseSensitive(DOMElementModel el)
        {
            foreach (Node child in Children)
            {
                if (child.EvaluateCaseSensitive(el))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
