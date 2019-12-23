using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Configuration;

namespace NinjaTools.FluentMockServer.API.Middleware
{
  
    public delegate Task<HttpResponseMessage> HandleProxyRequest(HttpContext httpContext);
    
    /// <summary>
    ///     Represents a delegate that handles a proxy request.
    /// </summary>
    /// <param name="context">
    ///     An <see cref="DownstreamContext"/> that represents the incoming proxy request.
    /// </param>
    /// <returns>
    ///     A <see cref="HttpResponseMessage"/> that represents
    ///    the result of handling the proxy request.
    /// </returns>
    public delegate Task<HttpResponseMessage> MockServerRequestDelegate(DownstreamContext context);
}
