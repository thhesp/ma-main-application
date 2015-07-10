using System;
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


            return aoi;
        }

        public static AOISettings LoadFromXML(XmlNode aoiNode)
        {
            AOISettings aoi = new AOISettings();


            return aoi;
        }
    }
}
