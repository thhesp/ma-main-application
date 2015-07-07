using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json;

using WebAnalyzer.Util;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class TestObject
    {
        [JavascriptIgnore]
        public ChromiumWebBrowser m_chromeBrowser { get; set; }

        [JavascriptIgnore]
        public void SetChromeBrowser(ChromiumWebBrowser b)
        {
            m_chromeBrowser = b;
        }

        public String tenst()
        {
            return "Tenst";
        }

        public void message(String msg)
        {
            Logger.Log("New Message From Javascript: " + msg);
        }


    }

}
