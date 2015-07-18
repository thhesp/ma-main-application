using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.Base;
using WebAnalyzer.Models.SettingsModel;

namespace WebAnalyzer.Events
{
    public delegate void CreateRuleEventtHandler(object sender, CreateRuleEvent e);

    public class CreateRuleEvent : EventArgs
    {

        private SettingsRule _rule;

        public CreateRuleEvent(SettingsRule rule)
        {
            _rule = rule;
        }

        public SettingsRule Rule
        {
            get { return _rule; }
        }
    }
}
