using System.IO.Abstractions;
using System.Threading.Tasks;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.Logging
{
    /// <inheritdoc />
    public class LoggingInitializer : IInitializer
    {
        private readonly IFileSystem _fileSystem;

        public LoggingInitializer(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <inheritdoc />
        public async Task InitializeAsync()
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
        }
    }
}
