﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TarahiOnline.Models.v1;

namespace Providers
{
    public class UniqueOrderNumberAttribute : ActionFilterAttribute
    {
        private DbContextv1 _db = new DbContextv1();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            long orderNumber = Convert.ToInt64(filterContext.HttpContext.Request.Form["OrderNumber"]);
            var paymentDataGateway = _db.PaymentDataGateways.FirstOrDefault(i => i.OrderNumber == orderNumber);

            if (paymentDataGateway != null)
            {
                filterContext.Controller.TempData["fail"] = "مشکلی پیش امده است لطفا دوباره سعی کنید";
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}