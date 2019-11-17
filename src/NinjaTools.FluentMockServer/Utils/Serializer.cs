using JetBrains.Annotations;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace NinjaTools.FluentMockServer.Utils
{
    /// <inheritdoc />
     public class Serializer : JsonSerializer
    {
        /// <summary>
        /// Gets the default <see cref="Serializer"/> Instance.
        /// </summary>
        public static Serializer Default => new Serializer(new CustomJsonSerializerSettings());
        
        /// <summary>
        /// Gets the <see cref="CustomJsonSerializerSettings"/>.
        /// </summary>
        public CustomJsonSerializerSettings CustomJsonSerializerSettings { get; }


        /// <inheritdoc />
        public Serializer(CustomJsonSerializerSettings customJsonSerializerSettings) => 
                    CustomJsonSerializerSettings = customJsonSerializerSettings;
        
        /// <summary>
        /// Serializes an <see cref="object"/> into an
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static JObject Serialize([NotNull] object o)  => JObject.FromObject(o, Default);
        public static T Deserialize<T>([NotNull] string json) => JObject.Parse(json).ToObject<T>(Default);
    }
}
