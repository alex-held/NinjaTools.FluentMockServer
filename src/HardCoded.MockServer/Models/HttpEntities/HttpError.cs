using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Models.ValueTypes;

namespace HardCoded.MockServer.Models.HttpEntities
{
    public class HttpError  : BuildableBase
    {
        public Delay Delay { get; set; }

        public bool DropConnection { get; set; }

        public string ResponseBytes { get; set; }
    }
}
