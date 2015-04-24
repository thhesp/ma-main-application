using System;

namespace WebAnalyzer.Util
{
    class Logger
    {

        public static void Log(String line)
        {
            Console.WriteLine(Timestamp.GetUnixTimestamp() + ": " + line);
        }
    }
}
