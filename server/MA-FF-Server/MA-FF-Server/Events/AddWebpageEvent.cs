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
        private String _connectionUID;
        private String _timestamp;

        private int _windowWidth = 0;
        private int _windowHeight = 0;

        public AddWebpageEvent(String url, int windowWidth, int windowHeight, String connectionUID)
        {
            _url = url;
            _connectionUID = connectionUID;
            _windowHeight = windowHeight;
            _windowWidth = windowWidth;
            _timestamp = Util.Timestamp.GetMillisecondsUnixTimestamp();
        }

        public String URL
        {
            get { return _url; }
        }

        public String ConnectionUID
        {
            get { return _connectionUID; }
        }

        public String Timestamp
        {
            get { return _timestamp; }
        }

        public int WindowWidth
        {
            get { return _windowWidth; }
        }

        public int WindowHeight
        {
            get { return _windowHeight; }
        }
    }
}
