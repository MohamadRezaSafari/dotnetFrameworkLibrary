using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Providers
{
    public class ServerSecurity
    {
        /*
         * var x = ServerSecurity.Search(HostingEnvironment.MapPath("~/Upload"));
         */
        public static string[] Search(string targetDirectory)
        {
            var files = Directory.GetFiles(targetDirectory, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".py") || s.EndsWith(".pl") || s.EndsWith(".asp") || s.EndsWith(".aspx") || s.EndsWith(".vb") || s.EndsWith(".php") || s.EndsWith(".zip") || s.EndsWith(".rar"));

            return files.ToArray();
        }


        /*
         * ServerSecurity.DeleteFile(x);
         */
        public static void DeleteFile(string[] target)
        {
            try
            {
                foreach (var item in target)
                {
                    if (File.Exists(item))
                    {
                        File.Delete(item);
                    }
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}