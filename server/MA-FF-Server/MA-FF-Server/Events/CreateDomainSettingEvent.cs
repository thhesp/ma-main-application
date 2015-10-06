using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.Base;
using WebAnalyzer.Models.SettingsModel;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// CreateDomainSettingEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">CreateDomainSettingEvent which is sent</param>
    public delegate void CreateDomainSettingEventHandler(object sender, CreateDomainSettingEvent e);

    /// <summary>
    /// Used for actually saving the newly created domain setting
    /// </summary>
    public class CreateDomainSettingEvent : EventArgs
    {

        /// <summary>
        /// The new domainsetting
        /// </summary>
        private DomainSettings _domain;

        /// <summary>
        /// EventConstructor
        /// </summary>
        /// <param name="domain">The new domainsetting</param>
        public CreateDomainSettingEvent(DomainSettings domain)
        {
            _domain = domain;
        }

        /// <summary>
        /// Getter for the domainsetting
        /// </summary>
        public DomainSettings Domain
        {
            get { return _domain; }
        }
    }
}
