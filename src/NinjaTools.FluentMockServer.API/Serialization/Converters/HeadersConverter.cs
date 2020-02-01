using System;
using System.Collections.Generic;
using NinjaTools.FluentMockServer.API.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace NinjaTools.FluentMockServer.API.Serialization.Converters
{
    /// <inheritdoc />
    public class HeadersConverter : IYamlTypeConverter
    {
        /// <inheritdoc />
        public bool Accepts(Type type) => typeof(Headers) == type;

        /// <inheritdoc />
        public object? ReadYaml(IParser parser, Type type)
        {
            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .Build();

            var headers = deserializer.Deserialize<Dictionary<string, string[]>>(parser);
            var headerCollection = new Headers(headers);
            return headerCollection;
        }

        /// <inheritdoc />
        public void WriteYaml(IEmitter emitter, object? value, Type type)
        {
            var serializer = new SerializerBuilder()
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                .EnsureRoundtrip()
                .Build();

            var headerCollection = (Headers) value!;
            var headers = (IDictionary<string, string[]>) headerCollection;

            serializer.Serialize(emitter, headers);
        }
    }
}
