using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace NinjaTools.FluentMockServer.Domain.Builders.Request
{
    internal class FluentHeaderBuilder : IFluentResponseHeaderBuilder
    {
        private readonly Dictionary<string, string[]> _headers;
        private readonly HttpRequestMessage _requestMessage = new HttpRequestMessage();
        private readonly HttpResponseMessage _responseMessage = new HttpResponseMessage();

        public FluentHeaderBuilder(Dictionary<string, string[]> seed = null)
        {
            _headers = seed ?? new Dictionary<string, string[]>();
        }

        private HttpRequestHeaders RequestHeaders => _requestMessage.Headers;
        private HttpResponseHeaders ResponseHeaders => _responseMessage.Headers;

        /// <inheritdoc />
        public AuthenticationHeaderValue Authentication
        {
            get => RequestHeaders.Authorization;
            set => RequestHeaders.Authorization = value;
        }

        /// <inheritdoc />
        public IFluentHeaderBuilder AddHeaders(params (string name, string value)[] headers)
        {
            foreach (var kvp in headers ?? new (string name, string value)[0]) Add(kvp.name, kvp.value);

            return this;
        }

        /// <inheritdoc />
        public IFluentHeaderBuilder Add(string name, string value)
        {
            RequestHeaders.TryAddWithoutValidation(name, value);
            _headers[name] = new[] {value};
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
        public Dictionary<string, string[]> Build()
        {
            return _headers;
        }

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

        public ContentDispositionHeaderValue ContentDisposition { get; set; }
    }
}
