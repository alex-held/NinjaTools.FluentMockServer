
using System;
using System.Collections.Generic;
using System.Net.Http;
using HardCoded.MockServer.Fluent.Builder;

namespace HardCoded.MockServer.Fluent
{
    public class MockServerSetup
    {
        private readonly static List<Expectation> _expectations = new List<Expectation>();
        public static IFluentExpectationBuilder Expectations => new FluentExpectationBuilder(_expectations);
    }
//
//    internal class FluentExpectationBuilder : IFluentExpectationBuilder 
//    {
//        private readonly List<Expectation> _expectations;
//
//        private Expectation _expectation;
//        public FluentExpectationBuilder(List<Expectation> expectations)
//        {
//            _expectations = expectations;
//            _expectation = new Expectation();
//        }
//
//        /// <inheritdoc />
//        public IWithRequest OnHandling(HttpMethod method, Action<IFluentHttpRequestBuilder> responseFactory)
//        {
//            var builder = new FluentHttpRequestBuilder(method);
//            responseFactory(builder);
//            _expectation.HttpRequest = builder.Build();
//            return this;
//        }
//    }
//    
//    public interface IFluentExpectationBuilder
//    {
//        IWithRequest OnHandling(HttpMethod method, Action<IFluentHttpRequestBuilder> responseFactory);
//    }
    
}