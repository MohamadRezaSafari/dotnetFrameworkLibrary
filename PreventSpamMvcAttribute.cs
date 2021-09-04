using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;

namespace Providers
{
    public class PreventSpamMvcAttribute : ActionFilterAttribute
    {
        public int DelayRequest = 60;
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ErrorMessage { get; set; }


        // [PreventSpamMvc(DelayRequest = 60, ErrorMessage = "You can only create a new widget every 60 seconds.")]

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var cache = filterContext.HttpContext.Cache;
            var originationInfo = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;
            originationInfo += request.UserAgent;
            var targetInfo = request.RawUrl + request.QueryString;
            var hashValue = string.Join("", SHA384.Create().ComputeHash(Encoding.ASCII.GetBytes(originationInfo + targetInfo)).Select(s => s.ToString("x2")));
            ErrorMessage = String.Format("شما تنها در {0} ثانیه 1 بار مجاز به انجام این کار هستید", DelayRequest);

            if (cache[hashValue] != null)
            {
                filterContext.Controller.TempData["fail"] = ErrorMessage;
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = Controller, action = Action }));
            }
            else
            {
                cache.Add(hashValue, true, null, DateTime.Now.AddSeconds(DelayRequest), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}