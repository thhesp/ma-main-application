using System.Windows.Forms;

namespace WebAnalyzer.Test.Communication
{
    class CommunicationTest
    {

        public CommunicationTest()
        {
            new MouseTrackingInterface();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MouseTracker());

        }
    }
}
