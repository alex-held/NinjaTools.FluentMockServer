using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace NinjaTools.FluentMockServer.API.Models.ViewModels
{
    public class HttpRequestViewModel
    {
        [JsonIgnore]
        public HttpRequest Request { get; }

        public HttpRequestViewModel(HttpRequest request)
        {
            Request = request;
            Body = ReadBody();

            Headers = Request.Headers.Any()
                ? Request.Headers.ToDictionary(
                    k => k.Key,
                    v => v.Value.ToList())
                : null;

            Cookies = request.Cookies.Any()
                ? Request.Cookies.ToDictionary(
                    k => k.Key,
                    v => v.Value)
                : null;

            Method = Request.Method;
            Path = Request.Path.Value;
            QueryString = Request.QueryString.HasValue ? Request.QueryString.Value : null;
            IsHttps = Request.IsHttps;
        }

        public string? Method { get; }
        public string? Path { get; }
        public string? Body { get; }
        public string? QueryString { get; }
        public Dictionary<string, List<string>> Headers { get; }
        public Dictionary<string, string> Cookies { get; }
        public bool IsHttps { get; }

        private string? ReadBody()
        {
            var stream = Request.Body;

            try
            {
                if (!stream.CanRead || !stream.CanSeek || stream.Length <= 0)
                {
                    return null;
                }

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
