using Microsoft.AspNetCore.Builder;
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
                endpoints.MapControllers();
            });
        }
    }
}
