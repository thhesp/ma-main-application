using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    public delegate void AddTestrunDataEventHandler(object sender, AddTestrunDataEvent e);

    public class AddTestrunDataEvent : EventArgs
    {

        private String _label;
        private String _protocol;

        public AddTestrunDataEvent(String label, String protocol)
        {
            _label = label;
            _protocol = protocol;
        }

        public String Label
        {
            get { return _label; }
        }

        public String Protocol
        {
            get { return _protocol; }
        }
    }
}
