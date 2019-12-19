using System.Net.Http;
using Xunit;

namespace NinjaTools.FluentMockServer.Tests.TestHelpers.Data.TheoryData
{
    public class HttpMethodTestData : TheoryData<HttpMethod, string>
    {
        public HttpMethodTestData()
        {
            Add(HttpMethod.Delete,  "DELETE");
            Add(HttpMethod.Put,     "PUT");
            Add(HttpMethod.Post,    "POST");
            Add(HttpMethod.Get,     "GET");
            Add(HttpMethod.Head,    "HEAD");
            Add(HttpMethod.Options, "OPTIONS");
            Add(HttpMethod.Patch,   "PATCH");
            Add(HttpMethod.Trace,   "TRACE");
        }
    }
}
