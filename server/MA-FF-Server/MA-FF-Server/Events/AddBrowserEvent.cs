using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAnalyzer.Models.EventModel;
using WebAnalyzer.Server;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// AddBrowserEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">AddBrowserEvent which is sent</param>
    public delegate void AddBrowserEventHandler(object sender, AddBrowserEvent e);


    /// <summary>
    /// Contains the data about an event which happend in the browser
    /// </summary>
    public class AddBrowserEvent : EventArgs
    {
        /// <summary>
        /// UID of the websocket which received the event
        /// </summary>
        private String _connectionUID;

        /// <summary>
        /// EventModel which contains the event data.
        /// </summary>
        /// <remarks>
        /// The given object is from an child class of BaseEventModel
        /// </remarks>
        private BaseEventModel _eventModel;


        /// <summary>
        /// Constructor for the event.
        /// </summary>
        /// <param name="eventModel">Contains the data about the event. Object of child class of BaseEventModel.</param>
        /// <param name="connectionUID">UID of the websocket which received the event</param>
        public AddBrowserEvent(BaseEventModel eventModel, String connectionUID)
        {
            _eventModel = eventModel;
            _connectionUID = connectionUID;
        }

        /// <summary>
        /// Getter for the EventModel
        /// </summary>
        public BaseEventModel EventModel
        {
            get { return _eventModel; }
        }

        /// <summary>
        /// Getter for the ConnectionUID
        /// </summary>
        public String ConnectionUID
        {
            get { return _connectionUID; }
        }
    }
}
