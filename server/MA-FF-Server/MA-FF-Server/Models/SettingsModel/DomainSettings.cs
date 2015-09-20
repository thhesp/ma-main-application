using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using WebAnalyzer.Models.Base;
using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.SettingsModel
{
    public class DomainSettings : UIDBase
    {

        private String _domain;

        private Boolean _includesSubdomains;

        private List<AOISettings> _aois = new List<AOISettings>();

        public DomainSettings() : base()
        {

        }

        public String Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }

        public Boolean IncludesSubdomains
        {
            get { return _includesSubdomains; }
            set { _includesSubdomains = value;  }
        }

        public DomainSettings(String domain)
        {
            _domain = domain;
        }

        public List<AOISettings> AOIS
        {
            get { return _aois; }
            set { _aois = value; }
        }

        public String[] GetAOIIdentifiers()
        {
            String[] aoiIdentifiers = new String[AOIS.Count];

            for (int i = 0; i < AOIS.Count; i++)
            {
                aoiIdentifiers[i] = AOIS[i].Identifier;
            }

            return aoiIdentifiers;
        }

        public String[] GetAOIUIDs()
        {
            String[] aoiUID = new String[AOIS.Count];

            for (int i = 0; i < AOIS.Count; i++)
            {
                aoiUID[i] = AOIS[i].UID;
            }

            return aoiUID;
        }

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

        public List<AOISettings> GetAOIs()
        {
            return _aois;
        }

        public void AddAOI(AOISettings aoi)
        {
            _aois.Add(aoi);
        }

        public void DeleteRule(AOISettings aoi)
        {
            _aois.Remove(aoi);
        }

        public Boolean URLFitsSetting(String url)
        {
            Uri urlURI = new Uri(url);
            string urlHost = urlURI.Host;

            Uri domainURI = new Uri(url);
            string domainHost = domainURI.Host;

            // check if the url host is the same as the domain host
            if (urlHost.Equals(domainHost))
            {
                return true;
            }

            //if subdomains are included
            if (IncludesSubdomains)
            {
                //check if url on the same domain
                // if it is the top level domain like google.com
                // subdomains like maps.google.com must contain google.com
                if (urlHost.Contains(domainHost))
                {
                    return true;
                }
            }

            return false;
        }

        public String GetFittingAOI(DOMElementModel el)
        {
            foreach (AOISettings setting in AOIS)
            {
                if (setting.ElementBelongsToAOI(el))
                {
                    return setting.Identifier;
                }

            }

            return "";
        }

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
