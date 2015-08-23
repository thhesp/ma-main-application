using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Util
{
    public static class FileIO
    {
        public static void CheckPath(String dir)
        {
            CheckPath(dir, false);
        }

        public static void CheckPath(String dir, bool invisble)
        {
            bool exists = Directory.Exists(dir);
            if (!exists)
            {
                DirectoryInfo di = Directory.CreateDirectory(dir);
                if (invisble)
                {
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }
            }
        }
    }
}
