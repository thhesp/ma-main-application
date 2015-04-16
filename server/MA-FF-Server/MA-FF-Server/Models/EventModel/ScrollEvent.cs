using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.Base;

namespace WebAnalyzer.Models.EventModel
{
    class ScrollEvent : BasicData
    {

        private String _eventTimestamp;
        private int _x;
        private int _y;

        public ScrollEvent(int x, int y, String serverReceivedTimestamp)
        {
            _x = x;
            _y = y;
            _serverReceivedTimestamp = serverReceivedTimestamp;
        }

        #region GetterSetterFunctions

        public int X
        {
            get { return _x; }
            set { _x = value;  }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public String EventTimestamp
        {
            get { return _eventTimestamp; }
            set { _eventTimestamp = value; }
        }

        #endregion
    }
}
