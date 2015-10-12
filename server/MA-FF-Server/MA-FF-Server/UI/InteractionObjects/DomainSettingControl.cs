using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.SettingsModel;
using System.Windows.Forms;

using WebAnalyzer.Events;
using WebAnalyzer.Util;

namespace WebAnalyzer.UI.InteractionObjects
{
    /// <summary>
    /// Interaction object for editing/ creating domain settings
    /// </summary>
    public class DomainSettingControl : BaseInteractionObject
    {

        /// <summary>
        /// Eventhandler for creating a new domain settings
        /// </summary>
        public event CreateDomainSettingEventHandler CreateDomainSetting;

        /// <summary>
        /// Eventhandler for triggering saves to xml
        /// </summary>
        public event TriggerSaveEventHandler TriggerSave;

        /// <summary>
        /// Reference to the edit domain form
        /// </summary>
        private EditDomainSettingForm _form;


        /// <summary>
        /// Reference to the domain settings which are being edited or created
        /// </summary>
        private DomainSettings _setting;

        /// <summary>
        /// Flag if it is a new domain setting
        /// </summary>
        private Boolean _create;

        /// <summary>
        /// Flag if refreshing is necessary
        /// </summary>
        private Boolean _refreshing = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">Reference to the edit domain form</param>
        /// <param name="setting">Reference to the domain setting</param>
        /// <param name="create">Flag if a new domain setting is being created</param>
        public DomainSettingControl(EditDomainSettingForm form, DomainSettings setting, Boolean create)
        {
            _form = form;
            _setting = setting;
            _create = create;
        }

        /// <summary>
        /// Returns if a new domain setting is being created
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public Boolean creatingNewDomainSetting()
        {
            return _create;
        }

        /// <summary>
        /// Saves the domain setting
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void saveDomainSetting()
        {
            if (_create)
            {
                CreateDomainSetting(this, new CreateDomainSettingEvent(_setting));
            }

            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.OK;
            });
        }

        /// <summary>
        /// Used for canceling the process
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void cancel()
        {
            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.Abort;
            });
        }

        /// <summary>
        /// Returns the current domain identifier
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String getDomain()
        {
            return _setting.Domain;
        }

        /// <summary>
        /// Used for updating the domain
        /// </summary>
        /// <param name="domain">The new domain</param>
        /// <remarks>Called from javascript</remarks>
        public void setDomain(String domain)
        {
            if (!domain.Contains("http://") && !domain.Contains("https://"))
            {
                domain = "http://"+domain;
            }

            _setting.Domain = domain;
        }

        /// <summary>
        /// Returns if the setting includes subdomains or not
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public Boolean getIncludeSubdomains()
        {
            return _setting.IncludesSubdomains;
        }

        /// <summary>
        /// Update the setting if it includes subdomains or not
        /// </summary>
        /// <param name="includeSubdomains">New value</param>
        /// <remarks>Called from javascript</remarks>
        public void setIncludeSubdomains(Boolean includeSubdomains)
        {
            _setting.IncludesSubdomains = includeSubdomains;
        }

        /// <summary>
        /// Returns an array of aoi setting identifiers
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String[] getAOIIdentifiers()
        {
            return _setting.GetAOIIdentifiers();
        }

        /// <summary>
        /// Returns an array of aoi setting uids
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String[] getAOIUIDs()
        {
            return _setting.GetAOIUIDs();
        }

        /// <summary>
        /// Used for editing an aoi setting
        /// </summary>
        /// <param name="uid">UID of the aoi setting</param>
        /// <remarks>Called from javascript</remarks>
        public void editAOI(String uid)
        {
            AOISettings aoi = _setting.GetAOISettingByUid(uid);
            ShowEditAOISettingForm(aoi, false);
        }

        /// <summary>
        /// Create a new aoi
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void createAOI()
        {
            AOISettings aoi = new AOISettings();
            ShowEditAOISettingForm(aoi, true);
        }

        /// <summary>
        /// Used for copying an aoi setting
        /// </summary>
        /// <param name="uid">UID of aoi setting</param>
        /// <remarks>Called from javascript</remarks>
        public void copyAOI(String uid)
        {
            AOISettings aoi = _setting.GetAOISettingByUid(uid);

            AOISettings copy = AOISettings.Copy(aoi);

            _setting.AddAOI(copy);

            refreshData();
            TriggerSave(this, new TriggerSaveEvent(TriggerSaveEvent.SAVE_TYPES.SETTINGS));
        }

        /// <summary>
        /// Used for deleting an aoi setting
        /// </summary>
        /// <param name="uid">UID of aoi setting</param>
        /// <remarks>Called from javascript</remarks>
        public void deleteAOI(String uid)
        {
            AOISettings aoi = _setting.GetAOISettingByUid(uid);
            _setting.AOIS.Remove(aoi);

            refreshData();
            TriggerSave(this, new TriggerSaveEvent(TriggerSaveEvent.SAVE_TYPES.SETTINGS));
        }

        /// <summary>
        /// Opens the edit aoi setting window
        /// </summary>
        /// <param name="setting">Reference to the AOI setting</param>
        /// <param name="createNew">Flag if its a new setting or not</param>
        private void ShowEditAOISettingForm(AOISettings setting, Boolean createNew)
        {
            _form.BeginInvoke((Action)delegate
            {
                EditAOISettingForm editSetting = new EditAOISettingForm(setting, createNew);
                editSetting.CreateAOISetting += On_CreateAOISetting;
                editSetting.TriggerSave += TriggerSave;

                if (editSetting.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    refreshData();
                    TriggerSave(this, new TriggerSaveEvent(TriggerSaveEvent.SAVE_TYPES.SETTINGS));
                }
            });
        }

        /// <summary>
        /// Callback for when a new aoi was created
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void On_CreateAOISetting(object source, CreateAOISettingEvent e)
        {
            _setting.AddAOI(e.AOI);
        }

        /// <summary>
        /// Checks if the page is refreshing
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public Boolean isRefreshing()
        {
            return _refreshing;
        }

        /// <summary>
        /// Refreshes the data on the page
        /// </summary>
        private void refreshData(){
            _refreshing = true;

            _form.ReloadPage();
        }
    }
}
