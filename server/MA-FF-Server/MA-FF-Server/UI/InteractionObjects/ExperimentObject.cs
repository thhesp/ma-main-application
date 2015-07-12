using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class ExperimentObject : BaseInteractionObject
    {

        private String _name;

        public ExperimentObject(String name){
            _name = name;
        }

        public String getName(){
            return _name;
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
