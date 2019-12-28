using Microsoft.AspNetCore.Builder;
using NinjaTools.FluentMockServer.API.Proxy;

namespace NinjaTools.FluentMockServer.API.Administration
{
    public static class ApplicationBuilderExtensions
    {
        private static void UseMockServerProxyMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ProxyMiddleware>();
        }

        private static void UseAdministrationMiddleware(this IApplicationBuilder app)
        {
            
            app.UseMiddleware<AdministrationMiddleware>();
        }

        public static void UseMockServerMiddleware(this IApplicationBuilder app)
        {
            app.UseAdministrationMiddleware();
            app.UseMockServerProxyMiddleware();
        }
    }
}
