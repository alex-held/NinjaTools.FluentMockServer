using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NinjaTools.FluentMockServer.API.Controllers;
using NinjaTools.FluentMockServer.API.Services;

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
                .AddSingleton<ExpectationService>()
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
