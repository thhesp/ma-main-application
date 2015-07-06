using System.Windows.Forms;

using WebAnalyzer.Experiment;
using WebAnalyzer.UI;

namespace WebAnalyzer.Test.UI
{
    class HTMLUITest
    {

        public HTMLUITest()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HTMLUI());

        }
    }
}
