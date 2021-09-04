using System;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;


namespace Providers
{
    public class AjaxCSRF : System.Web.Mvc.ActionFilterAttribute
    {
        public string Header { get; set; }

        /*
        @section Scripts {
            <script>
                @functions{
                    public string TokenHeaderValue()
                    {
                        string cookieToken, formToken;
                        AntiForgery.GetTokens(null, out cookieToken, out formToken);
                        return cookieToken + ":" + formToken;
                    }
                }
            </script>
        }  
        */

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool Result = ValidateRequestHeader(filterContext.HttpContext.Request.Headers[Header]);

            if (Result == false)
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Error" }));

            base.OnActionExecuting(filterContext);
        }


        private bool ValidateRequestHeader(string csrf)
        {
            string cookieToken = "";
            string formToken = "";

            if (!String.IsNullOrEmpty(csrf))
            {
                string[] tokens = csrf.Split(':');
                if (tokens.Length == 2)
                {
                    cookieToken = tokens[0].Trim();
                    formToken = tokens[1].Trim();
                }
            }

            try
            {
                AntiForgery.Validate(cookieToken, formToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}