using System.IO.Abstractions;
using System.Threading.Tasks;
using NinjaTools.FluentMockServer.API.DependencyInjection;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.Logging
{
    /// <summary>
    /// Marker interface to identify and disable <see cref="LoggingInitializer"/>  at startup,
    /// when configured in <seealso cref="ServiceCollectionExtensions.AddInitializers"/>.
    /// </summary>
    public interface ILoggingInitializer : IInitializer
    { }

    /// <inheritdoc />
    public class LoggingInitializer : ILoggingInitializer
    {
        private readonly IFileSystem _fileSystem;
        private readonly Paths  _paths;

        /// <summary>
        /// Initializes a new instance of <see cref="LoggingInitializer"/>.
        /// </summary>
        /// <param name="fileSystem"></param>
        internal LoggingInitializer(IFileSystem fileSystem, Paths paths)
        {
            _fileSystem = fileSystem;
            _paths = paths;
        }

        /// <inheritdoc />
        public Task InitializeAsync()
        {
            var setupLogsPath = _paths.LOG_PATH;
            var requestLogsPath  = _paths.LOG_PATH;

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
