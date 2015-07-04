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
        public AndNode(Node leftChild, Node rightChild) : base(leftChild, rightChild)
        {

        }

        public override bool Evaluate(DOMElementModel el)
        {
            return _leftChild.Evaluate(el) && _rightChild.Evaluate(el);
        }

        public override bool EvaluateCaseSensitive(DOMElementModel el)
        {
            return _leftChild.Evaluate(el) && _rightChild.Evaluate(el);
        }

    }
}
