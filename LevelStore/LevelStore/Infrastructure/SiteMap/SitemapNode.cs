using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace LevelStore.Infrastructure.SiteMap
{
    public class SitemapNode
    {
        public SitemapFrequency? Frequency { get; set; }
        public DateTime? LastModified { get; set; }
        public double? Priority { get; set; }
        public string Url { get; set; }
    }

    public enum SitemapFrequency
    {
        Never,
        Yearly,
        Monthly,
        Weekly,
        Daily,
        Hourly,
        Always
    }

    


public static class UrlHelperExtensions
{
    public static string AbsoluteRouteUrl(
        this UrlHelper urlHelper,
        string routeName,
        object routeValues = null)
    {
        string scheme = urlHelper.ActionContext.HttpContext.Request.Scheme;
        return urlHelper.RouteUrl(routeName, routeValues, scheme);
    }
}
}
