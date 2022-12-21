using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MohammadReza
{
    public class Security
    {
        private readonly string HtmlTag = "<.*?>";
        private readonly string BaseRegex = "[\"']";
        private readonly string Slash = @"[""'\\/]+";
        private readonly string Trim = @"\s+";


        private string Base(string str)
        {
            string trim = Regex.Replace(str, Trim, string.Empty);
            string slash = Regex.Replace(trim, Slash, string.Empty);
            return Regex.Replace(slash, BaseRegex, string.Empty);
        }


        public string Xss(string str)
        {
            string tag = Base(str);
            return Regex.Replace(tag, HtmlTag, string.Empty);
        }

    }
}