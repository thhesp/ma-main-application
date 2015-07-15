using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace WebAnalyzer.Models.SettingsModel
{
    public class ExperimentSettings
    {

        private List<DomainSettings> _domains = new List<DomainSettings>();

        public ExperimentSettings()
        {

        }

        public List<DomainSettings> GetDomains()
        {
            return _domains;
        }

        public String[] GetDomainUIDs()
        {
            String[] domainIdentifier = new String[Domains.Count];

            for (int i = 0; i < Domains.Count; i++)
            {
                domainIdentifier[i] = _domains[i].Domain;
            }

            return domainIdentifier;
        }

        public DomainSettings GetDomainSettingByUid(String uid)
        {
            foreach (DomainSettings setting in Domains)
            {
                if (setting.Domain.Equals(uid))
                {
                    return setting;
                }

            }

            return null;
        }

        public List<DomainSettings> Domains
        {
            get { return _domains; }
            set { _domains = value; }
        }

        public void AddDomain(DomainSettings domain)
        {
            _domains.Add(domain);
        }

        public void DeleteDomain(DomainSettings domain)
        {
            _domains.Remove(domain);
        }

        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode settings = xmlDoc.CreateElement("settings");

            foreach (DomainSettings domainSettings in _domains)
            {
                settings.AppendChild(domainSettings.ToXML(xmlDoc));
            }


            return settings;
        }

        public static ExperimentSettings LoadFromXML(XmlDocument doc)
        {
            ExperimentSettings settings = new ExperimentSettings();

            XmlNode settingsNode = doc.DocumentElement.SelectSingleNode("/settings");

            if (settingsNode == null)
            {
                return null;
            }

            foreach (XmlNode child in settingsNode.ChildNodes)
            {
                DomainSettings domain = DomainSettings.LoadFromXML(child);

                if (domain != null)
                {
                    settings.AddDomain(domain);
                }
            }

            return settings;
        }
    }
}
