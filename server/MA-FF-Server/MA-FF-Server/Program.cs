using System;
using System.Windows.Forms;

using WebAnalyzer.Test.Communication;
using WebAnalyzer.UI;
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
            //new CommunicationTest();

            new WebAnalyzer();
        }

    }

    

}
