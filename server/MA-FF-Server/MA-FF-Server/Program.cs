using System;
using System.Windows.Forms;

using WebAnalyzer.Test.Communication;
using WebAnalyzer.Experiment;

namespace WebAnalyzer
{


    // Testing the WebSocket Server

    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new TestForm());
            Application.Run(new MouseTracker());*/

            ExperimentController.getInstance().CreateExperiment("test");

            new CommunicationTest();



        }

    }

    

}
