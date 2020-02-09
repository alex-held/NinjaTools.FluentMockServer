using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NinjaTools.FluentMockServer.API.Administration;
using NinjaTools.FluentMockServer.API.Proxy;
using NinjaTools.FluentMockServer.API.Types;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace NinjaTools.FluentMockServer.API.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseMockServer(this IApplicationBuilder app)
        {
            app.UseMockServerAdminBranch();
            app.UseMockServerMiddleware();

           // var initializer = app.ApplicationServices.GetRequiredService<IStartupInitializer>();
           // initializer.InitializeAsync().GetAwaiter().GetResult();
        }

        private static void UseMockServerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ProxyMiddleware>();
            app.UseMockServerRouting();
        }


        private static void UseMockServerRouting(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static void UseMockServerAdminBranch(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetRequiredService<ServiceConstants>();
            app.MapWhen(context => context.Connection.LocalPort == options.ADMIN_PORT, a => a
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mock-Server API v1");
                    c.InjectStylesheet("https://raw.githubusercontent.com/ostranme/swagger-ui-themes/develop/themes/3.x/theme-material.css");
                })
                .UseMockServerRouting());
        }

        // app.UseSwagger(c =>
            // {
            //     c.RouteTemplate = "swagger/{documentName}/swagger.json";
            // }).UseSwaggerUI(
            //     c =>
            //     {
            //         c.RoutePrefix = options.SWAGGER_PATH;
            //         c.SwaggerEndpoint($"./{options.SWAGGER_VERSION}/swagger.json", $"{options.APPLICATION_NAME} API {options.SWAGGER_VERSION}");
            //         c.InjectStylesheet("https://raw.githubusercontent.com/ostranme/swagger-ui-themes/develop/themes/3.x/theme-material.css");
            //     }
            // );


            //  app.UseHealthChecks($"/{options.HEALTH_CHECK_PATH.TrimStart('/')}");


            // app.UseSwaggerUI(c =>
            // {
            //     // c.ConfigObject = new ConfigObject
            //     // {
            //     //     ShowCommonExtensions = true,
            //     //     ShowExtensions = true,
            //     //     DeepLinking = true,
            //     //     DisplayOperationId = true,
            //     //     DisplayRequestDuration = true,
            //     //     DefaultModelRendering = ModelRendering.Example,
            //     //     DocExpansion = DocExpansion.Full
            //     // };
            //
            //     c.DocumentTitle = options.SWAGGER_VERSION;
            //
            //     c.ShowExtensions();
            //
            //
            //     c.SwaggerEndpoint($"./{options.SWAGGER_VERSION}/swagger.json", $"{options.APPLICATION_NAME} API {options.SWAGGER_VERSION}");
            //     c.InjectStylesheet("https://raw.githubusercontent.com/ostranme/swagger-ui-themes/develop/themes/3.x/theme-material.css");
            // });

    }
}
