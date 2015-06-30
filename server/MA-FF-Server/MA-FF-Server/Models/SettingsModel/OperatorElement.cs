using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.SettingsModel
{
    public class OperatorElement : RuleElement
    {
        public static enum OPERATOR_TYPES { And, Or, Not };

        public OperatorElement(int type) : base(type)
        {

        }

    }
}
