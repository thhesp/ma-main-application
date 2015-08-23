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

        public static Logger getInstance()
        {
            if (_instance == null)
            {
                _instance = new Logger();
            }

            return _instance;
        }

        private static Logger _instance;

        private Logger()
        {
            checkLogLocation();
        }

        public void checkLogLocation(){
            FileIO.CheckPath(Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation, true);
        }

        private void Log(String message)
        {
            using (StreamWriter w = File.AppendText(Properties.Settings.Default.Datalocation + Properties.Settings.Default.LogsLocation + "logs.txt"))
            {
                w.WriteLine(message);
            }
        }


    }
}
