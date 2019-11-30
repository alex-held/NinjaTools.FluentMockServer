using NinjaTools.FluentMockServer.Client.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Client.Models.HttpEntities
{
    public partial class HttpTemplate
    {
        public string Template { get; set; }

        public Delay Delay { get; set; }
    }
}
