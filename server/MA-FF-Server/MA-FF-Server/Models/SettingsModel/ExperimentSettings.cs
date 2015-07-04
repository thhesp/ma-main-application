using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void AddDomain(DomainSettings domain)
        {
            _domains.Add(domain);
        }

        public void DeleteDomain(DomainSettings domain)
        {
            _domains.Remove(domain);
        }
    }
}
