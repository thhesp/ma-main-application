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

        private EditAOISettingForm _form;

        private AOISettings _setting;

        private Boolean _create;

        public AOISettingControl(EditAOISettingForm form, AOISettings setting, Boolean create)
        {
            _form = form;
            _setting = setting;
            _create = create;
        }


        public Boolean creatingNewDomainSetting()
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
    }
}
