using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Util
{
    public static class FileIO
    {
        public static void CheckPath(String dir)
        {
            bool exists = System.IO.Directory.Exists(dir);

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(dir);
            }
        }
    }
}
