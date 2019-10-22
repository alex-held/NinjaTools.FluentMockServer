
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using HardCoded.MockServer.Contracts;
using HardCoded.MockServer.Contracts.Models;
using HardCoded.MockServer.Fluent.Builder.Expectation;

using Newtonsoft.Json;

namespace HardCoded.MockServer.Fluent
{

    public class MockServerSetup
    {
        public List<Expectation> Expectations { get; } = new List<Expectation>();
        
        public static implicit operator HttpRequestMessage(MockServerSetup setup)
        {
            var expectations = setup.Expectations.ToList();
            var content = JsonConvert.SerializeObject(expectations, Formatting.Indented);

            return new HttpRequestMessage(HttpMethod.Put, new Uri("expectations", UriKind.Relative))
            {
                        Content = new StringContent(content, Encoding.Default, CommonContentType.Json)
            };
        }
    }

    public class MockServerBootstrap
    {
        private static MockServerSetup Setup = new MockServerSetup();
        public static IFluentExpectationBuilder Expectations => new FluentExpectationBuilder(Setup);
    }
}
