using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NinjaTools.FluentMockServer.API.Helper;
using NinjaTools.FluentMockServer.API.Models.Logging;

namespace NinjaTools.FluentMockServer.API.Services
{
    internal sealed class LogService : ILogService
    {
        private readonly ILogger<LogService> _logger;
        private readonly ILogRepository _logRepository;
        private readonly LogFactory _factory;

        public LogService(ILogger<LogService> logger, ILogRepository logRepository) : this(logger,logRepository ,new LogFactory())
        { }

        public LogService(ILogger<LogService> logger, ILogRepository logRepository, LogFactory logFactory)
        {
            _logger = logger;
            _logRepository = logRepository;
            _factory = logFactory;
        }

        /// <inheritdoc />
        public void Log<T>([NotNull] Func<LogFactory, LogItem<T>> logFactory)
        {
            var logItem = logFactory(_factory);
            _logger.LogInformation(logItem.ToFormattedString());
            _logRepository.AddOrUpdate(logItem);
        }

        public IEnumerable<T> Get<T>(Func<T, bool> predicate) where T : class, ILogItem
        {
            return _logRepository.Get()
                .OfType<T>()
                .Where(log => predicate(log));
        }

        public IEnumerable<ILogItem> Prune()
        {
            var logs = _logRepository.Get();
            _logRepository.Prune();
            return logs;
        }

        public IEnumerable<ILogItem> Prune(LogType type)
        {
            var pruned = _logRepository.Prune(type);
            return pruned;
        }

        /// <inheritdoc />
        public IEnumerable<ILogItem> Get() => _logRepository.Get();
    }
}
