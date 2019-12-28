using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API
{
    public static class RequestMatcherExtensions
    {
        public static bool IsMatch(this HttpContext context, RequestMatcher requestMatcher)
        {
            return requestMatcher.IsMatch(context);
        }
    }
}
