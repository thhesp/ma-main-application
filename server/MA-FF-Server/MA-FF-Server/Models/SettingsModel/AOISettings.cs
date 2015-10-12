using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Models.SettingsModel
{
    /// <summary>
    /// Represents all settings for the aoi
    /// </summary>
    public class AOISettings : UIDBase
    {

        /// <summary>
        /// AOI Identifier
        /// </summary>
        private String _identifier;

        /// <summary>
        /// List of Rules for this aoi
        /// </summary>
        private List<SettingsRule> _rules = new List<SettingsRule>();

        /// <summary>
        /// Empty constructor for the loading from xml
        /// </summary>
        public AOISettings() : base()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="identifier">AOI Identifier</param>
        public AOISettings(String identifier) : base()
        {
            _identifier = identifier;
        }

        /// <summary>
        /// Getter / Setter for the aoi identifier
        /// </summary>
        public String Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        /// <summary>
        /// Getter / Setter for the aoi rules
        /// </summary>
        public List<SettingsRule> Rules
        {
            get { return _rules; }
            set { _rules = value; }
        }

        /// <summary>
        /// Creates an array of rule uids
        /// </summary>
        /// <returns>Array of UIDS</returns>
        public String[] GetRuleUIDs()
        {
            String[] ruleUIDs = new String[Rules.Count];

            for (int i = 0; i < Rules.Count; i++)
            {
                ruleUIDs[i] = Rules[i].UID;
            }

            return ruleUIDs;
        }

        /// <summary>
        /// Gets the rule setting by the given UID
        /// </summary>
        /// <param name="uid">Rule setting UID</param>
        /// <returns></returns>
        public SettingsRule GetRuleSettingByUid(String uid)
        {
            foreach (SettingsRule rule in Rules)
            {
                if (rule.UID.Equals(uid))
                {
                    return rule;
                }

            }

            return null;
        }

        /// <summary>
        /// Returns the list for setting rules
        /// </summary>
        /// <returns></returns>
        public List<SettingsRule> GetRules()
        {
            return _rules;
        }

        /// <summary>
        /// Adds a setting rule
        /// </summary>
        /// <param name="rule">Rule to add</param>
        public void AddRule(SettingsRule rule)
        {
            _rules.Add(rule);
        }

        /// <summary>
        /// Delets a setting rule
        /// </summary>
        /// <param name="rule">Rule to delete</param>
        public void DeleteRule(SettingsRule rule)
        {
            _rules.Remove(rule);
        }

        /// <summary>
        /// Checks if the given element belongs to the aoi
        /// </summary>
        /// <param name="el">Element to check</param>
        /// <returns></returns>
        public Boolean ElementBelongsToAOI(DOMElementModel el)
        {
            foreach (SettingsRule rule in Rules)
            {
                if (rule.ElementFitsRule(el))
                {
                    return true;
                }

            }

            return false;
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode aoi = xmlDoc.CreateElement("aoi-setting");

            XmlAttribute identifier = xmlDoc.CreateAttribute("identifier");

            identifier.Value = Identifier;

            aoi.Attributes.Append(identifier);

            foreach (SettingsRule rule in _rules)
            {
                aoi.AppendChild(rule.ToXML(xmlDoc));
            }


            return aoi;
        }

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="aoiNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
        public static AOISettings LoadFromXML(XmlNode aoiNode)
        {
            AOISettings aoi = new AOISettings();

            foreach (XmlAttribute attr in aoiNode.Attributes)
            {
                switch (attr.Name)
                {
                    case "identifier":
                        aoi.Identifier = attr.Value;
                        break;
                }
            }

            foreach (XmlNode child in aoiNode.ChildNodes)
            {
                SettingsRule rule = SettingsRule.LoadFromXML(child);

                if (rule != null)
                {
                    aoi.AddRule(rule);
                }
            }

            return aoi;
        }

        /// <summary>
        /// Creates a copy of the given aoi setting
        /// </summary>
        /// <param name="orig">aoi setting to copy</param>
        /// <returns></returns>
        public static AOISettings Copy(AOISettings orig)
        {
            AOISettings copy = new AOISettings();

            copy.Identifier = "Kopie - " + orig.Identifier;

            foreach (SettingsRule rule in orig.Rules)
            {
                copy.Rules.Add(SettingsRule.Copy(rule));
            }

            return copy;
        }

        /// <summary>
        /// Creates a clone of the given aoi setting
        /// </summary>
        /// <param name="orig">aoi setting to clone</param>
        /// <returns></returns>
        public static AOISettings Clone(AOISettings orig)
        {
            AOISettings copy = new AOISettings();

            copy.Identifier = (String) orig.Identifier.Clone();

            foreach (SettingsRule rule in orig.Rules)
            {
                copy.Rules.Add(SettingsRule.Copy(rule));
            }

            return copy;
        }
    }
}
