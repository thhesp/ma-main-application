using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.SettingsModel.ExpressionTree
{
    public class ValueNode : Node
    {

        public static enum TYPES { Tag, Class, ID };

        private String _value;
        private TYPES _type;

        public ValueNode(TYPES type, String value) : base()
        {
            _type = type;
            _value = value;
        }

        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public TYPES Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public override bool Evaluate(DOMElementModel el)
        {
            String loweredValue = Value.ToLower();
            if (TYPES.Tag == Type)
            {
                return el.Tag.ToLower() == loweredValue;
            }
            else if (TYPES.ID == Type)
            {
                return  el.ID.ToLower() == loweredValue;
            }
            else if (TYPES.Class == Type)
            {
                return el.GetClasses().Contains(loweredValue, StringComparer.OrdinalIgnoreCase);
            }

            return false;
        }

        public override bool EvaluateCaseSensitive(DOMElementModel el)
        {
            if (TYPES.Tag == Type)
            {
                return el.Tag == Value;
            }
            else if (TYPES.ID == Type)
            {
                return  el.ID == Value;
            }
            else if (TYPES.Class == Type)
            {
                return el.GetClasses().Contains(Value);
            }

            return false;
        }

    }
}
