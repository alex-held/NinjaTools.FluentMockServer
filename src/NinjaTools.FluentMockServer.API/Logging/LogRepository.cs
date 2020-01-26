using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using NinjaTools.FluentMockServer.API.Infrastructure;
using NinjaTools.FluentMockServer.API.Logging.Models;

namespace NinjaTools.FluentMockServer.API.Logging
{
    /// <inheritdoc />
    public class LogRepository : ILogRepository
    {
        private readonly IFileSystem _fileSystem;
        private readonly List<FileSystemLogItem> _logItems;

        private static readonly string RequestLogDirectory = MockServerPaths.Logs.Requests;
        private static readonly string SetupLogDirectory = MockServerPaths.Logs.Setups;

        public LogRepository() : this(new FileSystem())
        { }

        public LogRepository(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _logItems = new List<FileSystemLogItem>();
        }

        /// <inheritdoc />
        public IEnumerable<ILogItem> Get()
        {
            return _logItems.Select(log => log.Log);
        }

        private string GetNextPath(ILogItem log)
        {
            var path = Path.Combine(log.Type switch
            {
                LogType.Request => RequestLogDirectory,
                LogType.Setup => SetupLogDirectory
            }, $"{log.Id}.log");

            return path;
        }

        /// <inheritdoc />
        public void AddOrUpdate(ILogItem log)
        {
            var logPath = GetNextPath(log);
            var content = log.ToFormattedString();

            _fileSystem.File.WriteAllText(logPath, content);
            var fsLogItem = new FileSystemLogItem(log, logPath);
            _logItems.Add(fsLogItem);
        }

        /// <inheritdoc />
        public IEnumerable<ILogItem> Prune(LogType type)
        {
            var pruned = new List<ILogItem>();

            foreach (var logItem in _logItems.Where(log => log.LogType == type))
            {
                _fileSystem.File.Delete(logItem.Path);
                pruned.Add(logItem.Log);
            }

            _logItems.RemoveAll(log => log.LogType == type);
            return pruned;
        }

        /// <inheritdoc />
        public IEnumerable<ILogItem> Prune()
        {
            var pruned = new List<ILogItem>();

            foreach (var logItem in _logItems)
            {
                _fileSystem.File.Delete(logItem.Path);
                pruned.Add(logItem.Log);
            }

            _logItems.Clear();
            return pruned;
        }
    }
}
