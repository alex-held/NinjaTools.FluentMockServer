using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Logging.Models
{
    public sealed class SetupLog : LogItem<Setup>
    {
        /// <inheritdoc />
        public SetupLog(string id, Setup content, LogKind  kind) : base(id, content)
        {
            Kind = kind;
        }

        public override LogKind Kind { get; }
    }
}
