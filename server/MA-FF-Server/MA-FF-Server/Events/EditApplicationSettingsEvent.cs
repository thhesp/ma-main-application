using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    public delegate void EditApplicationSettingsEventHandler(object sender, EditApplicationSettingsEvent e);

    public class EditApplicationSettingsEvent : EventArgs
    {

        public EditApplicationSettingsEvent()
        {
        }
    }
}
