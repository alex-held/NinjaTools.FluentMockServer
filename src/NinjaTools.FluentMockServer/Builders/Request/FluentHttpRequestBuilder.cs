using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.HttpEntities;

[assembly: InternalsVisibleTo("NinjaTools.FluentMockServer.Tests")]
namespace NinjaTools.FluentMockServer.Builders.Request
{
    internal class FluentHeaderBuilder : IFluentResponseHeaderBuilder
    {
        public readonly HttpRequestMessage RequestMessage = new HttpRequestMessage();
        public readonly HttpResponseMessage ResponseMessage = new HttpResponseMessage();

        public FluentHeaderBuilder(Dictionary<string, string[]> seed = null)
        {
            _headers = seed ?? new Dictionary<string, string[]>();
        }
        
        private HttpRequestHeaders RequestHeaders => RequestMessage.Headers;
        private HttpResponseHeaders ResponseHeaders => ResponseMessage.Headers;
        
        /// <inheritdoc />
        public AuthenticationHeaderValue Authentication
        {
            get => RequestHeaders.Authorization;
            set => RequestHeaders.Authorization = value;
        }
        
        /// <inheritdoc />
        public IFluentHeaderBuilder AddHeaders(params (string name, string value)[] headers)
        {
            foreach (var kvp in headers ?? new (string name, string value)[0])
            {
                Add(kvp.name, kvp.value);
            }

            return this;
        }

        /// <inheritdoc />
        public IFluentHeaderBuilder Add(string name, string value)
        {
            RequestHeaders.TryAddWithoutValidation(name, value);
            _headers[name]  = new []{value};
            return this;
        }

        /// <inheritdoc />
        public IFluentHeaderBuilder AddContentType(string contentType)
        {
            return Add(Headers.ContentType, contentType);
        }

        /// <inheritdoc />
        public IFluentHeaderBuilder AddBasicAuth(string username, string password)
        {
            var authInfo = $"{username}:{password}";
            var base64 = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            Authentication = new AuthenticationHeaderValue("Basic", base64);
            return Add(Headers.Authentication, Authentication.ToString());
        }

        /// <inheritdoc />
        public Dictionary<string, string[]> Build() => _headers;

        /// <inheritdoc />
        public IFluentHeaderBuilder WithContentDispositionHeader(string type, string name, string filename)
        {
            ContentDisposition = new ContentDispositionHeaderValue(type)
            {
                Name = name,
                FileName = filename,
                FileNameStar = null
            };

            Add(Headers.ContentDisposition, ContentDisposition.ToString());
            return this;
        }
        
        private readonly Dictionary<string, string[]> _headers;

        public ContentDispositionHeaderValue ContentDisposition
        {
            get;
            set;
        }
    }
    
    internal sealed class  FluentHttpRequestBuilder : IFluentHttpRequestBuilder
    {
        private readonly HttpRequest _httpRequest;

        public FluentHttpRequestBuilder()
        {
            _httpRequest = new HttpRequest();
        }

        /// <inheritdoc />
        public IFluentHttpRequestBuilder WithPath(string path)
        {
            path = path?.TrimStart('/').Insert(0, "/") ?? "/";
            _httpRequest.Path = path;
            return this;
        }

        /// <inheritdoc />
        public IFluentHttpRequestBuilder KeepConnectionAlive(bool keepalive = true)
        {
            _httpRequest.KeepAlive = keepalive;
            return this;
        }

        /// <inheritdoc />
        public IFluentHttpRequestBuilder EnableEncryption(bool useSsl = true)
        {
            _httpRequest.Secure = true;
            return this;
        }
        
        /// <inheritdoc />
        public IFluentHttpRequestBuilder WithBody(Body.BodyType type, string value)
        {
            var builder = new FluentBodyBuilder();
  
            Action<IFluentBodyBuilder> factory = type switch
            {
                Body.BodyType.JSON => b => b.WithExactJson(value),
                Body.BodyType.JSON_PATH => b =>b.MatchingJsonPath(value),
                Body.BodyType.JSON_SCHEMA => b => b.MatchingJsonSchema(value),
                Body.BodyType.XML => b => b.WithXmlContent(value),
                Body.BodyType.XPATH =>  b =>b.MatchingXPath(value),
                Body.BodyType.XML_SCHEMA => b => b.MatchingXmlSchema(value),
                Body.BodyType.STRING => b => b.WithExactContent(value)
            };

            factory(builder);
            _httpRequest.Body = builder.Build();
            return this;
        }

        /// <inheritdoc />
        public IFluentHttpRequestBuilder ConfigureHeaders(Action<IFluentHeaderBuilder> headerFactory)
        {
            var builder = new FluentHeaderBuilder(_httpRequest.Headers);
            headerFactory?.Invoke(builder);
            _httpRequest.Headers = builder.Build();
            return this;
        }

        /// <inheritdoc />
        public IFluentHttpRequestBuilder WithContent(Action<IFluentBodyBuilder> contentFactory)
        {
            var builder = new FluentBodyBuilder();
            contentFactory(builder);
            _httpRequest.Body = builder.Build();
            return this;
        }


        /// <inheritdoc />
        public HttpRequest Build() => _httpRequest;
        
        /// <inheritdoc />
        public IFluentHttpRequestBuilder AddContentType(string contentType)
        {
            _httpRequest.Headers ??= new Dictionary<string, string[]>(); 
             _httpRequest.Headers[Headers.ContentType] =  new[] {contentType};
             return this;
        }
    }
}
