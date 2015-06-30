using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.DataModel;

namespace WebAnalyzer.Models.SettingsModel
{
    public class SettingsRule
    {

        private String _tag;
        private String _class;
        private String _id;

        private Boolean _caseSensitive;

        private RuleElement[] _expression;

        public String Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public String Class
        {
            get { return _class; }
            set { _class = value; }
        }

        public String ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public Boolean CaseSensitive
        {
            get { return _caseSensitive; }
            set { _caseSensitive = value; }
        }

        public Boolean ElementFitsRule(DOMElementModel el)
        {
            if (CaseSensitive)
            {
                return ElementFitsRuleCaseSensitive(el);
            }
            else
            {
                return ElementFitsRuleCaseInsensitive(el);
            }
        }

        private Boolean ElementFitsRuleCaseSensitive(DOMElementModel el)
        {
            
            if (Tag != null && !Tag.Equals(el.Tag))
            {
                return false;
            }

            if (ID != null && ID != el.ID)
            {
                return false;
            }

            if (Class != null && !el.GetClasses().Contains(Class))
            {
                return false;
            }


            return true;
        }

        private Boolean ElementFitsRuleCaseInsensitive(DOMElementModel el)
        {
            if (Tag != null && Tag.ToLower() != el.Tag.ToLower())
            {
                return false;
            }

            if (ID != null && ID.ToLower() != el.ID.ToLower())
            {
                return false;
            }

            if (Class != null)
            {
                foreach (String elClass in el.GetClasses())
                {
                    if(Class.ToLower() == elClass.ToLower()){
                        return true;
                    }
                }
            }

            return true;
        }

    }
}
