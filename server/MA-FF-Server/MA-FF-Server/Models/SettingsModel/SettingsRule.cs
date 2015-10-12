using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Models.SettingsModel.ExpressionTree;
using System.Xml;
using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Models.SettingsModel
{
    /// <summary>
    /// Represents a rule for the aoi
    /// </summary>
    public class SettingsRule : UIDBase
    {

        /// <summary>
        /// Is the rule case sensitive
        /// </summary>
        private Boolean _caseSensitive;

        /// <summary>
        /// Root of the rule tree
        /// </summary>
        private Node _ruleRoot;

        /// <summary>
        /// Empty constructor for loading from xml
        /// </summary>
        public SettingsRule()
            : base()
        {

        }

        /// <summary>
        /// Getter / Setter for the rule root
        /// </summary>
        public Node RuleRoot
        {
            get { return _ruleRoot; }
            set { _ruleRoot = value; }
        }

        /// <summary>
        /// Getter / Setter for Case sensitive
        /// </summary>
        public Boolean CaseSensitive
        {
            get { return _caseSensitive; }
            set { _caseSensitive = value; }
        }

        /// <summary>
        /// Checks if the given element fits the rule
        /// </summary>
        /// <param name="el">Element to check</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="ruleNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
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

        /// <summary>
        /// Creates a copy of the given setting rule
        /// </summary>
        /// <param name="orig">setting rule to copy</param>
        /// <returns></returns>
        public static SettingsRule Copy(SettingsRule orig)
        {
            SettingsRule copy = new SettingsRule();

            copy.CaseSensitive = orig.CaseSensitive;
            copy.RuleRoot = Node.Copy(orig.RuleRoot);

            return copy;
        }
    }
}
