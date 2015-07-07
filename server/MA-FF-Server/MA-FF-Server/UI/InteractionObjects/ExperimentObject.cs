using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class ExperimentObject
    {

        private String _name;

        public ExperimentObject(String name){
            _name = name;
        }

        public String getName(){
            return _name;
        }
    }
}
