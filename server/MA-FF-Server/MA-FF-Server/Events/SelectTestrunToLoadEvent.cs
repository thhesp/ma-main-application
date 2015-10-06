using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// SelectTestrunToLoadEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">SelectTestrunToLoadEvent which is sent</param>
    public delegate void SelectTestrunToLoadEventHandler(object sender, SelectTestrunToLoadEvent e);

    /// <summary>
    /// Event which is used for loading testrun data
    /// </summary>
    public class SelectTestrunToLoadEvent : EventArgs
    {

        /// <summary>
        /// Path to the directory in which the testrun is contained
        /// </summary>
        private String _path;

        /// <summary>
        /// Timestamp on which the testrun was created (to identify the correct file)
        /// </summary>
        private String _created;

        /// <summary>
        /// Event-Constructor
        /// </summary>
        /// <param name="created">Timestamp on which the testrun was created (to identify the correct file)</param>
        /// <param name="path">Path to the directory in which the testrun is contained</param>
        public SelectTestrunToLoadEvent(String created, String path)
        {
            _path = path;
            _created = created;
        }

        /// <summary>
        /// Getter for the path to the directory in which the testrun is contained
        /// </summary>
        public String Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Getter for the Timestamp on which the testrun was created (to identify the correct file)
        /// </summary>
        public String Created
        {
            get { return _created; }
        }
    }
}
