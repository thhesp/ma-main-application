using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.SettingsModel.ExpressionTree
{
    public class XorNode : Node
    {

        public XorNode(Node leftChild, Node rightChild) : base(Node.NODE_TYPES.XOR, leftChild, rightChild)
        {

        }

        public override bool Evaluate(DOMElementModel el)
        {
            return _leftChild.Evaluate(el) || _rightChild.Evaluate(el);
        }

        public override bool EvaluateCaseSensitive(DOMElementModel el)
        {
            return _leftChild.Evaluate(el) || _rightChild.Evaluate(el);
        }
    }
}
