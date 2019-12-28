using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NinjaTools.FluentMockServer.API.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IMockServerBuilder AddMockServer(this IServiceCollection services, IConfiguration configuration)
        {
            return new MockServerBuilder(services, configuration);
        }

        public static IMockServerBuilder AddMockServer(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            return AddMockServer(services, config);
        }
    }
}
