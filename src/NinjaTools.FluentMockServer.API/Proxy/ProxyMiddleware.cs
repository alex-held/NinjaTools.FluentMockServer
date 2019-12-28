using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Proxy
{
    public class ProxyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISetupRepository _setupRepository;

        public ProxyMiddleware(RequestDelegate next, ISetupRepository setupRepository)
        {
            _next = next;
            _setupRepository = setupRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_setupRepository.TryGetMatchingSetup(context) is {} setup)
            {
                context.Response.StatusCode = (int) setup.Action.Response.StatusCode;
                await context.Response.WriteAsync(setup.Action.Response.Body);
            }
            else
            {
                context.Response.Clear();
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync($"No expectation for this request.");
            }
        }
    }
}
