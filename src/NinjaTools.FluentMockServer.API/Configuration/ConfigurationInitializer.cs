using System.Threading.Tasks;
using NinjaTools.FluentMockServer.API.DependencyInjection;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.Configuration
{

    /// <summary>
    /// Marker interface to identify and disable <see cref="ConfigurationInitializer"/>  at startup,
    /// when configured in <seealso cref="ServiceCollectionExtensions.AddInitializers"/>.
    /// </summary>
    public interface IConfigurationInitializer : IInitializer
    { }

    /// <summary>
    /// Bootstraps the mock-server using <see cref="IConfigFile"/> from the filesystem.
    /// </summary>
    internal sealed class ConfigurationInitializer : IInitializer
    {
        private readonly IConfigurationService _configurationService;

        public ConfigurationInitializer(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        /// <inheritdoc />
        public Task InitializeAsync()
        {
              _configurationService.Reload();
              return Task.CompletedTask;
        }
    }
}
