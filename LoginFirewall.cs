using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;

namespace Providers
{
    public class LoginFirewallSet : ActionFilterAttribute
    {
        public int Minutes { get; set; }

        private readonly Cookie _cookie = new Cookie();
        private readonly Encryption _encryption = new Encryption();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!_cookie.ExistCookie("login"))
            {
                var _enc = _encryption.Encrypt(filterContext.HttpContext.Request.UserHostAddress);
                _cookie.SetCookie("login", _enc, "min", Minutes);
            }

            base.OnActionExecuting(filterContext);
        }
    }


    public class LoginFirewall : ActionFilterAttribute
    {
        private readonly Cookie _cookie = new Cookie();
        private readonly Encryption _encryption = new Encryption();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_cookie.ExistCookie("login"))
            {
                bool _valid =  Validate(_cookie.GetCookie("login"), filterContext.HttpContext.Request.UserHostAddress);
                if (String.IsNullOrEmpty(_cookie.GetCookie("login")) || _valid == false)
                {
                    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.OnActionExecuting(filterContext);
        }


        private bool Validate(string cookie, string ip)
        {
            try
            {
                var _enc = _encryption.Decrypt(cookie);
                return (_enc == ip) ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}