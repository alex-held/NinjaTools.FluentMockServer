using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;

namespace NinjaTools.FluentMockServer.Serialization
{
    public class ConcreteTypeConverter<TConcrete> : JsonConverter where TConcrete : class, IBuildable
    {
        [ThreadStatic]
        static bool disabled;

        // Disables the converter in a thread-safe manner.
        bool Disabled { get { return disabled; } set { disabled = value; } }

        public override bool CanWrite { get { return !Disabled; } }


        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        /// <inheritdoc />
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var json = JObject.Load(reader);

            if (!typeof(IBuildable).IsAssignableFrom(objectType))
            {
                return null;
            }

            var instance = Activator.CreateInstance(objectType);

            // Populate object.
            using (var subReader = json.CreateReader())
            {
                serializer.Populate(subReader, instance);
            }

            return instance as TConcrete;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType) => typeof(IBuildable).IsAssignableFrom(objectType);
    }
}