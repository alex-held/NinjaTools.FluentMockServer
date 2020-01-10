using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NinjaTools.FluentMockServer.API.Proxy;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseMockServer(this IApplicationBuilder app)
        {
            app.UseMockServerMiddleware();
            app.UseMockServerRouting();

            var initializer = app.ApplicationServices.GetRequiredService<IStartupInitializer>();
            initializer.InitializeAsync().GetAwaiter().GetResult();
        }

        private static void UseMockServerProxyMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ProxyMiddleware>();
        }

        private static void UseMockServerMiddleware(this IApplicationBuilder app)
        {
            app.UseMockServerProxyMiddleware();
        }

        private static void UseMockServerRouting(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
