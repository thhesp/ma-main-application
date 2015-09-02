using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    public delegate void SelectExperimentEventHandler(object sender, SelectExperimentEvent e);

    public class SelectExperimentEvent : EventArgs
    {
        private String _path;

        public SelectExperimentEvent(String path)
        {
            _path = path;
        }

        public String Path
        {
            get { return _path; }
        }
    }
}
