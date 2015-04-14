using System.Windows.Forms;

using WebAnalyzer.Experiment;
using WebAnalyzer.UI;

namespace WebAnalyzer
{
    class WebAnalyzer
    {

        public WebAnalyzer()
        {
            ExperimentController.getInstance().CreateExperiment("mousetracking-test");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestForm());
        }
    }
}
