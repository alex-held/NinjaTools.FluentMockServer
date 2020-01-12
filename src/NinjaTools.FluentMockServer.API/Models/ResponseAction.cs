using System.Diagnostics;

namespace NinjaTools.FluentMockServer.API.Models
{
    [DebuggerDisplay("{DebuggerDisplay()}")]
    public class ResponseAction
    {
        public string DebuggerDisplay()
        {
            return $"{Response?.DebuggerDisplay()}";
        }

        public HttpResponse Response { get; set; }
    }

    [DebuggerDisplay("{DebuggerDisplay()}")]
    public class HttpResponse
    {
        public string DebuggerDisplay() => $"Status={StatusCode}; Body={Body}";
        public int StatusCode { get; set; }
        public string Body { get; set; }
    }
}
