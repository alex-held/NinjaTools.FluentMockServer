using System.Diagnostics;
using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.API.Models
{
    /// <summary>
    /// Defines how the MockServer responds zu a matched requests.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay()}")]
    public class ResponseAction
    {
        private string DebuggerDisplay() =>  $"{Response?.DebuggerDisplay()}";
        public HttpResponse Response { get; set; }
    }

    [DebuggerDisplay("{DebuggerDisplay()}")]
    public class HttpResponse
    {
        [NotNull]
        public string DebuggerDisplay() => $"Status={StatusCode}; Body={Body}";
        public int StatusCode { get; set; }
        public string Body { get; set; }
    }
}
