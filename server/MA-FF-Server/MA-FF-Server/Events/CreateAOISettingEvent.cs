using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Models.Base;
using WebAnalyzer.Models.SettingsModel;

namespace WebAnalyzer.Events
{
    public delegate void CreateAOISettingEventHandler(object sender, CreateAOISettingEvent e);

    public class CreateAOISettingEvent : EventArgs
    {

        private AOISettings _aoi;

        public CreateAOISettingEvent(AOISettings aoi)
        {
            _aoi = aoi;
        }

        public AOISettings AOI
        {
            get { return _aoi; }
        }
    }
}
