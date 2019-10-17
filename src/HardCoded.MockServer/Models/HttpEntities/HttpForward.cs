using HardCoded.MockServer.Contracts.Abstractions;


namespace HardCoded.MockServer.Models.HttpEntities
{
    public class HttpForward : BuildableBase
    {
        public string Host { get; set; }
        
        public long Port { get; set; }
    }
}
