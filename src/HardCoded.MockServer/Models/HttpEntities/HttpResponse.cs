using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Models.ValueTypes;

namespace HardCoded.MockServer.Models.HttpEntities
{
    public class HttpResponse : BuildableBase
    {
        public HttpResponse(int statusCode)
        {
            StatusCode = statusCode;
            ConnectionOptions = new ConnectionOptions();
            Delay = Delay.None;
        }
        
        public Delay Delay { get; set; }
        public ConnectionOptions ConnectionOptions { get; set; }
        public int StatusCode { get; set; }
        public RequestBody Body { get; set; }
    }
}
