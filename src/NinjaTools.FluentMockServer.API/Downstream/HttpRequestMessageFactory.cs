using System;
using System.Linq;
using System.Net.Http;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.Extensions;
using NinjaTools.FluentMockServer.Models;

namespace NinjaTools.FluentMockServer.API.Downstream
{
    public static class HttpRequestMessageFactory
    {
        public static HttpRequestMessage CreateHttpRequestMessage(DownstreamContext context)
        {
            var httpContext = context.HttpContext;
            var requestMessage = new HttpRequestMessage();
            var expectationRepository = context.ExpectationRepository;
            
            if (expectationRepository.GetExpectation(httpContext) is { } expectation)
            {
                // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
                foreach (var expectationHeader in expectation.HttpRequest.Headers)
                {
                    if(!requestMessage.Headers.TryAddWithoutValidation(expectationHeader.Key, expectationHeader.Value))
                    {
                        requestMessage.Content?.Headers.TryAddWithoutValidation(expectationHeader.Key, expectationHeader.Value);
                    }
                }
            }

            return requestMessage;
        }

        [NotNull]
        internal static HttpRequestMessage CreateDefaultHttpRequestMessage([NotNull] this HttpRequest request)
        {
            var requestMessage = new HttpRequestMessage();
            
            if (request.CouldHaveContentBasedOnMethod())
            {
                var streamContent = new StreamContent(request.Body);
                requestMessage.Content = streamContent;
            }

            foreach (var header in request.Headers)
            {
                if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                {
                    requestMessage?.Content.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }

            requestMessage.Headers.Host = request.Host.Host;
            requestMessage.RequestUri = CreateRequestUri(request);
            requestMessage.Method = new HttpMethod(request.Method);

            return requestMessage;
        }
        
        private static Uri CreateRequestUri([NotNull] HttpRequest request)
        {
            var scheme = request.Scheme;
            var host = request.Host.Host;
            var port = request.Host.Port;
            
            var path = request.Path.ToString();
            var query = request.Query.ToString();
            
            // TODO: Add https support
            var uriBuilder = new UriBuilder(scheme, host, port ?? 80, path)
            {
                Query = query
            };
            
            return uriBuilder.Uri;
        }
        
        
    }
}
