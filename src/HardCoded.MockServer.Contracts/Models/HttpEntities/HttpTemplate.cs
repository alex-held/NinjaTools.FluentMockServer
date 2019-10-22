using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Contracts.Models.ValueTypes;


namespace HardCoded.MockServer.Contracts.Models.HttpEntities
{
    public class HttpTemplate : BuildableBase
    {
        public string Template { get; set; }

        public Delay Delay { get; set; }
    }
}
