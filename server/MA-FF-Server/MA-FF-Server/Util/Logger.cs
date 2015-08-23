﻿using System;
using System.Runtime.CompilerServices;
using System.IO;

namespace WebAnalyzer.Util
{
    class Logger
    {

        public static void Log(String line, [CallerFilePathAttribute]string filePath = "", [CallerMemberName]string memberName = "")
        {
            getInstance().Log(Path.GetFileName(filePath) + " - " + memberName + ";" + Timestamp.GetUnixTimestamp() + ": " + line);
        }

        public static void JavascriptLog(String line, [CallerFilePathAttribute]string filePath = "", [CallerMemberName]string memberName = "")
        {
            getInstance().JavascriptLog(Path.GetFileName(filePath) + " - " + memberName + ";" + Timestamp.GetUnixTimestamp() + ": " + line);
        }

        public static Logger getInstance()
        {
            if (_instance == null)
            {
                _instance = new Logger();
            }

            return _instance;
        }

        private static Logger _instance;

        private String _experimentName = null;

        private Logger()
        {
            checkLogLocation();
            //add logrotation
        }

        public String ExperimentName
        {
            get { return _experimentName; }
            set { _experimentName = value; }
        }

        public void checkLogLocation(){
            FileIO.CheckPath(Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation, true);
        }

        private void Log(String message)
        {

            if (_experimentName != null)
            {
                using (StreamWriter w = File.AppendText(Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + ExperimentName + "-logs.txt"))
                {
                    w.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter w = File.AppendText(Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + "logs.txt"))
                {
                    w.WriteLine(message);
                }
            }


            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine(message);
            }
        }

        private void JavascriptLog(String message)
        {

            if (_experimentName != null)
            {
                using (StreamWriter w = File.AppendText(Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + ExperimentName + "-javascript-logs.txt"))
                {
                    w.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter w = File.AppendText(Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + "javascript-logs.txt"))
                {
                    w.WriteLine(message);
                }
            }

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine(message);
            }
        }


    }
}
