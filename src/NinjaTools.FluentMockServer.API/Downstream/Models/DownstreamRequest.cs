using System;
using System.Net.Http;
using System.Net.Http.Headers;
using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.API.Downstream.Models
{
    public class DownstreamRequest
    {
        private readonly HttpRequestMessage _requestMessage;
        
        public DownstreamRequest(HttpRequestMessage requestMessage)
        {
            _requestMessage = requestMessage;
            Method = _requestMessage.Method.Method;
            OriginalString = _requestMessage.RequestUri.OriginalString;
            Scheme = _requestMessage.RequestUri.Scheme;
            Host = _requestMessage.RequestUri.Host;
            Port = _requestMessage.RequestUri.Port;
            Headers = _requestMessage.Headers;
            AbsolutePath = _requestMessage.RequestUri.AbsolutePath;
            Query = _requestMessage.RequestUri.Query;
            Content = _requestMessage.Content;
        }

        public HttpRequestHeaders Headers { get; }

        public string Method { get; }

        public string OriginalString { get; }

        public string Scheme { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string AbsolutePath { get; set; }

        public string Query { get; set; }

        public HttpContent Content { get; set; }
        
        /// <summary>
        /// Creates an <see cref="Uri"/> based on <see cref="DownstreamRequest"/>'s internal state.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public Uri ResolveUri()
        {
            var uriBuilder = new UriBuilder
            {
                Port = Port,
                Host = Host,
                Path = AbsolutePath,
                Query = RemoveLeadingQuestionMark(Query),
                Scheme = Scheme
            };

            return uriBuilder.Uri;
        }

        /// <summary>
        /// Creates the <see cref="Uri.AbsoluteUri"/> based on <see cref="DownstreamRequest"/>'s internal state.
        /// </summary>
        [NotNull]
        public string ResolveUriString() => ResolveUri().AbsoluteUri;


        /// <inheritdoc />
        [NotNull]
        public override string ToString() => ResolveUriString();

        [CanBeNull]
        private string RemoveLeadingQuestionMark([CanBeNull] string query)
        {
            if (!string.IsNullOrEmpty(query) && query.StartsWith("?"))
            {
                return query.Substring(1);
            }

            return query;
        }
    }   
    
}
