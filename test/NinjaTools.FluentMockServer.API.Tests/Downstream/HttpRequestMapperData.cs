using System;
using System.IO;
using System.Net.Http;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Xunit;

namespace NinjaTools.FluentMockServer.API.Tests.Downstream
{

    public class HttpRequestMapperData : TheoryData<HttpRequest, HttpRequestMessage>
    {
        public HttpRequestMapperData()
        {
            Add(new Uri("http://example.com/some/path"), HttpMethod.Get);
            Add(new Uri("https://rabbitmq.net/virtualHost"), HttpMethod.Get);
            Add(new Uri("http://example.com/some/path"), HttpMethod.Put, RandomEmailFactory.GenerateOneRandomEmail());
            Add(new Uri("https://google.com"), HttpMethod.Put ,RandomEmailFactory.GenerateOneRandomEmail());
        }

        public void Add([NotNull] Uri requestUri, [NotNull] HttpMethod method, [CanBeNull] object body = null)
        {
            var (request, requestMessage) = Create(requestUri, method, body);
            Add(request, requestMessage);
        }


        public static (HttpRequest request, HttpRequestMessage requestMessage) Create(
            [NotNull] Uri requestUri,
            [NotNull] HttpMethod method,
            [CanBeNull] object body)
        {
            var httpContext = new DefaultHttpContext();
            var requestMessage = new HttpRequestMessage
            {
                Content = body is null
                    ? null
                    : new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body))),
                Method = method,
                RequestUri = requestUri
            };
            
            httpContext.Request.Scheme = requestUri.Scheme;
            httpContext.Request.Host = new HostString(requestUri.Authority, requestUri.Port);
            httpContext.Request.Path = new PathString(requestUri.AbsolutePath);
            httpContext.Request.QueryString = new QueryString(requestUri.Query);
            httpContext.Request.Method = method.ToString();
           
            
         
            if (body != null)
            {
                var memoryStream = new MemoryStream();
                var streamWriter = new StreamWriter(memoryStream);
                var bytes = JsonConvert.SerializeObject(body, Formatting.Indented);
                streamWriter.Write(bytes);

                httpContext.Request.Body = memoryStream;
                httpContext.Request.EnableBuffering();
                httpContext.Request.ContentType = "application/octet-stream";
                requestMessage.Content = new ByteArrayContent(memoryStream.ToArray());
            }
     


            return (httpContext.Request, requestMessage);
        }

    }
}
