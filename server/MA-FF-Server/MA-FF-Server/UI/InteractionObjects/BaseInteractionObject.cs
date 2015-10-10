using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CefSharp;
using CefSharp.WinForms;

using WebAnalyzer.Util;
using System.Windows.Forms;

namespace WebAnalyzer.UI.InteractionObjects
{
    public abstract class BaseInteractionObject
    {
        ChromiumWebBrowser _browser = null;


        public ChromiumWebBrowser Browser
        {
            get { return _browser; }
            set { _browser = value; }
        }

        public void DisplayError(String message)
        {
            //EvaluteJavaScript("alert('" + message + "');");

            MessageBox.Show(message, "Ein Fehler ist aufgetragen");
        }

        public void DisplayError(String message, String title)
        {
            //EvaluteJavaScript("alert('" + message + "');");

            MessageBox.Show(message, title);
        }

        public void DisplaySuccess(String message, String title)
        {
            //EvaluteJavaScript("alert('" + message + "');");

            MessageBox.Show(message, title);
        }

        public Boolean DisplayYesNoQuestion(String message, String title)
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        public void EvaluteJavaScript(String script)
        {
            if (Browser != null && script != null)
            {
                try
                {
                    Browser.EvaluateScriptAsync(script);
                }
                catch (NullReferenceException e)
                {
                    Logger.Log("Problems while executing javascript: " + e.Message);
                }
                
            }
        }

        public void javascriptLog(String message)
        {
            //EvaluteJavaScript("console.log('" + message + "');");
            Logger.JavascriptLog(message);
        }
    }
}
