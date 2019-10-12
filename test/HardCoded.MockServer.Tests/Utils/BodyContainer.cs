using System;
using System.Linq;
using HardCoded.MockServer.Fluent.Builder.Request;
using HardCoded.MockServer.Models;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace HardCoded.MockServer.Tests.Utils
{
  
    /// <inheritdoc />
    internal class BodyContainer : SerializationContainer<FluentBodyBuilder, IFluentBodyBuilder, RequestBody>
    {
        /// <inheritdoc />
        public BodyContainer(ITestOutputHelper outputHelper,
                             Action<IFluentBodyBuilder> factory, 
                             bool? invert = null) : base(outputHelper, factory, invert) { }

        /// <inheritdoc />
        public override bool CanInvert => true;
        
        /// <inheritdoc />
        public override string Envelope => @"{
    ""body"" : ""content"" 
}";
        /// <inheritdoc />
        public override string Keyname => "body";
        
        /// <inheritdoc />
        public override JObject Invert(string expected)
        {
            var root = JObject.Parse(expected);
            if (!CanInvert) 
                return root;
            if (ToBeInversed is null || (ToBeInversed == false) )
                return root;
            root["body"].Value<JObject>()
                        .AddFirst(new JProperty("not", true));
            return root;
        }

        /// <inheritdoc />
        protected override JToken GetPlaceholder(JObject jObject) =>
            JObject.Parse(Envelope)
                   .Children<JProperty>()
                   .First(prop => prop.Name == Keyname)
                   .Value.Value<JValue>();
    }
}