using System;

namespace WebAnalyzer.Util
{
    class Logger
    {

        public static void Log(String line)
        {
            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyy hh:mm:ss.fff ") + line);
        }
    }
}
