using System.Net.Http;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Serialization;

namespace NinjaTools.FluentMockServer.Utils
{
    internal static class MockServerEndpoints
    {
        public const string Expectations = "mockserver/expectation";
        public const string Reset  = "mockserver/reset";
        public const string Verify  = "mockserver/verify";
    }
    
    internal static class RequestFactory
    {
        
        [NotNull]
        public static HttpRequestMessage GetResetMessage() => new HttpRequestMessage(HttpMethod.Put, MockServerEndpoints.Reset);
        
        [NotNull]
        public static HttpRequestMessage GetVerifyRequest([NotNull] Verify verify) =>  new HttpRequestMessage(HttpMethod.Put, MockServerEndpoints.Verify)
        {
            Content = new JsonContent(verify)
        };
        
        [NotNull]
        public static HttpRequestMessage GetExpectationMessage([NotNull] Expectation expectation) =>  new HttpRequestMessage(HttpMethod.Put, MockServerEndpoints.Expectations)
        {
            Content = new JsonContent(expectation)
        };
    }
 
}
