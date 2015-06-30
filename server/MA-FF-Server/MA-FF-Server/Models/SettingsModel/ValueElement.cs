using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Models.SettingsModel
{
    public class ValueElement : RuleElement
    {
        public static enum TYPES { Tag, Class, ID };

        private String _value;

        public ValueElement(int type, String value) : base(type)
        {
            _value = value;
        }
    }
}
