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
    public class DomainSettingControl : BaseInteractionObject
    {

        public event CreateDomainSettingEventHandler CreateDomainSetting;

        private EditDomainSettingForm _form;

        private DomainSettings _setting;

        private Boolean _create;

        public DomainSettingControl(EditDomainSettingForm form, DomainSettings setting, Boolean create)
        {
            _form = form;
            _setting = setting;
            _create = create;
        }


        public Boolean creatingNewDomainSetting()
        {
            return _create;
        }

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

        public void cancel()
        {
            _form.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                _form.DialogResult = DialogResult.Abort;
            });
        }

        public String getDomain()
        {
            return _setting.Domain;
        }

        public void setDomain(String domain)
        {
            _setting.Domain = domain;
        }

        public Boolean getIncludeSubdomains()
        {
            return _setting.IncludesSubdomains;
        }

        public void setIncludeSubdomains(Boolean includeSubdomains)
        {
            _setting.IncludesSubdomains = includeSubdomains;
        }

        public String[] getAOIIdentifiers()
        {
            return _setting.GetAOIIdentifiers();
        }

        public String[] getAOIUIDs()
        {
            return _setting.GetAOIUIDs();
        }

        public void editAOI()
        {
            AOISettings aoi = new AOISettings();
            ShowEditAOISettingForm(aoi, true);
        }

        public void createAOI()
        {
            AOISettings aoi = new AOISettings();
            ShowEditAOISettingForm(aoi, true);
        }

        private void ShowEditAOISettingForm(AOISettings setting, Boolean createNew)
        {
            _form.BeginInvoke((Action)delegate
            {
                EditAOISettingForm editSetting = new EditAOISettingForm(setting, createNew);
                editSetting.CreateAOISetting += On_CreateAOISetting;

                if (editSetting.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //refresh participants
                }
            });
        }

        private void On_CreateAOISetting(object source, CreateAOISettingEvent e)
        {
            //currentExperiment.Settings.Domains.Add(e.Domain);

        }
    }
}
