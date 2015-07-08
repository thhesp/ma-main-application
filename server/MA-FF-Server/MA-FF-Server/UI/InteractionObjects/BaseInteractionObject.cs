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
    public abstract class BaseInteractionObject
    {
        ChromiumWebBrowser _browser = null;


        public ChromiumWebBrowser Browser
        {
            get { return _browser; }
            set { _browser = value; }
        }
    }
}