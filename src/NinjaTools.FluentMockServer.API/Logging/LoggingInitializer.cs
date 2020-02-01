using System.IO.Abstractions;
using System.Threading.Tasks;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.Logging
{
    /// <inheritdoc />
    public class LoggingInitializer : IInitializer
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of <see cref="LoggingInitializer"/>.
        /// </summary>
        /// <param name="fileSystem"></param>
        internal LoggingInitializer(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <inheritdoc />
        public Task InitializeAsync()
        {
            var setupLogsPath = MockServerPaths.Logs.Setups;
            var requestLogsPath  = MockServerPaths.Logs.Requests;

            if (_fileSystem.Directory.Exists(setupLogsPath))
            {
                _fileSystem.Directory.Delete(setupLogsPath, true);
            }

            _fileSystem.Directory.CreateDirectory(setupLogsPath);

            if (_fileSystem.Directory.Exists(requestLogsPath))
            {
                _fileSystem.Directory.Delete(requestLogsPath, true);
            }
            _fileSystem.Directory.CreateDirectory(requestLogsPath);

            return Task.CompletedTask;
        }
    }
}
