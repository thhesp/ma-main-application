using System.Windows.Forms;

using WebAnalyzer.Experiment;

namespace WebAnalyzer.Test.Communication
{
    class CommunicationTest
    {

        public CommunicationTest()
        {
            ExperimentController.getInstance().CreateExperiment("mousetracking-test");

            new MouseTrackingInterface();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MouseTracker());

        }
    }
}
