using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.SettingsModel.ExpressionTree
{
    public abstract class Node
    {

        protected Node _leftChild;
        protected Node _rightChild;

        public Node()
        {

        }

        public Node(Node leftChild, Node rightChild)
        {
            _leftChild = leftChild;
            _rightChild = rightChild;
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

        public abstract Boolean Evaluate(DOMElementModel el);
        public abstract Boolean EvaluateCaseSensitive(DOMElementModel el);

    }
}
