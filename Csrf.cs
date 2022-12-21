using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;

namespace EFDesign.Classes
{
    public class Csrf
    {
        public string TokenHeaderValue()
        {
            string cookieToken, formToken;
            AntiForgery.GetTokens(null, out cookieToken, out formToken);
            return cookieToken + ":" + formToken;
        }

        
        public bool ValidateRequestHeader(string csrf)
        {
            string cookieToken = "";
            string formToken = "";

            if (! String.IsNullOrEmpty(csrf))
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