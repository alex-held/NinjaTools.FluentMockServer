using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Utils;

namespace NinjaTools.FluentMockServer.FluentAPI.Builders.ValueObjects
{
    internal class FluentHeaderBuilder : IFluentResponseHeaderBuilder
    {
        private readonly Dictionary<string, string[]> _headers;
        private readonly HttpRequestMessage _requestMessage = new HttpRequestMessage();
        private readonly HttpResponseMessage _responseMessage = new HttpResponseMessage();

        public FluentHeaderBuilder([CanBeNull] Dictionary<string, string[]> seed = null)
        {
            _headers = seed ?? new Dictionary<string, string[]>();
        }

        private HttpRequestHeaders RequestHeaders => _requestMessage.Headers;
        // ReSharper disable once UnusedMember.Local
        // TODO: Add Builder Methods
        private HttpResponseHeaders ResponseHeaders => _responseMessage.Headers;

        /// <inheritdoc />
        public AuthenticationHeaderValue Authentication
        {
            get => RequestHeaders.Authorization;
            set => RequestHeaders.Authorization = value;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHeaderBuilder AddHeaders([CanBeNull] params (string name, string value)[] headers)
        {
            foreach (var kvp in headers ?? new (string name, string value)[0]) Add(kvp.name, kvp.value);

            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHeaderBuilder Add([NotNull] string name, string value)
        {
            RequestHeaders.TryAddWithoutValidation(name, value);
            _headers[name] = new[] {value};
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHeaderBuilder AddContentType(string contentType)
        {
            return Add(Headers.ContentType, contentType);
        }

        /// <inheritdoc />
        [NotNull]
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
        [NotNull]
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
