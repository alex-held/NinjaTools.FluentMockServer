using System.Net.Http;
using NinjaTools.FluentMockServer.Domain.Models;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using NinjaTools.FluentMockServer.Domain.Serialization;

namespace NinjaTools.FluentMockServer.Domain.Utils
{
    public static class RequestFactory
    {
        public static HttpRequestMessage Reset() => new HttpRequestMessage(HttpMethod.Put, MockServerEndpoints.Reset);
        
        public static HttpRequestMessage Verify(Verify verify) =>  new HttpRequestMessage(HttpMethod.Put, MockServerEndpoints.Verify)
        {
            Content = new JsonContent(verify)
        };
        
        public static HttpRequestMessage Expectation(Expectation expectation) =>  new HttpRequestMessage(HttpMethod.Put, MockServerEndpoints.Expectations)
        {
            Content = new JsonContent(expectation)
        };
        
    }
 
}
