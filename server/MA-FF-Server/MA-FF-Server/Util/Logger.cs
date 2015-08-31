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
            rotateLogs();
        }

        public String ExperimentName
        {
            get { return _experimentName; }
            set {
                checkExperimentLogLocation(value);
                rotateLogs(value);
                _experimentName = value;
            }
        }

        private void rotateLogs()
        {
            //Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + "logs.txt"
            // Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + "javascript-logs.txt"
            int logCount = Properties.Settings.Default.LogCount;

            String baseDir = Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation;

            for (int i = logCount; i >= 0; i--)
            {
                String sourceLogFile = baseDir + "logs-" + i + ".txt";
                String sourceJavascriptLogFile = baseDir + "javascript-logs-" + i + ".txt";

                if (i == 0)
                {
                    sourceLogFile = baseDir + "logs.txt";
                    sourceJavascriptLogFile = baseDir + "javascript-logs.txt";
                }

                if (i == logCount)
                {
                    //delete
                    if (File.Exists(sourceLogFile))
                    {
                        File.Delete(sourceLogFile);
                    }

                    if (File.Exists(sourceJavascriptLogFile))
                    {
                        File.Delete(sourceJavascriptLogFile);
                    }

                }
                else
                {
                    String destLogFile = baseDir + "logs-" + (i + 1) + ".txt";
                    String destJavascriptLogFile = baseDir + "javascript-logs-" + (i + 1) + ".txt";


                    if (File.Exists(sourceLogFile))
                    {
                        File.Move(sourceLogFile, destLogFile);
                    }

                    if (File.Exists(sourceJavascriptLogFile))
                    {
                        File.Move(sourceJavascriptLogFile, destJavascriptLogFile);
                    }
                }
            }
        }

        private void rotateLogs(String experimentName)
        {
            //Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + experimentName +"-logs.txt"
            // Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + experimentName + "-javascript-logs.txt"
            int logCount = Properties.Settings.Default.LogCount;

            String baseDir = Properties.Settings.Default.Datalocation + experimentName + "\\" + Properties.Settings.Default.LogsLocation;

            for (int i = logCount; i >= 0; i--)
            {
                String sourceLogFile = baseDir + "logs-" + i + ".txt";
                String sourceJavascriptLogFile = baseDir + "javascript-logs-" + i + ".txt";

                if (i == 0)
                {
                    sourceLogFile = baseDir + "logs.txt";
                    sourceJavascriptLogFile = baseDir + "javascript-logs.txt";
                }



                if (i == logCount)
                {
                    //delete
                    if (File.Exists(sourceLogFile))
                    {
                        File.Delete(sourceLogFile);
                    }

                    if (File.Exists(sourceJavascriptLogFile))
                    {
                        File.Delete(sourceJavascriptLogFile);
                    }

                }
                else
                {
                    String destLogFile = baseDir + "logs-" + (i + 1) + ".txt";
                    String destJavascriptLogFile = baseDir + "javascript-logs-" + (i + 1) + ".txt";


                    if (File.Exists(sourceLogFile))
                    {
                        File.Move(sourceLogFile, destLogFile);
                    }

                    if (File.Exists(sourceJavascriptLogFile))
                    {
                        File.Move(sourceJavascriptLogFile, destJavascriptLogFile);
                    }
                }
            }

        }

        private void checkLogLocation(){
            FileIO.CheckPath(Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation, true);
        }

        private void checkExperimentLogLocation(String experimentName)
        {
            FileIO.CheckPath(Properties.Settings.Default.Datalocation + experimentName + "\\" + Properties.Settings.Default.LogsLocation, true);
        }

        private void Log(String message)
        {
            String filename = Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + "logs.txt";

            if (_experimentName != null)
            {
                filename = Properties.Settings.Default.Datalocation + ExperimentName + "\\" + Properties.Settings.Default.LogsLocation + "logs.txt";
            }

            try
            {
                using (StreamWriter w = File.AppendText(filename))
                {
                    w.WriteLine(message);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }


            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine(message);
            }
        }

        private void JavascriptLog(String message)
        {

            String filename = Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + "javascript-logs.txt";
            if (_experimentName != null)
            {
                filename = Properties.Settings.Default.Datalocation + ExperimentName + "\\" + Properties.Settings.Default.LogsLocation + "javascript-logs.txt";
            }


            try
            {
                using (StreamWriter w = File.AppendText(filename))
                {
                    w.WriteLine(message);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine(message);
            }
        }


    }
}
