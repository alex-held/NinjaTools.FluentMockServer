using System;
using System.Net.Http;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using NinjaTools.FluentMockServer.API.DependencyInjection;

namespace NinjaTools.FluentMockServer.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices([NotNull] IServiceCollection services)
        {
            services
                .AddMockServer()
                .AddTelemetry()
                .AddSwagger()
                .AddInitializers(opt =>
                {
                    if (!opt.IsRunningInDocker) return;
                    opt.EnableConfigInitializer = true;
                    opt.EnableLoggingInitializer = true;
                })
                .AddHealthChecks(hc => hc.AddUrlGroup(uriOptions => uriOptions
                        .AddUri(new Uri("http://localhost:1080/api/logs", UriKind.Absolute), o => o
                            .UseHttpMethod(HttpMethod.Get)
                            .ExpectHttpCodes(200, 500)), "api", HealthStatus.Healthy)
                    .AddUrlGroup(uri => uri.UsePost()
                        .AddUri(new Uri("http://localhost:1080/setups/create"))
                        .ExpectHttpCodes(200, 500))
                    .AddApplicationInsightsPublisher()
                );

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMockServer();
        }
    }
}
