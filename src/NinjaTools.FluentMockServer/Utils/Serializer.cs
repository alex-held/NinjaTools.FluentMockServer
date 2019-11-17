using JetBrains.Annotations;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace NinjaTools.FluentMockServer.Utils
{
     /// <inheritdoc />
     ///
     public class Serializer : JsonSerializer
    {
        public Serializer()
        {
#if DEBUG
            JsonConvert.DefaultSettings = () => JsonConfig.Instance;
#endif
        }

        /// <summary>
        /// Gets the default <see cref="Serializer"/> Instance.
        /// </summary>
        public static JsonSerializer Default => CreateDefault(JsonConfig.Instance);
        
        /// <summary>
        /// Gets the <see cref="Config"/>.
        /// </summary>
        public static JsonConfig Config { get; set; }

        
        /// <summary>
        /// Serializes an <see cref="object"/> into an
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static JObject Serialize([NotNull] object o)  => JObject.FromObject(o, CreateDefault(Config));
        
        
        public static T Deserialize<T>([NotNull] string json) => JObject.Parse(json).ToObject<T>(Default);
    }
}
