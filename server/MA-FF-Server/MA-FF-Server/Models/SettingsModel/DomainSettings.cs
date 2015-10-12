using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using WebAnalyzer.Models.Base;
using WebAnalyzer.Models.DataModel;
using WebAnalyzer.Util;

namespace WebAnalyzer.Models.SettingsModel
{
    /// <summary>
    /// Class which represents all settings for the domain
    /// </summary>
    public class DomainSettings : UIDBase
    {
        /// <summary>
        /// The domain for this setting
        /// </summary>
        private String _domain;

        /// <summary>
        /// Should the settings include subdomains
        /// </summary>
        private Boolean _includesSubdomains;

        /// <summary>
        /// List of aoi settings
        /// </summary>
        private List<AOISettings> _aois = new List<AOISettings>();

        /// <summary>
        /// Empty Constructor used for loading from xml
        /// </summary>
        public DomainSettings()
            : base()
        {

        }

        /// <summary>
        /// Getter/ Setter for the domain
        /// </summary>
        public String Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }

        /// <summary>
        /// Getter/ Setter if subdomains are including or not
        /// </summary>
        public Boolean IncludesSubdomains
        {
            get { return _includesSubdomains; }
            set { _includesSubdomains = value; }
        }

        /// <summary>
        /// Constructor for the domain setting
        /// </summary>
        /// <param name="domain">The domain</param>
        public DomainSettings(String domain)
        {
            _domain = domain;
        }

        /// <summary>
        /// Getter/ Setter for the list of aois
        /// </summary>
        public List<AOISettings> AOIS
        {
            get { return _aois; }
            set { _aois = value; }
        }

        /// <summary>
        /// Creates an array of aoi identifiers
        /// </summary>
        /// <returns>Array of aoi identifiers</returns>
        /// <remarks>
        /// Used for the displaying of the domains in the ui
        /// </remarks>
        public String[] GetAOIIdentifiers()
        {
            String[] aoiIdentifiers = new String[AOIS.Count];

            for (int i = 0; i < AOIS.Count; i++)
            {
                aoiIdentifiers[i] = AOIS[i].Identifier;
            }

            return aoiIdentifiers;
        }

        /// <summary>
        /// Creates an array of aoi uids
        /// </summary>
        /// <returns>Array of aoi uids</returns>
        /// <remarks>
        /// Used for the displaying of the domains in the ui
        /// </remarks>
        public String[] GetAOIUIDs()
        {
            String[] aoiUID = new String[AOIS.Count];

            for (int i = 0; i < AOIS.Count; i++)
            {
                aoiUID[i] = AOIS[i].UID;
            }

            return aoiUID;
        }

        /// <summary>
        /// Returns the aoi setting for the given uid
        /// </summary>
        /// <param name="uid">UID of the aoi setting</param>
        /// <returns></returns>
        public AOISettings GetAOISettingByUid(String uid)
        {
            foreach (AOISettings setting in AOIS)
            {
                if (setting.UID.Equals(uid))
                {
                    return setting;
                }

            }

            return null;
        }

        /// <summary>
        /// Returns the list of aois settings
        /// </summary>
        /// <returns></returns>
        public List<AOISettings> GetAOIs()
        {
            return _aois;
        }

        /// <summary>
        /// Adds the given aoi setting
        /// </summary>
        /// <param name="aoi">AOI setting to add</param>
        public void AddAOI(AOISettings aoi)
        {
            _aois.Add(aoi);
        }

        /// <summary>
        /// Deletes the given aoi setting
        /// </summary>
        /// <param name="aoi">AOI Setting to delete</param>
        public void DeleteRule(AOISettings aoi)
        {
            _aois.Remove(aoi);
        }

        /// <summary>
        /// Checks if the url fits the setting data
        /// </summary>
        /// <param name="url">URL to check</param>
        /// <returns></returns>
        public Boolean URLFitsSetting(String url)
        {
            try
            {
                Uri urlURI = new Uri(url);
                string urlHost = urlURI.Host;

                Uri domainURI = new Uri(_domain);
                string domainHost = domainURI.Host;

                // check if the url host is the same as the domain host
                if (urlHost.Equals(domainHost))
                {
                    Logger.Log("setting host " + domainHost + " fits webpage host " + urlHost);
                    return true;
                }

                //if subdomains are included
                if (IncludesSubdomains)
                {
                    Logger.Log("check if subdomain fits");
                    //check if url on the same domain
                    // if it is the top level domain like google.com
                    // subdomains like maps.google.com must contain google.com
                    if (urlHost.Contains(domainHost))
                    {
                        Logger.Log(urlHost + " contains " + domainHost);
                        return true;
                    }
                }
            }
            catch (UriFormatException e)
            {
                Logger.Log("UriFormatException while checking domain settings for: " + _domain + " --- " + e.Message);
            }
            

            return false;
        }

        /// <summary>
        /// Returns the aois for the given element
        /// </summary>
        /// <param name="el">Element to check</param>
        /// <returns></returns>
        public String[] GetFittingAOIs(DOMElementModel el)
        {
            List<String> aois = new List<String>();
            foreach (AOISettings setting in AOIS)
            {
                if (setting.ElementBelongsToAOI(el))
                {
                    aois.Add(setting.Identifier);
                }
            }

            return aois.ToArray<String>();
        }

        /// <summary>
        /// Creates an xml representation of the object
        /// </summary>
        /// <param name="xmlDoc">XML Document which will contain the representation</param>
        /// <returns></returns>
        public XmlNode ToXML(XmlDocument xmlDoc)
        {
            XmlNode domain = xmlDoc.CreateElement("domain-setting");

            XmlAttribute domainName = xmlDoc.CreateAttribute("domain");

            domainName.Value = Domain;

            domain.Attributes.Append(domainName);

            XmlAttribute includeSubdomains = xmlDoc.CreateAttribute("include-subdomains");

            includeSubdomains.Value = IncludesSubdomains.ToString();

            domain.Attributes.Append(includeSubdomains);

            foreach (AOISettings aoiSettings in _aois)
            {
                domain.AppendChild(aoiSettings.ToXML(xmlDoc));
            }


            return domain;
        }

        /// <summary>
        /// Creates an object from XML
        /// </summary>
        /// <param name="domainNode">The node which contains the data</param>
        /// <returns>The loaded object</returns>
        public static DomainSettings LoadFromXML(XmlNode domainNode)
        {

            DomainSettings domain = new DomainSettings();

            foreach (XmlAttribute attr in domainNode.Attributes)
            {
                switch (attr.Name)
                {
                    case "domain":
                        domain.Domain = attr.Value;
                        break;
                    case "include-subdomains":
                        domain.IncludesSubdomains = Boolean.Parse(attr.Value);
                        break;
                }
            }

            foreach (XmlNode child in domainNode.ChildNodes)
            {
                AOISettings aoi = AOISettings.LoadFromXML(child);

                if (aoi != null)
                {
                    domain.AddAOI(aoi);
                }
            }

            return domain;
        }

        /// <summary>
        /// Creates a copy of the given domain setting
        /// </summary>
        /// <param name="orig">Domain setting to copy</param>
        /// <returns></returns>
        public static DomainSettings Copy(DomainSettings orig)
        {
            DomainSettings copy = new DomainSettings();

            copy.Domain = "Kopie - " + orig.Domain;
            copy.IncludesSubdomains = orig.IncludesSubdomains;

            foreach (AOISettings aoi in orig.AOIS)
            {
                copy.AOIS.Add(AOISettings.Clone(aoi));
            }

            return copy;
        }
    }
}
