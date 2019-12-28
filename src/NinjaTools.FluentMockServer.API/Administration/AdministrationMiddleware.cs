using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace NinjaTools.FluentMockServer.API.Administration
{
    public class AdministrationMiddleware
    {
        private readonly IAdminPath _adminPath;
        private readonly RequestDelegate _next;

        public AdministrationMiddleware(RequestDelegate next, IAdminPath adminPath)
        {
            _next = next;
            _adminPath = adminPath;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            
            if (request.Path.StartsWithSegments(_adminPath.Path, out var path))
            {
                var administrationService = context.RequestServices.GetRequiredService<IAdministrationService>();
                await administrationService.HandleAsync(context, path);
                return;
            }
            
            await _next.Invoke(context);
        }
    }
}
