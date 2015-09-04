using System;
using System.Runtime.CompilerServices;
using System.IO;

namespace WebAnalyzer.Util
{
    /// <summary>
    /// Class for logging purposes.
    /// </summary>
    class Logger
    {

        /// <summary>
        /// Should be used for normal logging.
        /// </summary>
        /// <param name="line">Log message</param>
        /// <param name="filePath">Don't use this parameter. Automatically gets filled with the name of the class/file the log call comes from.</param>
        /// <param name="memberName">Don't use this parameter. Automatically gets filled with the name of the function the log call comes from.</param>
        public static void Log(String line, [CallerFilePathAttribute]string filePath = "", [CallerMemberName]string memberName = "")
        {
            getInstance().Log(Path.GetFileName(filePath) + " - " + memberName + ";" + Timestamp.GetUnixTimestamp() + ": " + line);
        }

        /// <summary>
        /// Should be used for logging the javascript data from the ui
        /// </summary>
        /// <param name="line">Log message</param>
        /// <param name="filePath">Don't use this parameter. Automatically gets filled with the name of the class/file the log call comes from.</param>
        /// <param name="memberName">Don't use this parameter. Automatically gets filled with the name of the function the log call comes from.</param>
        public static void JavascriptLog(String line, [CallerFilePathAttribute]string filePath = "", [CallerMemberName]string memberName = "")
        {
            getInstance().JavascriptLog(Path.GetFileName(filePath) + " - " + memberName + ";" + Timestamp.GetUnixTimestamp() + ": " + line);
        }

        /// <summary>
        /// Returns instance of Logger class
        /// </summary>
        
        public static Logger getInstance()
        {
            if (_instance == null)
            {
                _instance = new Logger();
            }

            return _instance;
        }

        /// <summary>
        /// Logger class instace for reusing.
        /// </summary>
        private static Logger _instance;

        /// <summary>
        /// Name of the current experiment. Used for assign logs to an experiment.
        /// </summary>
        private String _experimentName = null;

        /// <summary>
        /// Logger Constructor. Private so it can only be used with the instance.
        /// </summary>
        /// <remarks>
        /// When creating a new Logger instance the log location will be checked and the logs will be rotated.
        /// </remarks>
        private Logger()
        {
            checkLogLocation();
            rotateLogs();
        }

        /// <summary>
        /// Acces to the ExperimentName, so that it can be set, when an Experiment gets loaded.
        /// </summary>
        /// <remarks>
        /// On setting the name the location will get checked if it exists and logs will be rotated.
        /// </remarks>
        public String ExperimentName
        {
            get { return _experimentName; }
            set {
                checkExperimentLogLocation(value);
                rotateLogs(value);
                _experimentName = value;
            }
        }

        /// <summary>
        /// Rotates the normal logs not assigned to an experiment.
        /// </summary>
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

        /// <summary>
        /// Rotates the log files assigned to an experiment.
        /// </summary>
        /// <param name="experimentName">Name of the experiment for which the logs should be rotated</param>
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

        /// <summary>
        /// Checks the normal log location. If it does not exist, it gets generated invisible.
        /// </summary>
        private void checkLogLocation(){
            FileIO.CheckPathAndCreate(Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation, true);
        }

        /// <summary>
        /// Checks the experiment log location. If it does not exist, it gets generated invisible.
        /// </summary>
        /// <param name="experimentName">Name of the experiment to which the location belongs.</param>
        private void checkExperimentLogLocation(String experimentName)
        {
            FileIO.CheckPathAndCreate(Properties.Settings.Default.Datalocation + experimentName + "\\" + Properties.Settings.Default.LogsLocation, true);
        }

        /// <summary>
        /// The "real" logging method. Which writes the logging message to the log file.
        /// If an experiment name is set, the experiment log file will be used.
        /// </summary>
        /// <remarks>If an Debugger is attached all logs will also be written to the console.</remarks>
        /// <param name="message">The message which should be logged.</param>
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

        /// <summary>
        /// The "real" logging method. Which writes the logging message to the javascript log file.
        /// If an experiment name is set, the experiment log file will be used.
        /// </summary>
        /// <remarks>If an Debugger is attached all logs will also be written to the console.</remarks>
        /// <param name="message">The message which should be logged.</param>
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
