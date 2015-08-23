using System;
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
                rotateLogs(value);
                _experimentName = value;
            }
        }

        private void rotateLogs()
        {
            //Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + "logs.txt"
            // Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + "javascript-logs.txt"
            int logCount = int.Parse(Properties.Settings.Default.LogCount);
            for (int i = logCount; i >= 0; i--)
            {
                String sourceLogFile = Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + "logs-" + i + ".txt";
                String sourceJavascriptLogFile = Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + "javascript-logs-" + i + ".txt";

                if (i == 0)
                {
                    sourceLogFile = Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + "logs.txt";
                    sourceJavascriptLogFile = Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + "javascript-logs.txt";
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
                    String destLogFile = Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + "logs-" + (i+1) + ".txt";
                    String destJavascriptLogFile = Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + "javascript-logs-" + (i+1) + ".txt";


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
            int logCount = int.Parse(Properties.Settings.Default.LogCount);
            for (int i = logCount; i >= 0; i--)
            {
                String sourceLogFile = Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + experimentName + "-logs-" + i + ".txt";
                String sourceJavascriptLogFile = Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + experimentName + "-javascript-logs-" + i + ".txt";

                if (i == 0)
                {
                    sourceLogFile = Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + experimentName + "-logs.txt";
                    sourceJavascriptLogFile = Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + experimentName + "-javascript-logs.txt";
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
                    String destLogFile = Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + experimentName + "-logs-" + (i + 1) + ".txt";
                    String destJavascriptLogFile = Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + experimentName + "-javascript-logs-" + (i + 1) + ".txt";


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
