using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NinjaTools.FluentMockServer.API.Administration;
using NinjaTools.FluentMockServer.API.Proxy;

namespace NinjaTools.FluentMockServer.API.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        private static void UseMockServerProxyMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ProxyMiddleware>();
        }

        public static void UseMockServerMiddleware(this IApplicationBuilder app)
        {
            app.UseMockServerProxyMiddleware();
        }

        public static void UseMockServerRouting(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                var administrationOptions = app.ApplicationServices.GetRequiredService<IOptions<AdminOptions>>().Value;
                endpoints.MapControllers().RequireHost($"*:{administrationOptions.Port.ToString()}");
            });
        }
    }
}
