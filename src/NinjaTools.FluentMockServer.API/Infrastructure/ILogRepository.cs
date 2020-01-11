using System.Collections.Generic;
using NinjaTools.FluentMockServer.API.Models.Logging;

namespace NinjaTools.FluentMockServer.API.Services
{
    public interface ILogRepository
    {
        IEnumerable<ILogItem> Get();

        public void AddOrUpdate(ILogItem log);

        public IEnumerable<ILogItem> Prune(LogType type);
        public IEnumerable<ILogItem> Prune();
    }
}
