using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// SaveTestrunEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">SaveTestrunEvent which is sent</param>
    public delegate void SaveTestrunEventHandler(object sender, SaveTestrunEvent e);

    /// <summary>
    /// Event for saving the testrun
    /// </summary>
    public class SaveTestrunEvent : EventArgs
    {

        /// <summary>
        /// EventConstructor
        /// </summary>
        public SaveTestrunEvent()
        {
        }
    }
}
