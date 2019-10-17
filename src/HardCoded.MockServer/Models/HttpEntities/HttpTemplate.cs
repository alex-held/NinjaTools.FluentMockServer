using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Models.ValueTypes;

namespace HardCoded.MockServer.Models.HttpEntities
{
    public class HttpTemplate : BuildableBase
    {
        public string Template { get; set; }

        public Delay Delay { get; set; }
    }
}
