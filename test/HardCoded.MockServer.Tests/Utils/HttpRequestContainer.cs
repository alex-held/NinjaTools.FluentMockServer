using System;
using HardCoded.MockServer.Fluent.Builder.Request;
using HardCoded.MockServer.Models.HttpEntities;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace HardCoded.MockServer.Tests.Utils
{
    internal class HttpRequestContainer : SerializationContainer<FluentHttpRequestBuilder, IFluentHttpRequestBuilder, HttpRequest>
    {
        /// <inheritdoc />
        public HttpRequestContainer(ITestOutputHelper outputHelper,
            Action<IFluentHttpRequestBuilder> factory) : base(outputHelper, factory) { }
        
        /// <inheritdoc />
        public override bool CanInvert => false;

        /// <inheritdoc />
        protected override JToken GetPlaceholder(JObject jObject) =>
            jObject.GetValue(Keyname).Value<JObject>();

        /// <inheritdoc />
        public override string Envelope => @"{
            ""httpRequest"": {
                ""content"": ""here""
            }
}";

        /// <inheritdoc />
        public override string Keyname { get; }

        /// <inheritdoc />
        public override JObject Invert(string expected)
        {
            return JObject.Parse(expected);
        }

    }
}