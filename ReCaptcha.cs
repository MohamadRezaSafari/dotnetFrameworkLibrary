using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Providers
{
    /**
     *  <script src='https://www.google.com/recaptcha/api.js?hl=fa'></script>
     *  <div class="g-recaptcha" data-sitekey="6LcfK3QUAAAAAEhvomSJ3A36S5tIlMISXM-0O3OI"></div>
     * 
     * */
    public class ReCaptcha
    {
        private readonly static string _secret = "6LcfK3QUAAAAAABTk33CCatFbkqvNIeaaadG2SzB";


        public static bool Send()
        {
            var response = HttpContext.Current.Request["g-recaptcha-response"];
            if (response != null && IsValid(response))
                return true;

            return false;
            
        }


        public static bool IsValid(string response)
        {
            var client = new WebClient();
            var reply = client.DownloadString($"https://www.google.com/recaptcha/api/siteverify?secret={_secret}&response={response}");
            var captchaResponse = JsonConvert.DeserializeObject<RepaptchaResponse>(reply);

            if (!captchaResponse.Success)
                return false;

            return true;
        }
    }


    public class RepaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}