﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CefSharp;
using CefSharp.WinForms;

using WebAnalyzer.Util;

namespace WebAnalyzer.UI.InteractionObjects
{
    public class Navigation
    {

        private static String BASE_PATH = "{0}UI/HTMLResources/html/main/";

        ChromiumWebBrowser _browser = null;
        Dictionary<string, string> _pages = new Dictionary<string, string>();

        public Navigation()
        {
            CreatePages();
        }

        private void CreatePages()
        {
            _pages.Add("overview", "index.html");
            _pages.Add("analyse", "analyse.html");
            _pages.Add("experiment_settings", "experiment_settings.html");
            _pages.Add("export", "export.html");
            _pages.Add("participants", "participants.html");
            
        }

        public void showPage(String page)
        {
            Logger.Log("Show page: " + page);
            if (Browser != null && _pages.ContainsKey(page))
            {
                String file = _pages[page];
                String pagePath = string.Format(BASE_PATH + file, Utilities.GetAppLocation());

                Browser.Load(pagePath);
            }
            
        }

        public ChromiumWebBrowser Browser
        {
            get { return _browser; }
            set { _browser = value; }
        }
    }
}
