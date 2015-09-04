using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    public delegate void SelectTestrunToLoadEventHandler(object sender, SelectTestrunToLoadEvent e);

    public class SelectTestrunToLoadEvent : EventArgs
    {

        private String _path;
        private String _created;

        public SelectTestrunToLoadEvent(String created, String path)
        {
            _path = path;
            _created = created;
        }

        public String Path
        {
            get { return _path; }
        }

        public String Created
        {
            get { return _created; }
        }
    }
}
