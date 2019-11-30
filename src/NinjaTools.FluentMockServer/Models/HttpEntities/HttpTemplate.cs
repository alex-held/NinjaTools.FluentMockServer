using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
    public class HttpTemplate
    {
        public string Template { get; set; }

        public Delay Delay { get; set; }
    }
}
