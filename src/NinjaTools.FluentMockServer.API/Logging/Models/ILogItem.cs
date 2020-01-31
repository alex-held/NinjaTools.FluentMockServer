using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.Logging.Models
{
    /// <inheritdoc />
    public interface ILogItem : IIdentifable<string>
    {
        public string ToFormattedString();

        LogKind Kind { get; }

        LogType Type => Kind switch
        {
            LogKind.RequestMatched => LogType.Request,
            LogKind.RequestUnmatched => LogType.Request,
            LogKind.SetupCreated => LogType.Setup,
            LogKind.SetupDeleted => LogType.Setup
        };
    }
}

