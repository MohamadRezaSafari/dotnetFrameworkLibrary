using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Providers
{
    public class UserAgent
    {
        public static string Detect()
        {
            return HttpContext.Current.Request.UserAgent;
        }
    }
}