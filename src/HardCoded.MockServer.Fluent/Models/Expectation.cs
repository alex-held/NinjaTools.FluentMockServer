using HardCoded.MockServer.Fluent.Builder;

namespace HardCoded.MockServer.Fluent.Models
{
    public class Expectation
    {
        public HttpRequest HttpRequest { get; set; }
        public HttpResponse HttpResponse { get; set; }
    }
}