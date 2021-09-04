using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Providers
{
    public class IdentityRoleAccess : ActionFilterAttribute
    {
        public string Role { get; set; }
        public string Message { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = HttpContext.Current.User.IsInRole(Role);

            if (user == false)
            {
                filterContext.Controller.TempData["fail"] = Message;
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = Controller, action = Action }));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}