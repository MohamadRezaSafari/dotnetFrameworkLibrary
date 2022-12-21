using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Providers
{
    public class StringHelper
    {
        //  StringHelper.String("الْحَمْدُ لِلَّهِ رَبِّ الْعَالَمِينَ - 1:2");
        public static string String(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }


    /**
     *      var x = Network.GetMACAddress();
            var y = x.SplitInParts(2);
            var z = String.Join("-", y);
    **/
    public static class StringExtensions
    {

        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

    }
}
 