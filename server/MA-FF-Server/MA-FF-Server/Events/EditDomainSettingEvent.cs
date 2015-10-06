using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// EditDomainSettingEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">EditDomainSettingEvent which is sent</param>
    public delegate void EditDomainSettingEventHandler(object sender, EditDomainSettingEvent e);

    /// <summary>
    /// Event which gets triggered while editing the domain settings
    /// </summary>
    public class EditDomainSettingEvent : EventArgs
    {
        /// <summary>
        /// Different edit types like create, edit, copy and delete
        /// </summary>
        public enum EDIT_TYPES { Create = 0, Edit = 1, Copy = 2, Delete = 3 };

        /// <summary>
        /// UID of the domain setting
        /// </summary>
        private String _uid;

        /// <summary>
        /// Edit type of the event
        /// </summary>
        private EDIT_TYPES _type;

        /// <summary>
        /// Event Constructor
        /// </summary>
        /// <param name="type">Edit type of the event</param>
        /// <param name="uid">UID of the domain setting</param>
        public EditDomainSettingEvent(EDIT_TYPES type, String uid)
        {
            _uid = uid;
            _type = type;
        }

        /// <summary>
        /// EventConstructor used for creating an domain setting
        /// </summary>
        /// 
        public EditDomainSettingEvent()
        {
            _type = EDIT_TYPES.Create;
        }

        /// <summary>
        /// Getter for the domain setting UID
        /// </summary>
        public String UID
        {   
            get {return _uid; }
        }

        /// <summary>
        /// Edit type of the current event
        /// </summary>
        public EDIT_TYPES Type
        {
            get { return _type; }
        }
    }
}
