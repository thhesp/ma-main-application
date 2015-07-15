using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace WebAnalyzer.Models.SettingsModel
{
    public class DomainSettings
    {

        private String _domain;

        private Boolean _includesSubdomains;

        private List<AOISettings> _aois = new List<AOISettings>();

        public DomainSettings()
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
                        //domain.IncludesSubdomains = attr.Value;
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
            copy.AOIS = orig.AOIS;

            return copy;
        }
    }
}
