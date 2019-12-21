using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NinjaTools.FluentMockServer.API.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        [NotNull]
        public static IMockServerBuilder AddMockServer(this IServiceCollection services)
        {
            var configuration = services
                .BuildServiceProvider()
                .GetRequiredService<IConfiguration>();
            
            return new MockServerBuilder(services, configuration);
        }

        [NotNull]
        public static IMockServerBuilder AddMockServer(this IServiceCollection services, IConfiguration configuration)
        {
            return new MockServerBuilder(services, configuration);
        }
    }

    public interface IMockServerBuilder
    {
    }
}
