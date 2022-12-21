using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Providers
{
    /**
     *  @using Providers
     *  @Html.GoogleCaptcha()
        @Html.InvalidGoogleCaptchaLabel("Captcha is not valid !")

        [ValidateGoogleCaptcha]
     * 
     * */
    public static class SiteSettings
    {
        public const string GoogleRecaptchaSecretKey = "6LcfK3QUAAAAAABTk33CCatFbkqvNIeaaadG2SzB"; //6LfimroUAAAAAPo9UPc2kVSUS5RG_ILTpxbWYXj_
        public const string GoogleRecaptchaSiteKey = "6LcfK3QUAAAAAEhvomSJ3A36S5tIlMISXM-0O3OI"; //6LfimroUAAAAAFsn4SSjKL1nRK7LBP1leTRoTb1d
    }


    public static class GoogleCaptchaHelper
    {
        public static IHtmlString GoogleCaptcha(this HtmlHelper helper)
        {
            const string publicSiteKey = SiteSettings.GoogleRecaptchaSiteKey;

            var mvcHtmlString = new TagBuilder("div")
            {
                Attributes =
            {
                new KeyValuePair<string, string>("class", "g-recaptcha"),
                new KeyValuePair<string, string>("style", "transform:scale(0.77);-webkit-transform:scale(0.77);transform-origin:0 0;-webkit-transform-origin:0 0;"),
                new KeyValuePair<string, string>("data-sitekey", publicSiteKey)
            }
            };

            const string googleCaptchaScript = "<script src='https://www.google.com/recaptcha/api.js?hl=fa'></script>";
            var renderedCaptcha = mvcHtmlString.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create($"{googleCaptchaScript}{renderedCaptcha}");
        }
    }

    
    public static class InvalidGoogleCaptchaHelper
    {
        public static IHtmlString InvalidGoogleCaptchaLabel(this HtmlHelper helper, string errorText)
        {
            var invalidCaptchaObj = helper.ViewContext.Controller.TempData["InvalidCaptcha"];

            var invalidCaptcha = invalidCaptchaObj?.ToString();
            if (string.IsNullOrWhiteSpace(invalidCaptcha)) return MvcHtmlString.Create("");

            var buttonTag = new TagBuilder("span")
            {
                Attributes =
            {
                new KeyValuePair<string, string>("class", "text text-danger")
            },
                InnerHtml = errorText ?? invalidCaptcha
            };

            return MvcHtmlString.Create(buttonTag.ToString(TagRenderMode.Normal));
        }
    }

    public class ValidateGoogleCaptchaAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            const string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            const string secretKey = SiteSettings.GoogleRecaptchaSecretKey;
            var captchaResponse = filterContext.HttpContext.Request.Form["g-recaptcha-response"];

            if (string.IsNullOrWhiteSpace(captchaResponse)) AddErrorAndRedirectToGetAction(filterContext);

            var validateResult = ValidateFromGoogle(urlToPost, secretKey, captchaResponse);
            if (!validateResult.Success) AddErrorAndRedirectToGetAction(filterContext);

            base.OnActionExecuting(filterContext);
        }

        private static void AddErrorAndRedirectToGetAction(ActionExecutingContext filterContext)
        {
            filterContext.Controller.TempData["InvalidCaptcha"] = "Invalid Captcha !";
            filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
        }

        private static ReCaptchaResponse ValidateFromGoogle(string urlToPost, string secretKey, string captchaResponse)
        {
            var postData = "secret=" + secretKey + "&response=" + captchaResponse;

            var request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                streamWriter.Write(postData);

            string result;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                    result = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<ReCaptchaResponse>(result);
        }
    }

    internal class ReCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public string ValidatedDateTime { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}