using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Models.EventModel;
using WebAnalyzer.Server;

namespace WebAnalyzer.Events
{
    public delegate void AddBrowserEventHandler(object sender, AddBrowserEvent e);

    public class AddBrowserEvent : EventArgs
    {
        private String _connectionUID;
        private BaseEventModel _eventModel;

        public AddBrowserEvent(BaseEventModel eventModel, String connectionUID)
        {
            _eventModel = eventModel;
            _connectionUID = connectionUID;
        }

        public BaseEventModel EventModel
        {
            get { return _eventModel; }
        }

        public String ConnectionUID
        {
            get { return _connectionUID; }
        }
    }
}
