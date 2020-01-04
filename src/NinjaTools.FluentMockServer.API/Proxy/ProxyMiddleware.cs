using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Services;

namespace NinjaTools.FluentMockServer.API.Proxy
{
    public class ProxyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISetupService _setupService;

        public ProxyMiddleware(RequestDelegate next, ISetupService setupService)
        {
            _next = next;
            _setupService = setupService;
        }

        public async Task InvokeAsync([NotNull] HttpContext context)
        {
            if (_setupService.TryGetMatchingSetup(context, out var setup))
            {
                context.Response.StatusCode = setup.Action.Response.StatusCode;
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
