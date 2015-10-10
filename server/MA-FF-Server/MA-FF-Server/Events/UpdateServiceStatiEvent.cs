using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// UpdateServiceStatiEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">UpdateServiceStatiEvent which is sent</param>
    public delegate void UpdateServiceStatiEventHandler(object sender, UpdateServiceStatiEvent e);

    /// <summary>
    /// Event to update the status of the services
    /// </summary>
    public class UpdateServiceStatiEvent : EventArgs
    {
    }
}
