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
    public class AOISettingControl : BaseInteractionObject
    {

        public event CreateAOISettingEventHandler CreateAOISetting;
        public event TriggerSaveEventHandler TriggerSave;

        private EditAOISettingForm _form;

        private AOISettings _setting;

        private Boolean _create;

        public AOISettingControl(EditAOISettingForm form, AOISettings setting, Boolean create)
        {
            _form = form;
            _setting = setting;
            _create = create;
        }


        public Boolean creatingNewAOISetting()
        {
            return _create;
        }

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

        public void cancel()
        {
            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.Abort;
            });
        }

        public String getIdentifier()
        {
            return _setting.Identifier;
        }

        public void setIdentifier(String identifier)
        {
            _setting.Identifier = identifier;
        }


        public void editRule(String uid)
        {
            SettingsRule rule = _setting.GetRuleSettingByUid(uid);
            ShowEditRuleSettingForm(rule, false);
        }

        public void createRule()
        {
            SettingsRule rule = new SettingsRule();
            ShowEditRuleSettingForm(rule, true);
        }

        public void copyRule(String uid)
        {
            SettingsRule rule = _setting.GetRuleSettingByUid(uid);

            SettingsRule copy = SettingsRule.Copy(rule);

            _setting.AddRule(copy);

            _form.ReloadPage();
            TriggerSave(this, new TriggerSaveEvent(TriggerSaveEvent.SAVE_TYPES.SETTINGS));
        }

        public void deleteRule(String uid)
        {
            SettingsRule rule = _setting.GetRuleSettingByUid(uid);
            _setting.Rules.Remove(rule);

            _form.ReloadPage();
            TriggerSave(this, new TriggerSaveEvent(TriggerSaveEvent.SAVE_TYPES.SETTINGS));
        }

        private void ShowEditRuleSettingForm(SettingsRule rule, Boolean createNew)
        {
            _form.BeginInvoke((Action)delegate
            {
                EditRuleForm editRule = new EditRuleForm(rule, createNew);
                editRule.CreateRule += On_CreateRule;

                if (editRule.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //refresh aois
                    _form.ReloadPage();
                    TriggerSave(this, new TriggerSaveEvent(TriggerSaveEvent.SAVE_TYPES.SETTINGS));
                }
            });
        }

        private void On_CreateRule(object source, CreateRuleEvent e)
        {
            _setting.AddRule(e.Rule);
        }
    }
}
