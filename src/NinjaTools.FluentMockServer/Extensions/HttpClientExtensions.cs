using System;
using System.Net.Http;

namespace NinjaTools.FluentMockServer.Extensions
{
    internal static class HttpClientExtensions
    {
        public static HttpClient WithDefaults(this HttpClient httpClient, Uri baseAddress)
        {
            httpClient.BaseAddress = baseAddress;
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Host = null;

            return httpClient;
        }
    }
}
