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
    public partial class HTMLUI : Form
    {
        private ChromiumWebBrowser myBrowser = null;
        private ExperimentObject _exp = null;

        public event EditParticipantEventHandler EditParticipant;
        public event EditDomainSettingEventHandler EditDomainSetting;
        public event TestrunEventHandler Testrun;
        public event EditApplicationSettingsEventHandler EditApplicationSetting;

        public HTMLUI()
        {
            InitializeComponent();
        }

        public void SetExperimentData(ExperimentModel experiment)
        {
            if (_exp != null)
            {
                _exp.Experiment = experiment;
            }
        }

        public void SetWSConnectionStatus(ExperimentObject.CONNECTION_STATUS status)
        {
            if (_exp != null)
            {
                _exp.setWSConnectionStatus(status);
            }
        }

        public void SetTrackingConnectionStatus(ExperimentObject.CONNECTION_STATUS status)
        {
            if (_exp != null)
            {
                _exp.setTrackingConnectionStatus(status);
            }
        }

        public void ReloadPage()
        {
            if (myBrowser != null)
            {
                myBrowser.Reload();
                Logger.Log("Reloading???");
            }
        }

        private void CreateBrowser(String page){
            myBrowser = new ChromiumWebBrowser(page);

            if(_exp != null){
                _exp.Browser = myBrowser;
                myBrowser.RegisterJsObject("experimentObj", _exp);
                ReloadPage();
            }
        }

        private void CreateNav(){
            Navigation nav = new Navigation();
            nav.Browser = myBrowser;

            nav.EditParticipant += this.EditParticipant;
            nav.EditDomainSetting += this.EditDomainSetting;
            nav.Testrun += this.Testrun;
            nav.EditApplicationSetting += this.EditApplicationSetting;
            myBrowser.RegisterJsObject("nav", nav);
        }

        private void CreateExperimentObj()
        {
            _exp = new ExperimentObject();
            _exp.Browser = myBrowser;
            myBrowser.RegisterJsObject("experimentObj", _exp);
        }

        private void Browser_Load(object sender, EventArgs e)
        {
            Cef.Initialize();

            string page = string.Format("{0}UI/HTMLResources/html/main/index.html", Utilities.GetAppLocation());
            CreateBrowser(page);

            CreateNav();
            CreateExperimentObj();

            myBrowser.Load(page);
            
            //ChromiumWebBrowser myBrowser = new ChromiumWebBrowser("http://www.maps.google.com");
            this.Controls.Add(myBrowser);

            // chromdev tools
            ChromeDevTools.CreateSysMenu(this);
        }

        private void Browser_Closing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

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
