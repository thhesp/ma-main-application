using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WebAnalyzer.UI.InteractionObjects;
using WebAnalyzer.Util;
using WebAnalyzer.Events;
using WebAnalyzer.Models.Base;

namespace WebAnalyzer.UI
{
    /// <summary>
    /// Window for doing a testrun
    /// </summary>
    public partial class TestrunForm : Form
    {
        /// <summary>
        /// Reference to the chromium browser
        /// </summary>
        private ChromiumWebBrowser myBrowser = null;

        /// <summary>
        /// Handler for the starting and stopping the testrun
        /// </summary>
        public event TestrunEventHandler Testrun;

        /// <summary>
        /// Eventhandler for selecting a participant
        /// </summary>
        public event SelectParticipantForTestEventHandler SelectParticipant;

        /// <summary>
        /// Eventhandler for adding data like label and protocol to the testrun
        /// </summary>
        public event AddTestrunDataEventHandler AddTestrunData;

        /// <summary>
        /// Reference to the currently loaded experiment
        /// </summary>
        private ExperimentModel _experiment;

        /// <summary>
        /// Reference to the interaction object TestrunControl
        /// </summary>
        private TestrunControl _control;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="experiment">Currently loaded experiment</param>
        public TestrunForm(ExperimentModel experiment)
        {
            _experiment = experiment;
            InitializeComponent();
        }

        /// <summary>
        /// Callback for when the form loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Browser_Load(object sender, EventArgs e)
        {
            //Cef.Initialize();

            string page = string.Format("{0}UI/HTMLResources/html/popup/testrun/testrun.html", Utilities.GetAppLocation());
            myBrowser = new ChromiumWebBrowser(page);

            _control = new TestrunControl(this);

            _control.Testrun += Testrun;
            _control.SelectParticipant += SelectParticipant;
            _control.AddTestrunData += AddTestrunData;

            _control.Experiment = _experiment;

            _control.Browser = myBrowser;

            myBrowser.RegisterJsObject("control", _control);

            myBrowser.Load(page);
            
            //ChromiumWebBrowser myBrowser = new ChromiumWebBrowser("http://www.maps.google.com");
            this.Controls.Add(myBrowser);

            // chromdev tools
            ChromeDevTools.CreateSysMenu(this);

        }

        /// <summary>
        /// Callback called when the form is closing.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">event data</param>
        private void Browser_Closing(object sender, FormClosingEventArgs e)
        {
            _control.addTestrunData();
            //Cef.Shutdown();
        }

        /// <summary>
        /// Needed for enabling the use of the chromium dev tools
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // Test if the About item was selected from the system menu
            if ((m.Msg == ChromeDevTools.WM_SYSCOMMAND) &&
               ((int)m.WParam == ChromeDevTools.SYSMENU_CHROME_DEV_TOOLS))
            {
                myBrowser.ShowDevTools();
            }
        }

        /// <summary>
        /// Should enable the opening of the chromium dev tools on pressing the F12 key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Currently doesn't work as intended :(
        /// </remarks>
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            Logger.Log("Keydown?");
            if (e.KeyCode == Keys.F12)
            {
                Logger.Log("Show dev tools");
                myBrowser.ShowDevTools();
            }
        }

        /// <summary>
        /// Reloads the page in the browser
        /// </summary>
        public void ReloadPage()
        {
            if (myBrowser != null)
            {
                myBrowser.Reload();
                Logger.Log("Reloading???");
            }
        }
    }

}
