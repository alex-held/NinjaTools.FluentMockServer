using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.Logging
{
    public static class MockServerPaths
    {
        private const string LogRoot = "/var/log/mock-server";

        public static class Logs
        {
            public static readonly string Setups = Path.Combine(LogRoot, "setups");
            public static readonly string Requests = Path.Combine(LogRoot, "requests");
        }

        public static string Configs { get; } = "/etc/mock-server/config/";
    }


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
