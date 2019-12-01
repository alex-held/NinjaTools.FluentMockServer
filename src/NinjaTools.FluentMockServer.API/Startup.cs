using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NinjaTools.FluentMockServer.API.Data;
using NinjaTools.FluentMockServer.API.Services;
using Serilog;
using Serilog.Events;

namespace NinjaTools.FluentMockServer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<ExpectationDbContext>(opt =>
                {
                    opt.UseSqlite($"Data Source={Path.Combine(Directory.GetCurrentDirectory(), "Data/expectations.sb")}");
                    opt.ConfigureWarnings(warnings => { warnings.Default(WarningBehavior.Log); });
                })
                .AddTransient<ExpectationService>()
                .AddControllers()
                .AddNewtonsoftJson()
                .AddMvcOptions(opt => opt.EnableEndpointRouting = false);
        }

       
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseSerilogRequestLogging(options =>
            {
                // Customize the message template
                options.MessageTemplate = "Handled {RequestPath}";

                // Emit debug-level events instead of the defaults
                options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

                // Attach additional properties to the request completion event
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                };
            });
            
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UsePathBase(new PathString("/mockconfig"));
            app.UseMvc();
            
            app.UseRouting();
//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllers();
//                endpoints.MapControllerRoute("mockconfig", "{controller}")}
//                endpoints.MapControllerRoute("expectations", "mockconfig/{controller}/{action}");
//            });
        }
    }
}
