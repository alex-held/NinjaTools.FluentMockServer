using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.API.Helper;
using NinjaTools.FluentMockServer.API.Models.Logging;

namespace NinjaTools.FluentMockServer.API.Services
{
    public interface ILogService
    {
        void Log<T>([NotNull] Func<LogFactory, LogItem<T>> logFactory);

        IEnumerable<T> Get<T>([NotNull] Func<T, bool> predicate) where T : class, ILogItem;

        IEnumerable<ILogItem> OfType(LogType type) => Get().Where(l => l.Type == type);
        IEnumerable<T> OfType<T>() where T : class, ILogItem => Get().OfType<T>();

        IEnumerable<ILogItem> Prune();
        IEnumerable<ILogItem> Prune(LogType type);
        IEnumerable<ILogItem> Get();
    }
}
