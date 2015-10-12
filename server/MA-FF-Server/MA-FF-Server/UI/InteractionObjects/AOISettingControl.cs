using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.SettingsModel;
using System.Windows.Forms;

using WebAnalyzer.Events;

namespace WebAnalyzer.UI.InteractionObjects
{
    /// <summary>
    /// Interaction object for editing/ creating aoi settings
    /// </summary>
    public class AOISettingControl : BaseInteractionObject
    {
        /// <summary>
        /// Eventhandler for creating a new aoi
        /// </summary>
        public event CreateAOISettingEventHandler CreateAOISetting;

        /// <summary>
        /// Eventhandler for triggering a save to xml
        /// </summary>
        public event TriggerSaveEventHandler TriggerSave;

        /// <summary>
        /// Reference to the edit aoi window
        /// </summary>
        private EditAOISettingForm _form;

        /// <summary>
        /// Reference to the aoi setting
        /// </summary>
        private AOISettings _setting;

        /// <summary>
        /// Are new aoi settings being created
        /// </summary>
        private Boolean _create;

        /// <summary>
        /// Flag if data needs to be refreshed
        /// </summary>
        private Boolean _refreshing = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">Reference to the edit aoi setting form</param>
        /// <param name="setting">Reference to the aoi setting, which is being edited or created</param>
        /// <param name="create">Flag if it is being created</param>
        public AOISettingControl(EditAOISettingForm form, AOISettings setting, Boolean create)
        {
            _form = form;
            _setting = setting;
            _create = create;
        }

        /// <summary>
        /// Returns if the aoi setting is being created
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public Boolean creatingNewAOISetting()
        {
            return _create;
        }

        /// <summary>
        /// Used for saving the aoi setting
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void saveAOISetting()
        {
            if (_create)
            {
                CreateAOISetting(this, new CreateAOISettingEvent(_setting));
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
        /// Returns the identifier of the aoi
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String getIdentifier()
        {
            return _setting.Identifier;
        }

        /// <summary>
        /// Used for updating the aoi identifier
        /// </summary>
        /// <param name="identifier">The new identifier</param>
        /// <remarks>Called from javascript</remarks>
        public void setIdentifier(String identifier)
        {
            _setting.Identifier = identifier;
        }

        /// <summary>
        /// Returns an array of setting rule uids
        /// </summary>
        /// <returns></returns>
        /// <remarks>Called from javascript</remarks>
        public String[] getRuleUIDs()
        {
            return _setting.GetRuleUIDs();
        }


        /// <summary>
        /// Used for editing an setting rule
        /// </summary>
        /// <param name="uid">UID of the setting rule</param>
        /// <remarks>Called from javascript</remarks>
        public void editRule(String uid)
        {
            SettingsRule rule = _setting.GetRuleSettingByUid(uid);
            ShowEditRuleSettingForm(rule, false);
        }

        /// <summary>
        /// Used for creating a new setting rule
        /// </summary>
        /// <remarks>Called from javascript</remarks>
        public void createRule()
        {
            SettingsRule rule = new SettingsRule();
            ShowEditRuleSettingForm(rule, true);
        }

        /// <summary>
        /// Used for copying a setting rule
        /// </summary>
        /// <param name="uid">UID of setting rule</param>
        /// <remarks>Called from javascript</remarks>
        public void copyRule(String uid)
        {
            SettingsRule rule = _setting.GetRuleSettingByUid(uid);

            SettingsRule copy = SettingsRule.Copy(rule);

            _setting.AddRule(copy);

            refreshData();
            TriggerSave(this, new TriggerSaveEvent(TriggerSaveEvent.SAVE_TYPES.SETTINGS));
        }

        /// <summary>
        /// Used for deleting a setting rule
        /// </summary>
        /// <param name="uid">UID of setting rule</param>
        /// <remarks>Called from javascript</remarks>
        public void deleteRule(String uid)
        {
            SettingsRule rule = _setting.GetRuleSettingByUid(uid);
            _setting.Rules.Remove(rule);

            refreshData();
            TriggerSave(this, new TriggerSaveEvent(TriggerSaveEvent.SAVE_TYPES.SETTINGS));
        }


        /// <summary>
        /// Displays the setting rule window
        /// </summary>
        /// <param name="rule">The setting rule</param>
        /// <param name="createNew">Is the rule new</param>
        private void ShowEditRuleSettingForm(SettingsRule rule, Boolean createNew)
        {
            _form.BeginInvoke((Action)delegate
            {
                EditRuleForm editRule = new EditRuleForm(rule, createNew);
                editRule.CreateRule += On_CreateRule;

                if (editRule.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //refresh aois
                    refreshData();
                    TriggerSave(this, new TriggerSaveEvent(TriggerSaveEvent.SAVE_TYPES.SETTINGS));
                }
            });
        }

        /// <summary>
        /// Callback when a rule is being created
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void On_CreateRule(object source, CreateRuleEvent e)
        {
            _setting.AddRule(e.Rule);
        }

        /// <summary>
        /// Does the data on the page need to be refreshed
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
        private void refreshData()
        {
            _refreshing = true;

            _form.ReloadPage();
        }
    }
}
