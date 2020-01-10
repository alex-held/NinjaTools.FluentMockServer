using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NinjaTools.FluentMockServer.API.Administration;
using NinjaTools.FluentMockServer.API.Proxy;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseMockServer(this IApplicationBuilder app)
        {
            app.UseMockServerAdminBranch();

            app.UseMockServerMiddleware();
            app.UseMockServerRouting();

            var initializer = app.ApplicationServices.GetRequiredService<IStartupInitializer>();
            initializer.InitializeAsync().GetAwaiter().GetResult();
        }

        private static void UseMockServerMiddleware(this IApplicationBuilder app)
        {
            app.UseMockServerProxyMiddleware();
        }


        private static void UseMockServerProxyMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ProxyMiddleware>();
        }


        private static void UseMockServerRouting(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void UseMockServerAdminBranch(this IApplicationBuilder app)
        {
            var adminOptions = app.ApplicationServices.GetRequiredService<IOptions<AdminOptions>>().Value;
            app.MapWhen(context => context.Connection.LocalPort == adminOptions.Port, MockServerAdminPipeline);
        }



        private static readonly Action<IApplicationBuilder> MockServerAdminPipeline = app =>
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mock-Server API v1");

                // https://github.com/ostranme/swagger-ui-themes/blob/develop/themes/3.x/theme-material.css
                // c.InjectStylesheet();
            });

            app.UseMockServerRouting();
        };

    }
}
