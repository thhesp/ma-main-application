using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.Base;
using WebAnalyzer.Models.SettingsModel;

namespace WebAnalyzer.Events
{
    /// <summary>
    /// CreateAOISettingEventHandler for sending the event
    /// </summary>
    /// <param name="sender">Sender from which the Event gets triggered</param>
    /// <param name="e">CreateAOISettingEvent which is sent</param>
    public delegate void CreateAOISettingEventHandler(object sender, CreateAOISettingEvent e);

    /// <summary>
    /// Used for saving new AOI Settings
    /// </summary>
    public class CreateAOISettingEvent : EventArgs
    {

        /// <summary>
        /// The aoi settings to be saved
        /// </summary>
        private AOISettings _aoi;


        /// <summary>
        /// EventConstructor
        /// </summary>
        /// <param name="aoi">The new aoi settings</param>
        public CreateAOISettingEvent(AOISettings aoi)
        {
            _aoi = aoi;
        }

        /// <summary>
        /// Getter for the aoi settings
        /// </summary>
        public AOISettings AOI
        {
            get { return _aoi; }
        }
    }
}
