using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Services;

namespace NinjaTools.FluentMockServer.API.Proxy
{
    public class ProxyMiddleware
    {
        private readonly ISetupService _setupService;
        private readonly ILogService _logService;

        public ProxyMiddleware(ISetupService setupService, ILogService logService)
        {
            _setupService = setupService;
            _logService = logService;
        }

        public async Task InvokeAsync([NotNull] HttpContext context)
        {
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
