using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using TarahiOnline.Controllers;

namespace Providers
{
    public class FTP
    {
        private static string username = "dleonlin";
        private static string password = "rX0v12fr9U";

        

        // FTP.Send("~/Upload/", "last-ned.png");
        public static bool Send(string path, string name)
        {
            try
            {
                WebClient wc = new WebClient();
                Uri uriadd = new Uri(@"ftp://hn92.mylittledatacenter.com/public_html/files/" + RequirementsController.PersianYear() + "/" + name);
                wc.Credentials = new NetworkCredential(username, password);
                wc.UploadFile(uriadd, HostingEnvironment.MapPath(path + name));

                return true;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}