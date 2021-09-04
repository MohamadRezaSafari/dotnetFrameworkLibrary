using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TarahiOnline.Providers.ReCaptcha
{
    public class ValidateRecaptchaAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        private const string RECAPTCHA_RESPONSE_KEY = "g-recaptcha-response";

        public ICaptchaValidationService CaptchaService { get; set; }

        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            var isValidate = new InvisibleRecaptchaValidationService(ConfigurationManager.AppSettings["RecaptchaSecretKey"]).Validate(filterContext.HttpContext.Request[RECAPTCHA_RESPONSE_KEY]);
            if (!isValidate)
                filterContext.Controller.ViewData.ModelState.AddModelError("Recaptcha", "Captcha validation failed.");
        }
    }
}