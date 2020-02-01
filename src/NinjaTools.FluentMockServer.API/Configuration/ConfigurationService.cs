using System.Collections.Generic;
using NinjaTools.FluentMockServer.API.Services;

namespace NinjaTools.FluentMockServer.API.Configuration
{
    public interface IConfigurationService
    {
        void Reload();
    }


    public class ConfigurationService : IConfigurationService
    {
        private readonly ISetupService _setupService;
        private readonly IConfigFileProvider _configFileProvider;

        public ConfigurationService(ISetupService setupService, IConfigFileProvider configFileProvider)
        {
            _setupService = setupService;
            _configFileProvider = configFileProvider;
        }

        public void Apply(IEnumerable<IConfigFile> configFiles)
        {
            foreach (var configFile in configFiles)
            {
                foreach (var setup in configFile.Configurations)
                {
                    _setupService.Add(setup);
                }
            }
        }

        public void Reload()
        {
           var configFiles =  _configFileProvider.GetConfigFiles();
           Apply(configFiles);
        }
    }
}
