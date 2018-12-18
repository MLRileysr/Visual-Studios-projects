using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace December2_2.CustomHtmlHelpers
{
    public static class CustomHtmlHelper
    {
        public static IHtmlString Image(this HtmlHelper helper, string src, string alt, string ID)
        {
            TagBuilder tb = new TagBuilder("img");
            if (ID != null)
            {
                string teststr = src + "  ";
                string sub = teststr.Substring(0, 1);
                if (sub == "~")   tb.Attributes.Add("src", VirtualPathUtility.ToAbsolute(src));
            tb.Attributes.Add("alt", alt);
            tb.Attributes.Add("height", "150"); 
            tb.Attributes.Add("width", "150");
            tb.Attributes.Add("onclick","location.href='Contact/"+ID+"'");
            }
            return new MvcHtmlString(tb.ToString(TagRenderMode.SelfClosing));
        }
    }
}