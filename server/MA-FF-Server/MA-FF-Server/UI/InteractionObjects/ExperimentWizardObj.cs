using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebAnalyzer.Util;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class ExperimentWizardObj : BaseInteractionObject
    {


        public void back()
        {
            Browser.Back();
        }

        public void createExperiment()
        {
            string page = string.Format("{0}UI/HTMLResources/html/popup/experiment_wizard/create_experiment.html", Utilities.GetAppLocation());
            Browser.Load(page);
        }
    }
}
