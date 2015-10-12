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
using WebAnalyzer.Models.SettingsModel;

namespace WebAnalyzer.UI
{
    /// <summary>
    /// Window for creating/ editing a domain setting
    /// </summary>
    public partial class EditDomainSettingForm : Form
    {
        /// <summary>
        /// Reference to the chromium browser
        /// </summary>
        private ChromiumWebBrowser myBrowser = null;

        /// <summary>
        /// Reference to the domain setting which is being edited/ created
        /// </summary>
        private DomainSettings _setting = null;

        /// <summary>
        /// Flag if it is being created
        /// </summary>
        private Boolean _create = false;

        /// <summary>
        /// Reference to the domain control interaction object
        /// </summary>
        private DomainSettingControl control = null;

        /// <summary>
        /// Eventhandler for creating a domain setting
        /// </summary>
        public event CreateDomainSettingEventHandler CreateDomainSetting;

        /// <summary>
        /// Eventhandler for triggering a save
        /// </summary>
        public event TriggerSaveEventHandler TriggerSave;

        public EditDomainSettingForm(DomainSettings setting, Boolean create)
        {
            _setting = setting;
            _create = create;
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

            string page = string.Format("{0}UI/HTMLResources/html/popup/settings/domain_edit.html", Utilities.GetAppLocation());
            myBrowser = new ChromiumWebBrowser(page);

            control = new DomainSettingControl(this, _setting, _create);
            control.Browser = myBrowser;
            control.CreateDomainSetting += this.CreateDomainSetting;
            control.TriggerSave += this.TriggerSave;

            myBrowser.RegisterJsObject("control", control);

            myBrowser.Load(page);


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
