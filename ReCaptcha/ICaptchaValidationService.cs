using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TarahiOnline.Providers.ReCaptcha
{
    public interface ICaptchaValidationService
    {
        bool Validate(string response);
    }

}