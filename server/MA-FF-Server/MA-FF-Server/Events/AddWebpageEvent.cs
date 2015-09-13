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
        private String _connetionUID;
        private String _timestamp;

        public AddWebpageEvent(String url, String connectionUID)
        {
            _url = url;
            _connetionUID = connectionUID;
            _timestamp = Util.Timestamp.GetMillisecondsUnixTimestamp();
        }

        public String URL
        {
            get { return _url; }
        }

        public String ConnectionUID
        {
            get { return _connetionUID; }
        }

        public String Timestamp
        {
            get { return _timestamp; }
        }
    }
}
