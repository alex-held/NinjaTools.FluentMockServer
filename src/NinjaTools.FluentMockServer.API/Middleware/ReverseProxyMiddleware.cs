using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace NinjaTools.FluentMockServer.API.Middleware
{
    public class ReverseProxyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpClient _httpClient;
        private readonly ReverseProxyOptions _options;

    }

    internal class ReverseProxyOptions
    {
        
    }

    internal class ReverseProxyRule
    {
        public Func<Uri, bool> Matcher { get; set; } 
    }
}
