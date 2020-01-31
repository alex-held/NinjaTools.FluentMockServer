using System.Threading.Tasks;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.Configuration
{
    public class ConfigurationInitializer : IInitializer
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
