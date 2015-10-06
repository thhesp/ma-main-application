using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Util;

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

        public String[] GetDomainIdentifiers()
        {
            String[] domainIdentifier = new String[Domains.Count];

            for (int i = 0; i < Domains.Count; i++)
            {
                domainIdentifier[i] = _domains[i].Domain;
            }

            return domainIdentifier;
        }

        public String[] GetDomainUIDs()
        {
            String[] domainUIDs = new String[Domains.Count];

            for (int i = 0; i < Domains.Count; i++)
            {
                domainUIDs[i] = _domains[i].UID;
            }

            return domainUIDs;
        }

        public DomainSettings GetDomainSettingByUid(String uid)
        {
            foreach (DomainSettings setting in Domains)
            {
                if (setting.UID.Equals(uid))
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


        public DomainSettings GetSettings(WebpageModel webpage)
        {
            foreach (DomainSettings setting in Domains)
            {
                if (setting.URLFitsSetting(webpage.Url))
                {
                    Logger.Log("Domainsettings found: " + setting.Domain);
                    return setting;
                }

            }

            Logger.Log("No fitting Domain for " + webpage.Url + " found.");

            return null;
        }

        public DomainSettings GetSettings(String url)
        {
            foreach (DomainSettings setting in Domains)
            {
                if (setting.URLFitsSetting(url))
                {
                    Logger.Log("Domainsettings found: " + setting.Domain);
                    return setting;
                }

            }

            Logger.Log("No fitting Domain for " + url + " found.");

            return null;
        }

        public String[] GetAOIs(String url, DOMElementModel el)
        {
            DomainSettings setting = GetSettings(url);

            if (setting != null)
            {
                return setting.GetFittingAOIs(el);
            }

            return new String[0];
        }

        public String[] GetAOIs(WebpageModel webpage, DOMElementModel el)
        {
            DomainSettings setting = GetSettings(webpage);

            if (setting != null)
            {
                return setting.GetFittingAOIs(el);
            }

            return new String[0];
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
