
using System;
using System.Collections.Concurrent;
using System.Linq;
using HardCoded.MockServer.Fluent.Builder.Expectation;
using HardCoded.MockServer.Models;
using HardCoded.MockServer.Requests;
using Newtonsoft.Json;

namespace HardCoded.MockServer.Fluent
{

    public class MockServerSetup
    {
        [JsonIgnore]
        private static readonly Lazy<MockServerSetup> _lazy = new Lazy<MockServerSetup>(() => Init());
    
        [JsonIgnore]
        internal static MockServerSetup Setup => _lazy.Value;
       
        [JsonIgnore]
        private readonly ConcurrentBag<Expectation> _expectations;
      
        [JsonIgnore]
        public Func<Expectation, MockServerSetup> UpdateExpectation { get; }

        
        public static MockServerSetup Init() => new MockServerSetup();
        
        internal MockServerSetup()
        {
            _expectations = new ConcurrentBag<Expectation>();
            UpdateExpectation = expectation =>
            {
                 _expectations.Add(expectation);
                 return Setup;
            };
        }
        
        [JsonIgnore]
        public ExpectationRequest ExpectationRequest => new ExpectationRequest(_expectations);


        /// <inheritdoc />
        public override string ToString()
        {
            return JsonConvert.SerializeObject(_expectations.ToList(), Formatting.Indented);
        }
    }

    public class MockServerBootstrap
    {
        public static IFluentExpectationBuilder Expectations => new FluentExpectationBuilder(MockServerSetup.Setup.UpdateExpectation);
    }
}