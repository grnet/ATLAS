using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace StudentPractice.Portal
{
    public enum enVersioning
    {
        Enabled,
        Disabled
    }

    public static class Html
    {
        public static IHtmlString Css(string cssPath, enVersioning versioned = enVersioning.Enabled)
        {
            return CssWithMedia(cssPath, versioned, null);
        }

        public static IHtmlString CssWithMedia(string cssPath, enVersioning versioned = enVersioning.Enabled, string media = null)
        {
            if (string.IsNullOrEmpty(cssPath))
                throw new ArgumentNullException("cssPath");

            if (cssPath.StartsWith("/"))
                cssPath = "~" + cssPath;

            cssPath = VirtualPathUtility.ToAbsolute(cssPath);

            if (versioned == enVersioning.Enabled)
                cssPath = string.Format("{0}?v={1}", cssPath, Global.VersionNumber);

            media = media == null ? string.Empty : string.Format(" media=\"{0}\"", media);
            
            return new HtmlString(string.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\"{1} />", cssPath, media));
        }
    }
}