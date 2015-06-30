using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.SettingsModel
{
    abstract class RuleElement
    {

        protected int _type;

        public RuleElement(int type)
        {
            _type = type;
        }
    }
}
