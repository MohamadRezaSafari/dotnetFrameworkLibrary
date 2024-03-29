﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using Providers;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Providers
{
    public class XSS : ValidationAttribute
    {
        private static string HtmlTag = "<.*?>";
        private static string BaseRegex = "[\"']";
        private static string Slash = @"[""'\\/]+";
        private static string Trim = @"\s+";


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var _value = Xss(value.ToString());
                var Txt = (String.IsNullOrEmpty(_value)) ? "..." : _value;

                validationContext
                    .ObjectType
                    .GetProperty(validationContext.MemberName)
                    .SetValue(validationContext.ObjectInstance, Txt, null);

                return ValidationResult.Success;
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