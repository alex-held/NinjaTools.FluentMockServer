using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
