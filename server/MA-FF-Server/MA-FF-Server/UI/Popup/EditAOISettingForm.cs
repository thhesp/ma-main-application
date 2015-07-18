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
using WebAnalyzer.Models.SettingsModel;

namespace WebAnalyzer.UI
{
    public partial class EditAOISettingForm : Form
    {
        private ChromiumWebBrowser myBrowser = null;
        private AOISettings _setting = null;
        private Boolean _create = false;


        private AOISettingControl control = null;

        public event CreateAOISettingEventHandler CreateAOISetting;
        public event TriggerSaveEventHandler TriggerSave;

        public EditAOISettingForm(AOISettings setting, Boolean create)
        {
            _setting = setting;
            _create = create;
            InitializeComponent();
        }

        private void Browser_Load(object sender, EventArgs e)
        {
            //Cef.Initialize();

            string page = string.Format("{0}UI/HTMLResources/html/popup/settings/aoi_edit.html", Utilities.GetAppLocation());
            myBrowser = new ChromiumWebBrowser(page);

            control = new AOISettingControl(this, _setting, _create);
            control.Browser = myBrowser;
            control.CreateAOISetting += this.CreateAOISetting;
            control.TriggerSave += this.TriggerSave;

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
