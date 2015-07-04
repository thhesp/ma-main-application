using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.SettingsModel.ExpressionTree
{
    public class NotNode : Node
    {

        public NotNode(Node child)
            : base()
        {
            _leftChild = child;
        }

        public override bool Evaluate(DOMElementModel el)
        {
            return !_leftChild.Evaluate(el); ;
        }

        public override bool EvaluateCaseSensitive(DOMElementModel el)
        {
            return !_leftChild.Evaluate(el); ;
        }
    }
}
