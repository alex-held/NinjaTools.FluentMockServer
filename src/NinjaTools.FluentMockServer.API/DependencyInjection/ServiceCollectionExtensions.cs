using System;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.Services;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        [NotNull]
        public static IMockServerBuilder AddMockServer([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            return new MockServerBuilder(services, configuration);
        }

        [NotNull]
        public static IMockServerBuilder AddMockServer([NotNull] this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            return AddMockServer(services, config);
        }

        internal static IServiceCollection AddInitializers(this IServiceCollection services)
        {
            services.TryAddSingleton<IStartupInitializer>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<StartupInitializer>>();
                var startupInitializer = new StartupInitializer(logger);

                startupInitializer.AddInitializer(new ConfigurationInitializer(sp.GetRequiredService<IConfigurationService>()));
                return startupInitializer;
            });

            return services;
        }
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(o =>
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
        }
    }
}
