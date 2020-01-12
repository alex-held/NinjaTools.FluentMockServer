using System.Collections.Generic;
using NinjaTools.FluentMockServer.API.Logging.Models;

namespace NinjaTools.FluentMockServer.API.Infrastructure
{
    public interface ILogRepository
    {
        IEnumerable<ILogItem> Get();

        public void AddOrUpdate(ILogItem log);

        public IEnumerable<ILogItem> Prune(LogType type);
        public IEnumerable<ILogItem> Prune();
    }
}
