using System;
using System.Linq;
using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Contracts.FluentInterfaces;
using HardCoded.MockServer.Fluent.Builder;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace HardCoded.MockServer.Tests.Utils
{
    internal abstract class SerializationContainer<TBuilder, TFluentBuilder, TBuildable>
        where TBuilder : class, TFluentBuilder, new()
        where TFluentBuilder : IFluentBuilder<TBuildable>, IFluentInterface
        where TBuildable : class, IBuildable
    {
        protected ITestOutputHelper OutputHelper;
        protected bool? ToBeInversed { get; set; }
        public abstract bool CanInvert { get; }
        public IBuildable Content { get; protected set; }
        
        public SerializationContainer(ITestOutputHelper outputHelper, Action<TFluentBuilder> factory, bool? invert = null)
        {
            OutputHelper = outputHelper;
            var builder = new TBuilder();
            factory(builder);
            Content = builder.Build();
            ToBeInversed = invert;
        }
        
        public JObject Deserialize()
        {
            var envelope = JObject.Parse(Envelope);
            var body = envelope
                      .Children()
                      .OfType<JProperty>()
                      .First(p => p.Name == Keyname).Value
                      .Value<JValue>();
            
            var contentObject = Content.Serialize();
            body.Replace(contentObject);
            
            return envelope;
        }
        
        public abstract string Envelope { get; }
        public abstract string Keyname { get; }
        public abstract JObject Invert(string expected);
        protected abstract JToken GetPlaceholder(JObject jObject);
    }
}