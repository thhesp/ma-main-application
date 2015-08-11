using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    public delegate void TestrunEventHandler(object sender, TestrunEvent e);

    public class TestrunEvent : EventArgs
    {

        public enum EVENT_TYPE { Start = 0, Stop = 1 };

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
