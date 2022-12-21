using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace Providers
{
    public class Log
    {
        private static string FolderName = HostingEnvironment.MapPath("~/Log");
        private static string LogFileName = "log.txt";
        private static string pathString = null;

        /**
         *  Log.Save(Rand.GUID())
         */
        public static void Save(string Text)
        {
            if (!Directory.Exists(FolderName))
            {
                Directory.CreateDirectory(FolderName);
            }

            pathString = Path.Combine(FolderName, LogFileName);

            if (!File.Exists(pathString))
            {
                File.Create(pathString).Dispose();
            }

            File.AppendAllText(pathString, Text + Environment.NewLine);
        }
    }
}