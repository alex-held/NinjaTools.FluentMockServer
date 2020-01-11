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
        private readonly List<ILogItem> _logs;
        private readonly ILogger<LogService> _logger;
        private readonly LogFactory _factory;

        public LogService(ILogger<LogService> logger) : this(logger, new LogFactory())
        {
            _logger = logger;
            _logs = new List<ILogItem>();
            _factory = new LogFactory();
        }

        public LogService(ILogger<LogService> logger, LogFactory logFactory)
        {
            _logger = logger;
            _logs = new List<ILogItem>();
            _factory = logFactory;
        }

        /// <inheritdoc />
        public void Log<T>([NotNull] Func<LogFactory, LogItem<T>> logFactory)
        {
            var logItem = logFactory(_factory);
            _logger.LogInformation(logItem.ToFormattedString());
            _logs.Add(logItem);
        }

        public IEnumerable<T> Get<T>(Func<T, bool> predicate) where T : class, ILogItem
        {
            return _logs
                .OfType<T>()
                .Where(log => predicate(log));
        }

        public IEnumerable<ILogItem> Prune()
        {
            var logs = _logs.ToList();

            _logs.Clear();

            return logs;
        }

        /// <inheritdoc />
        public IEnumerable<ILogItem> Get() => _logs.ToList();
    }
}
