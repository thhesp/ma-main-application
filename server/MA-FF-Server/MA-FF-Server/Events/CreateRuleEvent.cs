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
    /// CreateRuleEventtHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">CreateRuleEvent which is sent</param>
    public delegate void CreateRuleEventtHandler(object sender, CreateRuleEvent e);

    /// <summary>
    /// Used for saving the newly created setting rule
    /// </summary>
    public class CreateRuleEvent : EventArgs
    {

        /// <summary>
        /// The Setting rule
        /// </summary>
        private SettingsRule _rule;

        /// <summary>
        /// EventConstructor
        /// </summary>
        /// <param name="rule">The Setting rule</param>
        public CreateRuleEvent(SettingsRule rule)
        {
            _rule = rule;
        }

        /// <summary>
        /// Getter for the setting rule
        /// </summary>
        public SettingsRule Rule
        {
            get { return _rule; }
        }
    }
}
