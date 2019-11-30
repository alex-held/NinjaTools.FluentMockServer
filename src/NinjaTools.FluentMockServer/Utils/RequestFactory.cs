using System;
using System.Collections.Generic;
using System.Net.Http;
using NinjaTools.FluentMockServer.Client.Models;
using NinjaTools.FluentMockServer.Client.Models.HttpEntities;
using NinjaTools.FluentMockServer.Client.Serialization;
using NinjaTools.FluentMockServer.Utils;

namespace NinjaTools.FluentMockServer.Requests
{
    public static class MockServerEndpoints
    {
        public const string Expectations = "mockserver/expectation";
        public const string Reset  = "mockserver/reset";
        public const string Verify  = "mockserver/verify";
    }

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
