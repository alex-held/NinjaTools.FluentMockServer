using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NinjaTools.FluentMockServer.API.Administration;
using NinjaTools.FluentMockServer.API.Services;

namespace NinjaTools.FluentMockServer.API.Proxy
{
    public class ProxyMiddleware
    {
        private readonly IOptions<AdminOptions> _adminOptions;
        private readonly RequestDelegate _next;
        private readonly ISetupService _setupService;

        public ProxyMiddleware(RequestDelegate next, ISetupService setupService, IOptions<AdminOptions> adminOptions)
        {
            _next = next;
            _setupService = setupService;
            _adminOptions = adminOptions;
        }

        public async Task InvokeAsync([NotNull] HttpContext context)
        {
            if (context.Connection.LocalPort ==  _adminOptions.Value.Port)
            {
                await _next.Invoke(context);
                return;
            }
            
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
