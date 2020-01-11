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
        private readonly ILogService _logService;

        public ProxyMiddleware(RequestDelegate next, ISetupService setupService, IOptions<AdminOptions> adminOptions, ILogService logService)
        {
            _next = next;
            _setupService = setupService;
            _adminOptions = adminOptions;
            _logService = logService;
        }

        public async Task InvokeAsync([NotNull] HttpContext context)
        {
            var path = context.Request.Path;
            var method = context.Request.Method;
            if (context.Connection.LocalPort ==  _adminOptions.Value.Port)
            {
                await _next.Invoke(context);
                return;
            }
            
            if (_setupService.TryGetMatchingSetup(context, out var setup))
            {
                _logService.Log(fac => fac.RequestMached(context, setup));
                context.Response.StatusCode = setup.Action.Response.StatusCode;

                if (setup.Action.Response.Body is {} body)
                {
                    await context.Response.WriteAsync(body);
                    return;
                }

                await context.Response.CompleteAsync();
            }
            else
            {
                _logService.Log(fac => fac.RequestUnmatched(context));
                context.Response.Clear();
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync($"No expectation for this request.");
            }
        }
    }

}
