using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.API.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace NinjaTools.FluentMockServer.API.Serialization.Converters
{
    /// <inheritdoc cref="IYamlTypeConverter" />
    public class PathConverter : JsonConverter<Path>, IYamlTypeConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, Path value, JsonSerializer serializer)
        {
            var jt = JToken.FromObject(value.ToPath());
            if (jt.Type != JTokenType.Object)
            {
                jt.WriteTo(writer);
            }
        }

        /// <inheritdoc />
        public override Path ReadJson(JsonReader reader, Type objectType, Path existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = JToken.ReadFrom(reader);
            return new Path(value.ToString());
        }

        public bool Accepts(Type type) => typeof(Path) == type;

        /// <inheritdoc />
        public object? ReadYaml(IParser parser, Type type)
        {
            var value = parser.Consume<Scalar>().Value;
            var pathCollection = new Path(value);
            return pathCollection;
        }

        /// <inheritdoc />
        public void WriteYaml(IEmitter emitter, object? value, Type type)
        {
            var pathCollection = (Path) value!;
            var path = pathCollection.ToPath();
            emitter.Emit(new Scalar(null, null, path, ScalarStyle.Any, true, false));
        }
    }
}
