using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NinjaTools.FluentMockServer.API.Administration;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.Logging;

namespace NinjaTools.FluentMockServer.API.Types
{
    internal sealed class StartupInitializer : IStartupInitializer
    {
        private readonly ILogger<IStartupInitializer> _logger;
        private readonly List<IInitializer> _initializers;
        private readonly StartupInitializerOptions _options;

        public StartupInitializer(ILogger<IStartupInitializer> logger, StartupInitializerOptions options)
        {
            _initializers = new List<IInitializer>();
            _logger = logger;
            _options = options;
        }

        /// <inheritdoc />
        public Task InitializeAsync() => Task.WhenAll(_initializers.Where(_options.IsEnabled).Select(init => init.InitializeAsync()));

        /// <inheritdoc />
        public void AddInitializer<TInitializer>(TInitializer initializer) where TInitializer : class, IInitializer
        {
            if (_initializers.Contains(initializer))
            {
                var initializerName = typeof(TInitializer).Name;
                _logger.LogError($"Failed to add {nameof(IInitializer)} to {nameof(StartupInitializer)} container. Initilaizer={initializerName};");
                return;
            }

            _initializers.Add(initializer);
        }
    }
}
