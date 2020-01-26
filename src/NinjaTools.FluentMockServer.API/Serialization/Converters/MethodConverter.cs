using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.API.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace NinjaTools.FluentMockServer.API.Serialization.Converters
{
    /// <inheritdoc cref="JsonConverter{T}" />
    public class MethodConverter : JsonConverter<Method>, IYamlTypeConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, Method value, JsonSerializer serializer)
        {
            var t = JToken.FromObject(value?.MethodString);
            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
        }

        /// <inheritdoc />
        public override Method ReadJson(JsonReader reader, Type objectType, Method existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize<string>(reader);
            return new Method(value);
        }


        /// <inheritdoc />
        public bool Accepts(Type type) => typeof(Method) == type;

        /// <inheritdoc />
        public object? ReadYaml(IParser parser, Type type)
        {
            var value = parser.Consume<Scalar>().Value;
            var pathCollection = new Method(value);
            return pathCollection;
        }

        /// <inheritdoc />
        public void WriteYaml(IEmitter emitter, object? value, Type type)
        {
            var methodWrapper = (Method) value!;
            var path = methodWrapper.MethodString;
            emitter.Emit(new Scalar(null, null, path, ScalarStyle.Any, true, false));
        }
    }
}
