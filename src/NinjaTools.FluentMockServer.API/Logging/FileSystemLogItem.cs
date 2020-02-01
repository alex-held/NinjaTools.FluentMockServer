using System.Diagnostics;
using NinjaTools.FluentMockServer.API.Logging.Models;

namespace NinjaTools.FluentMockServer.API.Logging
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
