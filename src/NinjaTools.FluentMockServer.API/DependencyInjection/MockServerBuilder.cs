using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NinjaTools.FluentMockServer.API.Infrastructure;
using NinjaTools.FluentMockServer.API.Services;

namespace NinjaTools.FluentMockServer.API.DependencyInjection
{
    public class MockServerBuilder : IMockServerBuilder
    {
        public MockServerBuilder([NotNull] IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            Services.TryAddSingleton<ISetupRepository, SetupRepository>();
            Services.TryAddSingleton<ISetupService, SetupService>();
            Services.TryAddScoped<IAdministrationService, AdministrationService>();
            Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            MvcCoreBuilder = Services.AddMvcCore()
                .AddControllersAsServices()
                .AddAuthorization()
                .AddNewtonsoftJson();

            Services.AddMiddlewareAnalysis();
            Services.AddLogging();
        }

        public IServiceProvider ServiceProvider => Services.BuildServiceProvider();


        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }

        /// <inheritdoc />
        public IMvcCoreBuilder MvcCoreBuilder { get; }
    }
}
