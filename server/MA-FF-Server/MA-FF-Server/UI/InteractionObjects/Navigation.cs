using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CefSharp;
using CefSharp.WinForms;

using WebAnalyzer.Util;
using WebAnalyzer.Events;
using System.Windows.Forms;

namespace WebAnalyzer.UI.InteractionObjects
{
    /// <summary>
    /// Interaction object for the navigation in the main ui
    /// </summary>
    public class Navigation : BaseInteractionObject
    {
        /// <summary>
        /// Base path to the different screens of the main ui
        /// </summary>
        private static String BASE_PATH = "{0}UI/HTMLResources/html/main/";

        /// <summary>
        /// Eventhandler for editing/ creating a participant
        /// </summary>
        public event EditParticipantEventHandler EditParticipant;

        /// <summary>
        /// Eventhandler for editing/ creating a domain setting
        /// </summary>
        public event EditDomainSettingEventHandler EditDomainSetting;

        /// <summary>
        /// Eventhandler for creating a testrun
        /// </summary>
        public event TestrunEventHandler Testrun;

        /// <summary>
        /// Eventhandler for editing the application settings
        /// </summary>
        public event EditApplicationSettingsEventHandler EditApplicationSetting;

        /// <summary>
        /// dictionary of the pages and the corresponding html files
        /// </summary>
        Dictionary<string, string> _pages = new Dictionary<string, string>();

        /// <summary>
        /// Currently active page
        /// </summary>
        private String _activePage = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public Navigation()
        {
            CreatePages();
            _activePage = _pages.Keys.ElementAt(0);
        }

        /// <summary>
        /// Creates the dictionary with the data about all pages
        /// </summary>
        private void CreatePages()
        {
            _pages.Add("overview", "index.html");
            _pages.Add("analyse", "analyse.html");
            _pages.Add("experiment_settings", "experiment_settings.html");
            _pages.Add("export", "export.html");
            _pages.Add("participants", "participants.html");
            
        }

        /// <summary>
        /// Used for changing to another page
        /// </summary>
        /// <param name="page">The page to display</param>
        /// <remarks>Called from javascript</remarks>
        public void showPage(String page)
        {
            Logger.Log("Show page: " + page);
            if (Browser != null && _pages.ContainsKey(page))
            {
                _activePage = page;
                String file = _pages[page];
                String pagePath = string.Format(BASE_PATH + file, Utilities.GetAppLocation());

                Browser.Load(pagePath);
            }
            
        }

        /// <summary>
        /// Returns the currently active page
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String getActivePage()
        {
            return _activePage;
        }

        /// <summary>
        /// Used for creating a participant window
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void createParticipant()
        {
            Logger.Log("Create participant?");
            EditParticipant(this, new EditParticipantEvent());
        }

        /// <summary>
        /// Used for editing a participant
        /// </summary>
        /// <param name="uid">UID of the participant</param>
        /// <remarks>Called from javascript</remarks>
        public void editParticipant(String uid){
            EditParticipant(this, new EditParticipantEvent(EditParticipantEvent.EDIT_TYPES.Edit, uid));
        }

        /// <summary>
        /// Used for copying a participant
        /// </summary>
        /// <param name="uid">UID of the participant</param>
        /// <remarks>Called from javascript</remarks>
        public void copyParticipant(String uid)
        {
            EditParticipant(this, new EditParticipantEvent(EditParticipantEvent.EDIT_TYPES.Copy, uid));
        }

        /// <summary>
        /// Used for deleting a participant
        /// </summary>
        /// <param name="uid">UID of the participant</param>
        /// <remarks>Called from javascript</remarks>
        public void deleteParticipant(String uid)
        {
            EditParticipant(this, new EditParticipantEvent(EditParticipantEvent.EDIT_TYPES.Delete, uid));
        }

        /// <summary>
        /// Used for creating a domain setting
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void createDomainSetting()
        {
            Logger.Log("Create domain setting?");
            EditDomainSetting(this, new EditDomainSettingEvent());
        }

        /// <summary>
        /// Used for editing a domain setting
        /// </summary>
        /// <param name="uid">UID of the domain setting</param>
        /// <remarks>Called from javascript</remarks>
        public void editDomainSetting(String uid)
        {
            EditDomainSetting(this, new EditDomainSettingEvent(EditDomainSettingEvent.EDIT_TYPES.Edit, uid));
        }

        /// <summary>
        /// Used for copying a domain setting
        /// </summary>
        /// <param name="uid">UID of the domain setting</param>
        /// <remarks>Called from javascript</remarks>
        public void copyDomainSetting(String uid)
        {
            EditDomainSetting(this, new EditDomainSettingEvent(EditDomainSettingEvent.EDIT_TYPES.Copy, uid));
        }

        /// <summary>
        /// Used for delting a domain setting
        /// </summary>
        /// <param name="uid">UID of the domain setting</param>
        /// <remarks>Called from javascript</remarks>
        public void deleteDomainSetting(String uid)
        {
            EditDomainSetting(this, new EditDomainSettingEvent(EditDomainSettingEvent.EDIT_TYPES.Delete, uid));
        }

        /// <summary>
        /// Used for creating a testrun
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void createTestrun()
        {
            Testrun(this, new TestrunEvent(TestrunEvent.EVENT_TYPE.Create));
        }

        /// <summary>
        /// Used for editing the application settings
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void showApplicationSettings()
        {
            Logger.Log("Show application settings");
            EditApplicationSetting(this, new EditApplicationSettingsEvent());
        }
    }
}
