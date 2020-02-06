using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.API.Extensions;

namespace NinjaTools.FluentMockServer.API.Models.ViewModels
{
    public interface IUpstreamRequest
    {
        bool WasMatched { get; }
        HttpRequestViewModel HttpRequest { get; }
        string? Path => HttpRequest.Path;
        string? Query => HttpRequest.QueryString;
        string HttpMethod => HttpRequest.Method;
        Headers? Headers => HttpRequest.Headers;
        Cookies? Cookies => HttpRequest.Cookies;
    }


    /// <inheritdoc />
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class UnmatchedRequest : IUpstreamRequest
    {
        [JsonIgnore]
        public HttpContext HttpContext { get; }

        public UnmatchedRequest(HttpContext context)
        {
            HttpContext = context;
            HttpRequest = new HttpRequestViewModel(context.Request);
        }

        /// <inheritdoc />
        [JsonIgnore]
        public bool WasMatched => false;

        /// <inheritdoc />
        [JsonIgnore]
        public HttpRequestViewModel HttpRequest { get; }
    }

    [JsonObject(MemberSerialization.OptOut, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class HttpRequestViewModel
    {
        public HttpRequestViewModel(HttpRequest request)
        {
            Request = request;
            Body = ReadBody();
            Headers = Request.Headers.ToHeadersOrDefault();
            Cookies = request.Cookies.ToCookiesOrDefault();
            Method = Request.Method;
            Path = Request.Path.HasValue ? Request.Path.Value : null;
            QueryString = Request.QueryString.HasValue ? Request.QueryString.Value : null;
            IsHttps = Request.IsHttps;
        }

        [JsonIgnore]
        public HttpRequest Request { get; }
        public string? Method { get; }
        public string? Path { get; }
        public string? Body { get; }
        public string? QueryString { get; }
        public Headers Headers { get; }
        public Cookies Cookies { get; }
        public bool? IsHttps { get; }

        private string? ReadBody()
        {
            var stream = Request.Body;

            try
            {
                if (!stream.CanRead || !stream.CanSeek || stream.Length <= 0) return null;

                Request.EnableBuffering();
                stream.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(stream);
                var body = reader.ReadToEndAsync().GetAwaiter().GetResult();
                return body != string.Empty ? body : null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
