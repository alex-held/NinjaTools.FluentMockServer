using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Logging.Models
{
    /// <inheritdoc />
    public sealed class SetupLog : LogItem<Setup>
    {
        /// <inheritdoc />
        public SetupLog(string id, Setup content, LogKind  kind) : base(id, content)
        {
            Kind = kind;
        }

        /// <inheritdoc />
        public override LogKind Kind { get; }
    }
}
