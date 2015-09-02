using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CefSharp;
using CefSharp.WinForms;

using WebAnalyzer.Util;

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
            EvaluteJavaScript("alert('" + message + "');");
        }

        public void EvaluteJavaScript(String script)
        {
            if (Browser != null && script != null)
            {
                Browser.EvaluateScriptAsync(script);
            }
        }

        public void javascriptLog(String message)
        {
            //EvaluteJavaScript("console.log('" + message + "');");
            Logger.JavascriptLog(message);
        }
    }
}
