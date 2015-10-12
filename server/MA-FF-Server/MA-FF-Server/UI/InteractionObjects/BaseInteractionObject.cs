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
    /// <summary>
    /// Base class for all interaction objects
    /// </summary>
    public abstract class BaseInteractionObject
    {
        /// <summary>
        /// Reference to the chromium browser of the window
        /// </summary>
        ChromiumWebBrowser _browser = null;

        /// <summary>
        /// Getter / Setter for the chromium browser
        /// </summary>
        public ChromiumWebBrowser Browser
        {
            get { return _browser; }
            set { _browser = value; }
        }

        /// <summary>
        /// Method used for displaying a error message
        /// </summary>
        /// <param name="message">The message to display</param>
        public void DisplayError(String message)
        {
            //EvaluteJavaScript("alert('" + message + "');");

            MessageBox.Show(message, "Ein Fehler ist aufgetragen");
        }

        /// <summary>
        /// Method used for displaying a error message
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="title">The title of the message box</param>
        public void DisplayError(String message, String title)
        {
            //EvaluteJavaScript("alert('" + message + "');");

            MessageBox.Show(message, title);
        }

        /// <summary>
        /// Method used for display a success message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="title">Title of the message box</param>
        public void DisplaySuccess(String message, String title)
        {
            //EvaluteJavaScript("alert('" + message + "');");

            MessageBox.Show(message, title);
        }

        /// <summary>
        /// Method for displaying a yes no question box
        /// </summary>
        /// <param name="message">Message of the box</param>
        /// <param name="title">Title of the box</param>
        /// <returns>Returns true if the user answers "Yes", False if "No"</returns>
        public Boolean DisplayYesNoQuestion(String message, String title)
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        /// <summary>
        /// Executes the given javascript in the browser
        /// </summary>
        /// <param name="script">JavaScript Code to execute</param>
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

        /// <summary>
        /// Used for logging to the javascript log file
        /// </summary>
        /// <param name="message"></param>
        public void javascriptLog(String message)
        {
            //EvaluteJavaScript("console.log('" + message + "');");
            Logger.JavascriptLog(message);
        }
    }
}
