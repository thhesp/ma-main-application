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
    /// <summary>
    /// Class which contain all experiment settings.
    /// </summary>
    public class ExperimentSettings
    {

        /// <summary>
        /// List of domain settings
        /// </summary>
        private List<DomainSettings> _domains = new List<DomainSettings>();

        /// <summary>
        /// Empty constructor used from loading from xml
        /// </summary>
        public ExperimentSettings()
        {

        }

        /// <summary>
        /// Returns the list of domains
        /// </summary>
        /// <returns></returns>
        public List<DomainSettings> GetDomains()
        {
            return _domains;
        }

        /// <summary>
        /// Creates an array of domain identifiers
        /// </summary>
        /// <returns>Array of domain identifiers</returns>
        /// <remarks>
        /// Used for the displaying of the domains in the ui
        /// </remarks>
        public String[] GetDomainIdentifiers()
        {
            String[] domainIdentifier = new String[Domains.Count];

            for (int i = 0; i < Domains.Count; i++)
            {
                domainIdentifier[i] = _domains[i].Domain;
            }

            return domainIdentifier;
        }

        /// <summary>
        /// Creates an array of domain uids
        /// </summary>
        /// <returns>Array of domain uids</returns>
        /// <remarks>
        /// Used for the displaying of the domains in the ui
        /// </remarks>
        public String[] GetDomainUIDs()
        {
            String[] domainUIDs = new String[Domains.Count];

            for (int i = 0; i < Domains.Count; i++)
            {
                domainUIDs[i] = _domains[i].UID;
            }

            return domainUIDs;
        }

        /// <summary>
        /// Returns domain setting by uid
        /// </summary>
        /// <param name="uid">UID of the domain setting</param>
        /// <returns></returns>
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

        /// <summary>
        /// Getter/ Setter for the list of domains
        /// </summary>
        public List<DomainSettings> Domains
        {
            get { return _domains; }
            set { _domains = value; }
        }

        /// <summary>
        /// Adds domain to the list of domains
        /// </summary>
        /// <param name="domain">Domain to add</param>
        public void AddDomain(DomainSettings domain)
        {
            _domains.Add(domain);
        }

        /// <summary>
        /// Removes domain from list of domains
        /// </summary>
        /// <param name="domain">Domain to remove</param>
        public void DeleteDomain(DomainSettings domain)
        {
            _domains.Remove(domain);
        }

        /// <summary>
        /// Returns the domain settings for the given webpage model
        /// </summary>
        /// <param name="webpage">Webpage model to check</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the domain settings for the given url
        /// </summary>
        /// <param name="url">URL to check</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns all aois for the given url and the given element
        /// </summary>
        /// <param name="url">URL to check</param>
        /// <param name="el">Element to check</param>
        /// <returns></returns>
        public String[] GetAOIs(String url, DOMElementModel el)
        {
            DomainSettings setting = GetSettings(url);

            if (setting != null)
            {
                return setting.GetFittingAOIs(el);
            }

            return new String[0];
        }

        /// <summary>
        /// Returns all aois for the given webpage model and the given element
        /// </summary>
        /// <param name="webpage">Webpage model which contains the element</param>
        /// <param name="el">Element for which to check for aois</param>
        /// <returns></returns>
        public String[] GetAOIs(WebpageModel webpage, DOMElementModel el)
        {
            DomainSettings setting = GetSettings(webpage);

            if (setting != null)
            {
                return setting.GetFittingAOIs(el);
            }

            return new String[0];
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode settings = xmlDoc.CreateElement("settings");

            foreach (DomainSettings domainSettings in _domains)
            {
                settings.AppendChild(domainSettings.ToXML(xmlDoc));
            }


            return settings;
        }

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="doc">XML Document which contains the data</param>
        /// <returns>The loaded object</returns>
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
