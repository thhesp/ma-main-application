using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.UI.InteractionObjects;

namespace WebAnalyzer.Events
{
    public delegate void TestrunEventHandler(BaseInteractionObject sender, TestrunEvent e);

    public class TestrunEvent : EventArgs
    {

        public enum EVENT_TYPE { Create = 0,Start = 1, Stop = 2 };

        private EVENT_TYPE _type;

        public TestrunEvent(EVENT_TYPE type)
        {
            _type = type;
        }

        public EVENT_TYPE Type
        {
            get { return _type; }
        }
    }
}
