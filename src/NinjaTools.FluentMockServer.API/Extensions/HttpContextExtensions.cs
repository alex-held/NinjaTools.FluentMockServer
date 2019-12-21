using System;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace NinjaTools.FluentMockServer.API.Extensions
{
    public static class RequestExtensions
    {
        public static bool CouldHaveContentBasedOnMethod([NotNull] this HttpRequest httpRequest)
        {
            return Enum.Parse<HttpMethod>(httpRequest.Method) switch
            {
                HttpMethod.Get => false,
                HttpMethod.Trace => false,
                HttpMethod.Delete => false,
                HttpMethod.Head => false,
                _ => true

            };
        }
    }

    public static class HttpContentExtensions
    {
        [CanBeNull]
        public static T Deserialize<T>([CanBeNull] this HttpContent content, JsonSerializerSettings? settings = null) where T : class
        {
            return content?.DeserializeAsync<T>(settings).Result;
        }
        
        public static async Task<T> DeserializeAsync<T>([CanBeNull]this HttpContent content, JsonSerializerSettings? settings = null) where T : class
        {
            if (content is {} httpContent)
            {
                return await httpContent.DeserializeAsync<T>(settings ?? new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                });
            }

            return null;
        }
    }
    public static class HttpContextExtensions
    {
        public static HttpRequestMessage CreateRequestMessage(this HttpContext context, Uri uri)
        {
            var request = context.Request;
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

            requestMessage.Headers.Host = uri.Authority;
            requestMessage.RequestUri = uri;
            requestMessage.Method = new System.Net.Http.HttpMethod(request.Method);

            return requestMessage;
        }
        
        public static HttpContext Proxyfy(this HttpContext context, Uri uri)
        {
            var proxyService = context.RequestServices.GetRequiredService<ProxyService>();

            using (var requestMessage = context.CreateRequestMessage(uri))
            {
                
            }

            return context;
        }
    }

    public class ProxyService
    {
    }
}
