using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// AddTestrunDataEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">AddTestrunDataEvent which is sent</param>
    public delegate void AddTestrunDataEventHandler(object sender, AddTestrunDataEvent e);

    /// <summary>
    /// Used for adding data to an testrun
    /// </summary>
    public class AddTestrunDataEvent : EventArgs
    {

        /// <summary>
        /// Label of the testrun
        /// </summary>
        private String _label;

        /// <summary>
        /// Protocol of the testrun
        /// </summary>
        private String _protocol;

        /// <summary>
        /// EventConstructor
        /// </summary>
        /// <param name="label">Label of the testrun</param>
        /// <param name="protocol">Protocol of the testrun</param>
        public AddTestrunDataEvent(String label, String protocol)
        {
            _label = label;
            _protocol = protocol;
        }

        /// <summary>
        /// Getter for the label of the testrun
        /// </summary>
        public String Label
        {
            get { return _label; }
        }

        /// <summary>
        /// Getter for the protocol of the testrun
        /// </summary>
        public String Protocol
        {
            get { return _protocol; }
        }
    }
}
