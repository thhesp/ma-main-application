using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Server;

namespace WebAnalyzer.Events
{
    public delegate void AddWebpageEventHandler(object sender, AddWebpageEvent e);

    public class AddWebpageEvent : EventArgs
    {

        private String _url;
        private String _timestamp;

        public AddWebpageEvent(String url)
        {
            _url = url;
            _timestamp = Util.Timestamp.GetMillisecondsUnixTimestamp();
        }

        public String URL
        {
            get { return _url; }
        }

        public String Timestamp
        {
            get { return _timestamp; }
        }
    }
}
