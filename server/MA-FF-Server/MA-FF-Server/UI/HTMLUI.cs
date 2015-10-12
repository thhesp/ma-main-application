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
    /// The main UI window of the application
    /// </summary>
    public partial class HTMLUI : Form
    {

        /// <summary>
        /// Reference to the chromium browser
        /// </summary>
        private ChromiumWebBrowser myBrowser = null;

        /// <summary>
        /// Reference to the interaction object representing the experiment
        /// </summary>
        private ExperimentObject _exp = null;

        /// <summary>
        /// Reference to the interaction object for analysis and export
        /// </summary>
        private AnalysisExportControl _analysisExportControl = null;

        /// <summary>
        /// EventHandler for editing participants
        /// </summary>
        public event EditParticipantEventHandler EditParticipant;

        /// <summary>
        /// Eventhandler for editing domain settings
        /// </summary>
        public event EditDomainSettingEventHandler EditDomainSetting;

        /// <summary>
        /// Eventhandler for creating a testrun
        /// </summary>
        public event TestrunEventHandler Testrun;

        /// <summary>
        /// Eventhandler for opening the application settings
        /// </summary>
        public event EditApplicationSettingsEventHandler EditApplicationSetting;

        /// <summary>
        /// Eventhandler for triggering saves to the xml
        /// </summary>
        public event TriggerSaveEventHandler TriggerSave;


        /// <summary>
        /// Constructor of the main UI
        /// </summary>
        public HTMLUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the data about the loaded experiment
        /// </summary>
        /// <param name="experiment">The loaded experiment</param>
        public void SetExperimentData(ExperimentModel experiment)
        {
            if (_exp != null)
            {
                _exp.Experiment = experiment;
            }

            if (_analysisExportControl != null)
            {
                _analysisExportControl.Experiment = experiment;
            }
        }

        /// <summary>
        /// Sets the connection status of the websocket server
        /// </summary>
        /// <param name="status">Current status of the websocket server</param>
        public void SetWSConnectionStatus(ExperimentObject.CONNECTION_STATUS status)
        {
            if (_exp != null)
            {
                _exp.SetWSConnectionStatus(status);
            }
        }

        /// <summary>
        /// Sets the connection status of the tracking component
        /// </summary>
        /// <param name="status">Current status of the tracking component</param>
        public void SetTrackingConnectionStatus(ExperimentObject.CONNECTION_STATUS status)
        {
            if (_exp != null)
            {
                _exp.SetTrackingConnectionStatus(status);
            }
        }

        /// <summary>
        /// Sets the websocket connection count
        /// </summary>
        /// <param name="count">Count of open websocket connections</param>
        public void SetWSConnectionCount(int count)
        {
            if (_exp != null)
            {
                _exp.SetWSConnectionCount(count);
            }
        }

        /// <summary>
        /// Refreshes the data in the experiment object
        /// </summary>
        public void RefreshData()
        {

            if (_exp != null && myBrowser != null && !myBrowser.IsLoading && !myBrowser.IsLoading)
            {
                _exp.RefreshData();
                Logger.Log("Refresh Data..");
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
                Logger.Log("Reloading Page");
            }
        }


        /// <summary>
        /// Creates the Browser and opens the given page
        /// </summary>
        /// <param name="page">Page to display in the browser</param>
        private void CreateBrowser(String page){
            myBrowser = new ChromiumWebBrowser(page);

            if(_exp != null){
                _exp.Browser = myBrowser;
                myBrowser.RegisterJsObject("experimentObj", _exp);
                ReloadPage();
            }
        }

        /// <summary>
        /// Creates the interaction Object "navigation".
        /// </summary>
        private void CreateNav(){
            Navigation nav = new Navigation();
            nav.Browser = myBrowser;

            nav.EditParticipant += this.EditParticipant;
            nav.EditDomainSetting += this.EditDomainSetting;
            nav.Testrun += this.Testrun;
            nav.EditApplicationSetting += this.EditApplicationSetting;
            myBrowser.RegisterJsObject("nav", nav);
        }

        /// <summary>
        /// Creates the interaction object for analysis and export
        /// </summary>
        private void CreateAnalysisAndExportObj()
        {
            _analysisExportControl = new AnalysisExportControl(this);

            _analysisExportControl.Browser = myBrowser;

            myBrowser.RegisterJsObject("analysisExportControl", _analysisExportControl);
        }

        /// <summary>
        /// Creates the interaction object for the experiment
        /// </summary>
        private void CreateExperimentObj()
        {
            _exp = new ExperimentObject(this);
            _exp.Browser = myBrowser;
            _exp.TriggerSave += TriggerSave;
            myBrowser.RegisterJsObject("experimentObj", _exp);
        }

        /// <summary>
        /// Callback for when the form loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Browser_Load(object sender, EventArgs e)
        {
            Cef.Initialize();

            string page = string.Format("{0}UI/HTMLResources/html/main/index.html", Utilities.GetAppLocation());

            Logger.Log("UI Location: " + page);

            CreateBrowser(page);

            CreateNav();
            CreateExperimentObj();
            CreateAnalysisAndExportObj();

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
            Cef.Shutdown();
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
    }
}
