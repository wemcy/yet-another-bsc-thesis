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
