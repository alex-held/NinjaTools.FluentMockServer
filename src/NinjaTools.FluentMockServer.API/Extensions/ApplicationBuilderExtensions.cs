using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Middleware;
using HttpResponse = NinjaTools.FluentMockServer.Models.HttpEntities.HttpResponse;

namespace NinjaTools.FluentMockServer.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void RunProxy(this IApplicationBuilder app, HandleProxyRequest handleProxyRequest)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (handleProxyRequest == null)
            {
                throw new ArgumentNullException(nameof(handleProxyRequest));
            }

           // app.UseMiddleware<ProxyMiddleware<HandleProxyRequestWrapper>>(new HandleProxyRequestWrapper(handleProxyRequest))
        }
        
        private class HandleProxyRequestWrapper : IProxyHandler
        {
            private readonly HandleProxyRequest _handleProxyRequest;

            public HandleProxyRequestWrapper(HandleProxyRequest handleProxyRequest)
            {
                _handleProxyRequest = handleProxyRequest;
            }

            public Task<HttpResponseMessage> HandleProxyRequest(HttpContext httpContext) => _handleProxyRequest(httpContext);
        }
    }

    /// <summary>
    ///     Exposes a handler which supports forwarding a request to an upstream host.
    /// </summary>
    public interface IProxyHandler
    {
        /// <summary>
        ///     Represents a delegate that handles a proxy request.
        /// </summary>
        /// <param name="context">
        ///     An HttpContext that represents the incoming proxy request.
        /// </param>
        /// <returns>
        ///     A <see cref="HttpResponseMessage"/> that represents
        ///    the result of handling the proxy request.
        /// </returns>
        Task<HttpResponseMessage> HandleProxyRequest(HttpContext context);
    }
}
