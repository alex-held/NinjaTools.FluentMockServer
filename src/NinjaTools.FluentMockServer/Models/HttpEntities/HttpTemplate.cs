using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Models.ValueTypes;


namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
    public class HttpTemplate : BuildableBase
    {
        public string Template { get; set; }

        public Delay Delay { get; set; }
    }
}
