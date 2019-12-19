using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NinjaTools.FluentMockServer.Serialization
{
    public static class Serializer
    {
        private static Lazy<ProxySerializer> _lazy = new Lazy<ProxySerializer>(() => new ProxySerializer());

        public static JsonSerializerSettings SerializerSettings
        {
            get => ProxySerializer.Default;
            set { _lazy = new Lazy<ProxySerializer>(() => new ProxySerializer(value)); }
        }

        public static JObject SerializeJObject<T>(T obj)
        {
            return JObject.Parse(Serialize(obj));
        }

        public static T Deserialize<T>(string json)
        {
            var stringReader = new StringReader(json);
            var jsonTextReader = new JsonTextReader(stringReader);
            var result = _lazy.Value.GetSerializer().Deserialize<T>(jsonTextReader);
            return result;
        }


        public static string Serialize<T>(T obj)
        {
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var jsonTextWriter = new JsonTextWriter(stringWriter);
            _lazy.Value.GetSerializer().Serialize(jsonTextWriter, obj, typeof(T));
            var result = sb.ToString();
            return result;
        }

        /// <inheritdoc />
        private class ProxySerializer : JsonSerializer
        {
            static ProxySerializer()
            {
                Default = new JsonConfig();
            }


            public ProxySerializer(JsonSerializerSettings settings = null)
            {
                Settings = settings;
            }

            public JsonSerializerSettings Settings { get; }

            /// <summary>
            ///     Gets the <see cref="JsonConfig" />.
            /// </summary>
            public static JsonConfig Default { get; }

            public JsonSerializer GetSerializer()
            {
                return CreateDefault(Settings ?? Default);
            }
        }
    }
}
