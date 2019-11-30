using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Domain.Models.HttpEntities
{
    public partial class HttpTemplate
    {
        public string Template { get; set; }

        public Delay Delay { get; set; }
    }
}
