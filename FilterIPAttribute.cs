using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Providers
{
    public class FilterIPAttribute : ActionFilterAttribute
    {
        public string IPs { get; set; }

        /*
         *  [FilterIP(IPs = "127.0.0.1")]    // allowed ips
         */
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string ipAddress = Network.GetIPAddress();
            var result = IsIpAddressAllowed(ipAddress.Trim());

            if (!result)
            {
                context.Result = new HttpStatusCodeResult(403);
            }

            base.OnActionExecuting(context);
        }


        private bool IsIpAddressAllowed(string IpAddress)
        {
            if (!string.IsNullOrWhiteSpace(IpAddress))
            {
                string[] addresses = IPs.Split(',');
                return addresses.Where(a => a.Trim().Equals(IpAddress, StringComparison.InvariantCultureIgnoreCase)).Any();
            }
            else
            {
                return false;
            }
        }
    }
}
 