using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.Infrastructure;
using NinjaTools.FluentMockServer.API.Logging;
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
            Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            Services.TryAddSingleton<IConfigurationService, ConfigurationService>();
            Services.TryAddSingleton<IConfigFileProvider, ConfigFileProvider>();
            Services.TryAddSingleton<ILogService, LogService>();
            Services.TryAddSingleton<ILogRepository, LogRepository>();

            MvcCoreBuilder = Services
                .AddMvcCore(options => { options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; })
                .AddControllersAsServices()
                .AddAuthorization()
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.Formatting = Formatting.Indented;
                    opt.SerializerSettings.Converters.Add(new StringEnumConverter());
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            Services.AddMiddlewareAnalysis();
            Services.AddLogging();
            Services.AddInitializers();
        }

        public IServiceProvider ServiceProvider => Services.BuildServiceProvider();


        /// <inheritdoc />
        public IMvcCoreBuilder MvcCoreBuilder { get; }
        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }

    }
}
