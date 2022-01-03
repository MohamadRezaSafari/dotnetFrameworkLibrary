using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Providers
{
    public class JWTAuthenticationAttribute : ActionFilterAttribute
    {
        public string Identity { get; set; }

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            string[] data = filterContext.Request.Headers.FirstOrDefault(i => i.Key == Identity).Value.ElementAt(0).ToString().Split(':');
            string _username = data[0].Trim();
            string _token = data[1].Trim();

            if (String.IsNullOrEmpty(_token) || String.IsNullOrEmpty(_username))
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            string result = JWTAuthentication.ValidateToken(_token);

            if (String.IsNullOrEmpty(result))
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    { Content = new StringContent("The username/password combination was wrong.") }; 
            }

            base.OnActionExecuting(filterContext);
        }
    }
}