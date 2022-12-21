using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Providers
{
    public class FilterMacAddressAttribute : ActionFilterAttribute
    {
        public string MacAddresses { get; set; }

        /**
         * [FilterMacAddress(MacAddresses = "2C-4D-54-57-C2-6E,2C-4D-54-57-C2-6A")]
         * */
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string macAddress = Network.GetMACAddress();
            var result = IsMacAddressAllowed(macAddress.Trim());

            if (!result)
            {
                context.Result = new HttpStatusCodeResult(403);
            }

            base.OnActionExecuting(context);
        }


        private bool IsMacAddressAllowed(string MacAddress)
        {
            if (!string.IsNullOrWhiteSpace(MacAddress))
            {
                string[] addresses = MacAddresses.Split(',');
                IList<string> M_Addresses = new List<string>();
                foreach (var item in addresses)
                {
                    M_Addresses.Add(item.Replace("-", String.Empty));
                }
                return M_Addresses.Where(a => a.Trim().Equals(MacAddress, StringComparison.InvariantCultureIgnoreCase)).Any();
            }
            else
            {
                return false;
            }
        }
    }
}