using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Providers.Helpers
{
    public static class HtmlHelperProvider
    {
        /**
         * @using Providers.Helpers
         * @Html.EncryptHiddenField("userId", "Zzz")
         * */
        public static MvcHtmlString EncryptHiddenField(this HtmlHelper helper, string name, string value)
        {
            var builder = new TagBuilder("input");
            builder.MergeAttribute("type", "hidden");
            builder.MergeAttribute("name", name);
            builder.MergeAttribute("value", Encryption.ProtectedDataEncryption(value));

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }


        /*
         @using Providers.Helpers
         @Html.LabelCustom("LOL")
        */
        public static MvcHtmlString LabelCustom(this HtmlHelper helper, string value)
        {
            var builder = new TagBuilder("input");
            builder.MergeAttribute("type", "text");
            //builder.MergeAttribute("name", name);
            builder.MergeAttribute("value", value);
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}