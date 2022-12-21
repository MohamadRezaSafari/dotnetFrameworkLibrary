using System.Web.Helpers;

namespace Providers
{
    public class WebApiRequirement
    {
        public static string CreateCsrfToken()
        {
            string cookieToken, formToken;
            AntiForgery.GetTokens(null, out cookieToken, out formToken);
            return cookieToken + ":" + formToken;
        }
    }
}