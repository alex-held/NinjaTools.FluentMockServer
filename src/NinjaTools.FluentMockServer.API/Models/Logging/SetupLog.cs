namespace NinjaTools.FluentMockServer.API.Models.Logging
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
