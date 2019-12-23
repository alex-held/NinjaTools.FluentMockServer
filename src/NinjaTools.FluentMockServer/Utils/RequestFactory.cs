using System.Net.Http;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Serialization;

namespace NinjaTools.FluentMockServer.Utils
{
    public static class MockServerEndpoints
    {
        public const string Expectations = "mockserver/expectation";
        public const string Reset  = "mockserver/reset";
        public const string Verify  = "mockserver/verify";
    }
    
    public static class RequestFactory
    {
        
        [NotNull]
        public static HttpRequestMessage Reset() => new HttpRequestMessage(HttpMethod.Put, MockServerEndpoints.Reset);
        
        [NotNull]
        public static HttpRequestMessage Verify([NotNull] Verify verify) =>  new HttpRequestMessage(HttpMethod.Put, MockServerEndpoints.Verify)
        {
            Content = new JsonContent(verify)
        };
        
        [NotNull]
        public static HttpRequestMessage Expectation([NotNull] Expectation expectation) =>  new HttpRequestMessage(HttpMethod.Put, MockServerEndpoints.Expectations)
        {
            Content = new JsonContent(expectation)
        };
    }
 
}
