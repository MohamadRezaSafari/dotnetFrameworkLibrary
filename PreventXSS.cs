using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Providers
{
    public class PreventXSS
    {
        private static string HtmlTag = "<.*?>";
        private static string BaseRegex = "[\"']";
        private static string Slash = @"[""'\\/]+";
        private static string Trim = @"\s+";
        private static string Txt = String.Empty;
        

        public static string Filter(string str)
        {
            try
            {
                if (!String.IsNullOrEmpty(str))
                {
                    var _value = Xss(str);
                    Txt = (String.IsNullOrEmpty(_value)) ? "..." : _value;
                }
                return Txt;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }


        private static string Base(string str)
        {
            string trim = Regex.Replace(str, Trim, string.Empty);
            string slash = Regex.Replace(trim, Slash, string.Empty);
            return Regex.Replace(slash, BaseRegex, string.Empty);
        }


        private static string Xss(string str)
        {
            string tag = Base(str);
            return Regex.Replace(tag, HtmlTag, string.Empty);
        }
    }
}