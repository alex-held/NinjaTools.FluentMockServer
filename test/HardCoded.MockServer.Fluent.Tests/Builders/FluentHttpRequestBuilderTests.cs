using System;
using HardCoded.MockServer.Fluent.Builder.Request;
using HardCoded.MockServer.Models.HttpEntities;
using HardCoded.MockServer.Tests.Utils;
using Xunit.Abstractions;

namespace HardCoded.MockServer.Fluent.Tests.Builders
{
    public class FluentHttpRequestBuilderTests
    {
        private readonly ITestOutputHelper _outputHelper;
        public FluentHttpRequestBuilderTests(ITestOutputHelper outputHelper) =>  _outputHelper = outputHelper;

        protected void Assert(string expected, Action<IFluentHttpRequestBuilder> factory)
        {
            AssertionHelpers<FluentHttpRequestBuilder, IFluentHttpRequestBuilder, HttpRequest>
               .Assert<HttpRequestContainer>(_outputHelper, expected, factory);
        }


    }
}