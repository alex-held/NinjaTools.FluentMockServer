using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NinjaTools.FluentMockServer.API.Administration;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.Logging;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        [NotNull]
        public static IMockServerBuilder AddMockServer([NotNull] this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            return new MockServerBuilder(services, config);
        }

        [NotNull]
        public static IMockServerBuilder AddInitializers(this IMockServerBuilder builder, Action<StartupInitializerOptions> initializerOptions)
        {
            var services = builder.Services;

            services.ConfigureOrUpdate(initializerOptions);
            services.TryAddSingleton<IStartupInitializer>(sp =>
            {
                var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<StartupInitializer>();
                var options = sp.GetRequiredService<StartupInitializerOptions>();
                var startupInitializer = new StartupInitializer(logger, options );

                startupInitializer.AddInitializer(new ConfigurationInitializer(sp.GetRequiredService<IConfigurationService>()));
                startupInitializer.AddInitializer(new LoggingInitializer(new FileSystem()));

                return startupInitializer;
            });

            return builder;
        }

        [NotNull]
        public static IMockServerBuilder  AddSwagger(this IMockServerBuilder builder)
        {
            builder.Services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo
                {
                    Description = "MockServer Description",
                    Contact = new OpenApiContact
                    {
                        Email = "contact@alexanderheld.net",
                        Name = "Alexander Held",
                        Url = new Uri("https://github.com/alex-held/NinjaTools.FluentMockServer")
                    },
                    Title = "Mock-Server",
                    Version = "v1"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                o.IncludeXmlComments(xmlPath);
            });
            return builder;
        }


        internal static IServiceCollection ConfigureOrUpdate<T>(this IServiceCollection services, Action<T> update)
        {
            return services.Replace(new ServiceDescriptor(typeof(StartupInitializerOptions), sp =>
            {
                if (!(sp.GetService<T>() is {} options))
                    return ActivatorUtilities.GetServiceOrCreateInstance(sp, typeof(T));
                update?.Invoke(options);
                return options;
            }, ServiceLifetime.Singleton));
        }
    }
}
