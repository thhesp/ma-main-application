using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAnalyzer.Util
{
    /// <summary>
    /// Class for code which is used for file control. Mainly for checking if a path exists.
    /// </summary>
    public static class FileIO
    {

        /// <summary>
        /// Checks if a path exists and creates if it does not exists.
        /// </summary>
        /// <param name="dir">Location which should be checked</param>
        public static void CheckPathAndCreate(String dir)
        {
            CheckPathAndCreate(dir, false);
        }

        /// <summary>
        /// Class for code which is used for file control. Mainly for checking if a path exists.
        /// </summary>
        /// <param name="dir">Location which should be checked</param>
        /// <param name="invisible">If the location does not exists, should it be created as a invisible Directory.</param>
        public static void CheckPathAndCreate(String dir, bool invisible)
        {
            bool exists = Directory.Exists(dir);
            if (!exists)
            {
                DirectoryInfo di = Directory.CreateDirectory(dir);
                if (invisible)
                {
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }
            }
        }
    }
}
