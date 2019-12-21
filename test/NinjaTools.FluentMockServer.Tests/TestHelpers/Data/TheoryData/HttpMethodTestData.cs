using System.Net.Http;
using Xunit;

namespace NinjaTools.FluentMockServer.Tests.TestHelpers.Data.TheoryData
{
    // public class HttpMethodTestData : TheoryData<HttpMethod, string>
    // {
    //     public HttpMethodTestData()
    //     {
    //         Add(HttpMethod.Delete,  "DELETE");
    //         Add(HttpMethod.Put,     "PUT");
    //         Add(HttpMethod.Post,    "POST");
    //         Add(HttpMethod.Get,     "GET");
    //         Add(HttpMethod.Head,    "HEAD");
    //         Add(HttpMethod.Options, "OPTIONS");
    //         Add(HttpMethod.Patch,   "PATCH");
    //         Add(HttpMethod.Trace,   "TRACE");
    //         Add(HttpMethod.Connect,   "CONNECT");
    //         Add(HttpMethod.Custom,   "CUSTOM");
    //         Add(new HttpMethod(), "GET");
    //     }
    // }

    public class HttpMethodTestData : TheoryData<string, HttpMethod>
    {
        public HttpMethodTestData()
        {
            Add("DELETE", HttpMethod.Delete);
            Add("PUT", HttpMethod.Put);
            Add("POST", HttpMethod.Post);
            Add("GET", HttpMethod.Get);
            Add("HEAD", HttpMethod.Head);
            Add("OPTIONS", HttpMethod.Options);
            Add("PATCH", HttpMethod.Patch);
            Add("TRACE", HttpMethod.Trace);
            // Add("CONNECT", new HttpMethod("CONNECT"));
            // Add("CUSTOM", new HttpMethod("CUSTOM"));
            // Add("GET", new HttpMethod("GET"));
        }
    }


    // public class HttpMethodTest : TheoryData<HttpMethod>
    // {
    //     public HttpMethodTest()
    //     {
    //         Add(HttpMethod.Delete,  "DELETE");
    //         Add(HttpMethod.Put,     "PUT");
    //         Add(HttpMethod.Post,    "POST");
    //         Add(HttpMethod.Get,     "GET");
    //         Add(HttpMethod.Head,    "HEAD");
    //         Add(HttpMethod.Options, "OPTIONS");
    //         Add(HttpMethod.Patch,   "PATCH");
    //         Add(HttpMethod.Trace,   "TRACE");
    //         Add(HttpMethod.Connect,   "Connect");
    //         Add(HttpMethod.Custom,   "MOCKSERVER");
    //         Add(HttpMethod.None,  null);
    //     }
    // }
}
