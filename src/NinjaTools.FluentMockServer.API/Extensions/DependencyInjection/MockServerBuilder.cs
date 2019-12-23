using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NinjaTools.FluentMockServer.API.Configuration.Expectations;
using NinjaTools.FluentMockServer.API.Infrastructure;
using NinjaTools.FluentMockServer.Utils;

namespace NinjaTools.FluentMockServer.API.Extensions.DependencyInjection
{
    public class MockServerBuilder : IMockServerBuilder
    {
        [NotNull]
        public IServiceCollection Services { get; }
        
        [NotNull] 
        public IConfiguration Configuration { get; }

        [NotNull] 
        public IMvcCoreBuilder MvcCoreBuilder { get; }
        
        public MockServerBuilder([NotNull] IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            Services = services;
            Configuration = configuration;
            
            // see this for why we register this as singleton http://stackoverflow.com/questions/37371264/invalidoperationexception-unable-to-resolve-service-for-type-microsoft-aspnetc
            // could maybe use a scoped data repository
            Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            Services.TryAddSingleton<IScopeRepository, ScopedHttpContextRepository>();
            
         //   Services.AddMemoryCache();
            
            
            //add asp.net services..
            var assembly = typeof(ExpectationConfigurationController).GetTypeInfo().Assembly;

            this.MvcCoreBuilder = Services.AddMvcCore()
                .AddApplicationPart(assembly)
                .AddControllersAsServices()
                .AddAuthorization()
                .AddNewtonsoftJson(); 
            
            Services.AddLogging();
        
            // Services.AddMiddlewareAnalysis();
        }
        
        
    }

 
}
