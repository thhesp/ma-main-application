﻿using CefSharp;
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
    public partial class SelectExperimentForm : Form
    {
        public event SelectExperimentEventHandler SelectExperiment;

        private ChromiumWebBrowser myBrowser = null;

        private String _path = "";

        public SelectExperimentForm()
        {
            InitializeComponent();
        }

        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }

        private void Browser_Load(object sender, EventArgs e)
        {
            //Cef.Initialize();

            string page = string.Format("{0}UI/HTMLResources/html/popup/experiment_wizard/select_experiment.html", Utilities.GetAppLocation());
            myBrowser = new ChromiumWebBrowser(page);

            SelectExperimentControl control = new SelectExperimentControl(this);

            control.SelectExperiment += SelectExperiment;

            control.Browser = myBrowser;

            myBrowser.RegisterJsObject("control", control);

            myBrowser.Load(page);


            this.Controls.Add(myBrowser);

            // chromdev tools
            ChromeDevTools.CreateSysMenu(this);

        }

        private void Browser_Closing(object sender, FormClosingEventArgs e)
        {

            //Cef.Shutdown();
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
