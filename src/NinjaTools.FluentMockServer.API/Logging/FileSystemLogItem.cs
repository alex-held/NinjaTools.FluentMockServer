using System.Diagnostics;
using NinjaTools.FluentMockServer.API.Models.Logging;

namespace NinjaTools.FluentMockServer.API.Services
{
    [DebuggerDisplay("Type={LogType}; Path={Path}; Log={Log};")]
    public class FileSystemLogItem
    {
        public FileSystemLogItem(ILogItem log, string path)
        {
            Log = log;
            Path = path;
        }

        public ILogItem Log { get; }
        public string Path { get; }
        public LogType LogType => Log.Type;
    }
}
