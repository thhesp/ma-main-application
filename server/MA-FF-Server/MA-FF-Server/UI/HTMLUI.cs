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

namespace WebAnalyzer.UI
{
    public partial class HTMLUI : Form
    {
        ChromiumWebBrowser myBrowser = null;

        public HTMLUI()
        {
            InitializeComponent();
        }

        private void Browser_Load(object sender, EventArgs e)
        {
            Cef.Initialize();

            string page = string.Format("{0}UI/HTMLResources/html/main/index.html", Utilities.GetAppLocation());
            myBrowser = new ChromiumWebBrowser(page);

            TestObject test = new TestObject();
            test.SetChromeBrowser(myBrowser);

            Navigation nav = new Navigation();
            nav.Browser = myBrowser;

            ExperimentObject exp = new ExperimentObject("test experiment");
            

            // Register the JavaScriptInteractionObj class with JS
            myBrowser.RegisterJsObject("testObj", test);

            myBrowser.RegisterJsObject("nav", nav);

            myBrowser.RegisterJsObject("experimentObj", exp);

            
            Console.WriteLine(page);

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
