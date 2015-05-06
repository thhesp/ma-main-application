using System;
using System.Windows.Forms;

using WebAnalyzer.Test.Communication;

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

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Properties.Settings.Default.Reset();
            }


            //new CommunicationTest();

            new WebAnalyzer();
        }

    }

    

}
