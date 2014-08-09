using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationSite.Tools
{
    public static class UrlExtensions
    {
        public static string AbsoluteAction(this UrlHelper url, string action, string controller, object routeValues)
        {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;

            if (requestUrl == null) return null;
            string absoluteAction = string.Format("{0}{1}",
                requestUrl.GetLeftPart(UriPartial.Authority),
                url.Action(action, controller, routeValues));

            return absoluteAction;
        }
    }
}