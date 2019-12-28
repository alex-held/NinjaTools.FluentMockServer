namespace NinjaTools.FluentMockServer.API.Models
{
    public class ResponseAction 
    {
        public HttpResponse Response { get; set; }
    }

    public class HttpResponse
    {
        public int StatusCode { get; set; }
        public string Body { get; set; }
    }
}
