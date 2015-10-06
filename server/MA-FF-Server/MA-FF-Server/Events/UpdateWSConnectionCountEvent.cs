using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// UpdateWSConnectionCountEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">UpdateWSConnectionCountEvent which is sent</param>
    public delegate void UpdateWSConnectionCountEventHandler(object sender, UpdateWSConnectionCountEvent e);

    /// <summary>
    /// Event to update the ws connection count
    /// </summary>
    public class UpdateWSConnectionCountEvent : EventArgs
    {
        /// <summary>
        /// The current number of connections
        /// </summary>
        private int _count;

        /// <summary>
        /// Eventconstructor
        /// </summary>
        /// <param name="count">The current connection count</param>
        public UpdateWSConnectionCountEvent(int count)
        {
            _count = count;
        }

        /// <summary>
        /// Getter for the connection count
        /// </summary>
        public int Count
        {
            get { return _count; }
        }
    }
}
