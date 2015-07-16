using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    public delegate void TriggerSaveEventHandler(object sender, TriggerSaveEvent e);

    public class TriggerSaveEvent : EventArgs
    {

        public enum SAVE_TYPES { ALL = 0, PARTICIPANTS = 1, SETTINGS = 2 };

        private SAVE_TYPES _type;

        public TriggerSaveEvent(SAVE_TYPES type)
        {
            _type = type;
        }

        public SAVE_TYPES Type
        {
            get { return _type; }
        }
    }
}
