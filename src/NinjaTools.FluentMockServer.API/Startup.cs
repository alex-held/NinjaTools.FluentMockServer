using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NinjaTools.FluentMockServer.API.DependencyInjection;

namespace NinjaTools.FluentMockServer.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices([NotNull] IServiceCollection services)
        {
            services.AddMockServer()
                .AddAdminPath()
                .AddSwagger()
                .AddInitializers(opt =>
                {
                    if (!opt.IsRunningInDocker) return;
                    opt.EnableConfigInitializer = true;
                    opt.EnableLoggingInitializer = true;
                });

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
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();

            app.UseMockServer();
        }
    }
}
