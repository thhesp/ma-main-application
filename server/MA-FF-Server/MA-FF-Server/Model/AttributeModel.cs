using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Model
{
    class AttributeModel
    {
        private String _name;
        private String _value;

        public AttributeModel(String name, String value)
        {
            _name = name;
            _value = value;
        }

        public String Name
        {
            get { return _name; }
            set { _name = value;  }
        }


        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
