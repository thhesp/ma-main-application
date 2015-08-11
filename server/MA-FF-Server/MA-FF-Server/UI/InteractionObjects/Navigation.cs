using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CefSharp;
using CefSharp.WinForms;

using WebAnalyzer.Util;
using WebAnalyzer.Events;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class Navigation : BaseInteractionObject
    {

        private static String BASE_PATH = "{0}UI/HTMLResources/html/main/";

        public event EditParticipantEventHandler EditParticipant;
        public event EditDomainSettingEventHandler EditDomainSetting;
        public event TestrunEventHandler Testrun;

        Dictionary<string, string> _pages = new Dictionary<string, string>();

        private String _activePage = null;

        public Navigation()
        {
            CreatePages();
            _activePage = _pages.Keys.ElementAt(0);
        }

        private void CreatePages()
        {
            _pages.Add("overview", "index.html");
            _pages.Add("analyse", "analyse.html");
            _pages.Add("experiment_settings", "experiment_settings.html");
            _pages.Add("export", "export.html");
            _pages.Add("participants", "participants.html");
            
        }

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

        public String getActivePage()
        {
            return _activePage;
        }

        public void createParticipant()
        {
            Logger.Log("Create participant?");
            EditParticipant(this, new EditParticipantEvent(EditParticipantEvent.EDIT_TYPES.Create));
        }

        public void editParticipant(String uid){
            EditParticipant(this, new EditParticipantEvent(EditParticipantEvent.EDIT_TYPES.Edit, uid));
        }

        public void copyParticipant(String uid)
        {
            EditParticipant(this, new EditParticipantEvent(EditParticipantEvent.EDIT_TYPES.Copy, uid));
        }

        public void deleteParticipant(String uid)
        {
            EditParticipant(this, new EditParticipantEvent(EditParticipantEvent.EDIT_TYPES.Delete, uid));
        }

        public void createDomainSetting()
        {
            Logger.Log("Create domain setting?");
            EditDomainSetting(this, new EditDomainSettingEvent(EditDomainSettingEvent.EDIT_TYPES.Create));
        }

        public void editDomainSetting(String uid)
        {
            EditDomainSetting(this, new EditDomainSettingEvent(EditDomainSettingEvent.EDIT_TYPES.Edit, uid));
        }

        public void copyDomainSetting(String uid)
        {
            EditDomainSetting(this, new EditDomainSettingEvent(EditDomainSettingEvent.EDIT_TYPES.Copy, uid));
        }

        public void deleteDomainSetting(String uid)
        {
            EditDomainSetting(this, new EditDomainSettingEvent(EditDomainSettingEvent.EDIT_TYPES.Delete, uid));
        }

        public void startTestrun()
        {
            Testrun(this, new TestrunEvent(TestrunEvent.EVENT_TYPE.Start));
        }

        public void stopTestrun()
        {
            Testrun(this, new TestrunEvent(TestrunEvent.EVENT_TYPE.Stop));
        }
    }
}
