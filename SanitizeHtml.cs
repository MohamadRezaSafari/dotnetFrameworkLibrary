using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace TarahiOnline.Providers
{
    public class SanitizeHtml : ValidationAttribute
    {
        private static string HtmlTag = "<.*?>";
        private static string BaseRegex = "[\"']";
        private static string Slash = @"[""'\\/]+";
        private static string Trim = @"\s+";


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var Txt = Xss(value.ToString());

            validationContext
                .ObjectType
                .GetProperty(validationContext.MemberName)
                .SetValue(validationContext.ObjectInstance, Txt, null);

            return ValidationResult.Success;
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
            if (String.IsNullOrEmpty(Regex.Replace(tag, HtmlTag, string.Empty)))
            {
                return "a";
            }
            else
            {
                return tag;
            }
        }
    }
}