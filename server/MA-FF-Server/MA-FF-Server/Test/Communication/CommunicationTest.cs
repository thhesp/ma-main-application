using System.Windows.Forms;

namespace Server.Test.Communication
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
