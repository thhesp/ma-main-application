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


    }
}
