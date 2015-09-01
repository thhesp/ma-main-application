using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    public delegate void SaveTestrunEventHandler(object sender, SaveTestrunEvent e);

    public class SaveTestrunEvent : EventArgs
    {

        public SaveTestrunEvent()
        {
        }
    }
}
