using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Models.SettingsModel.ExpressionTree;
using System.Xml;

namespace WebAnalyzer.Models.SettingsModel
{
    public class SettingsRule
    {

        private Boolean _caseSensitive;

        private Node _ruleRoot;

        public Node RuleRoot
        {
            get { return _ruleRoot; }
            set { _ruleRoot = value; }
        }

        public Boolean CaseSensitive
        {
            get { return _caseSensitive; }
            set { _caseSensitive = value; }
        }

        public Boolean ElementFitsRule(DOMElementModel el)
        {
            if (CaseSensitive)
            {
                return RuleRoot.EvaluateCaseSensitive(el);
            }
            else
            {
                return RuleRoot.Evaluate(el);
            }
        }


        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode rule = xmlDoc.CreateElement("rule");

            XmlAttribute caseSensitive = xmlDoc.CreateAttribute("case-sensitive");

            caseSensitive.Value = CaseSensitive.ToString();

            rule.Attributes.Append(caseSensitive);

            if (RuleRoot != null)
            {
                rule.AppendChild(RuleRoot.ToXML(xmlDoc));
            }

            return rule;
        }

        public static SettingsRule LoadFromXML(XmlNode ruleNode)
        {

            SettingsRule rule = new SettingsRule();

            foreach (XmlAttribute attr in ruleNode.Attributes)
            {
                switch (attr.Name)
                {
                    case "case-sensitive":
                        rule.CaseSensitive = Boolean.Parse(attr.Value);
                        break;
                }
            }

            foreach (XmlNode child in ruleNode.ChildNodes)
            {
                Node node = Node.LoadFromXML(child);

                if (node != null)
                {
                    rule.RuleRoot = node;
                }
            }

           
            return rule;
        }
    }
}
