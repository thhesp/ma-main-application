using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.Base;
using WebAnalyzer.Models.SettingsModel;

namespace WebAnalyzer.Events
{
    public delegate void CreateDomainSettingEventHandler(object sender, CreateDomainSettingEvent e);

    public class CreateDomainSettingEvent : EventArgs
    {

        private DomainSettings _domain;

        public CreateDomainSettingEvent(DomainSettings domain)
        {
            _domain = domain;
        }

        public DomainSettings Domain
        {
            get { return _domain; }
        }
    }
}
