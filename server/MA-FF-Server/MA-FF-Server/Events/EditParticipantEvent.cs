﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// EditParticipantEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">EditParticipantEvent which is sent</param>
    public delegate void EditParticipantEventHandler(object sender, EditParticipantEvent e);

    /// <summary>
    /// Event for editing participant
    /// </summary>
    public class EditParticipantEvent : EventArgs
    {

        /// <summary>
        /// Different possible Edit types
        /// </summary>
        public enum EDIT_TYPES { Create = 0, Edit = 1, Copy = 2, Delete = 3 };

        /// <summary>
        /// UID of the participant
        /// </summary>
        private String _uid;

        /// <summary>
        /// Edit type of the event
        /// </summary>
        private EDIT_TYPES _type;

        /// <summary>
        /// EventConstructor
        /// </summary>
        /// <param name="type">Edit type of the event</param>
        /// <param name="uid">UID of the participant</param>
        public EditParticipantEvent(EDIT_TYPES type, String uid)
        {
            _uid = uid;
            _type = type;
        }

        /// <summary>
        /// EventConstructor used for creating an participant
        /// </summary>
        public EditParticipantEvent()
        {
            _type = EDIT_TYPES.Create;
        }

        /// <summary>
        /// 
        /// </summary>
        public String UID
        {   
            get {return _uid; }
        }

        public EDIT_TYPES Type
        {
            get { return _type; }
        }
    }
}
