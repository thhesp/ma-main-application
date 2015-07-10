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

            foreach (AOISettings aoiSettings in _aois)
            {
                domain.AppendChild(aoiSettings.ToXML(xmlDoc));
            }


            return domain;
        }

        public static DomainSettings LoadFromXML(XmlNode domainNode)
        {

            DomainSettings domain = new DomainSettings();

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
    }
}
