using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// EditApplicationSettingsEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">EditApplicationSettingsEvent which is sent</param>
    public delegate void EditApplicationSettingsEventHandler(object sender, EditApplicationSettingsEvent e);

    /// <summary>
    /// Event for editing the application settings (opens the necessary window)
    /// </summary>
    public class EditApplicationSettingsEvent : EventArgs
    {
        
        /// <summary>
        /// EventConstructor
        /// </summary>
        public EditApplicationSettingsEvent()
        {
        }
    }
}
