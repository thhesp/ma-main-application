using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Models.SettingsModel.ExpressionTree;

namespace WebAnalyzer.Models.SettingsModel
{
    public class SettingsRule
    {

        private Boolean _caseSensitive;

        private Node _ruleRoot;

        public Boolean CaseSensitive
        {
            get { return _caseSensitive; }
            set { _caseSensitive = value; }
        }

        public Boolean ElementFitsRule(DOMElementModel el)
        {
            if (CaseSensitive)
            {
                return _ruleRoot.EvaluateCaseSensitive(el);
            }
            else
            {
                return _ruleRoot.Evaluate(el);
            }
        }

       
    }
}
