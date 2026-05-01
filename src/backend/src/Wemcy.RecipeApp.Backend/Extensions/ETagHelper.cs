using Azure;
using Azure.Core;
using Microsoft.Net.Http.Headers;

namespace Wemcy.RecipeApp.Backend.Extensions
{
    public static class ETagHelper
    {
        public static bool CheckETagMatch(this ControllerBase controller, string etag)
        {
            controller.Response.Headers[HeaderNames.ETag] = etag;
            controller.Response.Headers[HeaderNames.CacheControl] = "public, max-age=3600"; // Cache for 1 hour
            controller.Response.Headers[HeaderNames.Expires] = DateTime.UtcNow.AddHours(1).ToString("R"); 
            if (controller.Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var ifNoneMatchHeader))
            {
                var ifNoneMatch = new EntityTagHeaderValue(ifNoneMatchHeader.ToString());
                if (ifNoneMatch.Tag == etag)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
