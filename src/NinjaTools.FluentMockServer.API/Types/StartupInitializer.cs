using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace NinjaTools.FluentMockServer.API.Types
{
    internal sealed class StartupInitializer : IStartupInitializer
    {
        private readonly ILogger<IStartupInitializer> _logger;
        private readonly List<IInitializer> _initializers;

        public StartupInitializer(ILogger<IStartupInitializer> logger)
        {
            _initializers = new List<IInitializer>();
            _logger = logger;
        }


        /// <inheritdoc />
        public async Task InitializeAsync()
        {
            foreach (var initializer in _initializers)
            {
                await initializer.InitializeAsync();
            }
        }

        /// <inheritdoc />
        public void AddInitializer<TInitializer>([NotNull] TInitializer initializer) where TInitializer : class, IInitializer
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
