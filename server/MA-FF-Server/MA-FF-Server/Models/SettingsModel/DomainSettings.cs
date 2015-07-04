using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.SettingsModel
{
    public class DomainSettings
    {

        private String _domain;

        private Boolean _includesSubdomains;

        private List<AOISettings> _aois = new List<AOISettings>();

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
    }
}
