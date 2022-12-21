using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MohammadReza
{
    public class Firewall
    {
        //
        private MohammadReza.Cookie cookie = new Cookie();
        //
        private MohammadReza.Encryption enc = new Encryption();
        // Encryption Key
        private readonly string key = "rm2y9%$^*&!H@N#)T&12rfre";

        /**
        *   Generate Login Cookie
        */
        public void GenerateLoginCookie()
        {
            if(!cookie.ExistCookie("Login"))
            {
                cookie.SetCookie("Login", enc.CreateToken("_loginFirewallSystem", key), "minute", 3);
            }
        }

        /**
        *   Check Login Cookie
        *
        *   @return bool
        */
        public bool CheckLoginCookie()
        {
            if (cookie.ExistCookie("Login"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /**
        *   Generate Attempt Cookie
        */
        public void GenerateAttemptCookie()
        {
            if (!cookie.ExistCookie("Attempt"))
            {
                cookie.SetCookie("Attempt", enc.CreateToken("_attemptFirewallSystem", key), "minute", 1);
            }
        }


       /**
       *   Check Too Many Attempt Cookie
       *
       *   @return bool
       */
        public bool CheckAttemptCookie()
        { 
            if (cookie.ExistCookie("Attempt"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}