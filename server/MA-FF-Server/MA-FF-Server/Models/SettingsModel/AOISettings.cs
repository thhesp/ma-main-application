﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace WebAnalyzer.Models.SettingsModel
{
    public class AOISettings
    {

        private String _identifier;

        private List<SettingsRule> _rules = new List<SettingsRule>();

        public AOISettings()
        {

        }

        public AOISettings(String identifier)
        {
            _identifier = identifier;
        }

        public String Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public List<SettingsRule> Rules
        {
            get { return _rules; }
            set { _rules = value; }
        }

        public List<SettingsRule> GetRules()
        {
            return _rules;
        }

        public void AddRule(SettingsRule rule)
        {
            _rules.Add(rule);
        }

        public void DeleteRule(SettingsRule rule)
        {
            _rules.Remove(rule);
        }


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

        public static AOISettings Copy(AOISettings orig)
        {
            AOISettings copy = new AOISettings();

            copy.Identifier = "Kopie - " + orig.Identifier;
            copy.Rules = orig.Rules;

            return copy;
        }
    }
}
