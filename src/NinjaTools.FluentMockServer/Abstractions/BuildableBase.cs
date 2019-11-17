using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NinjaTools.FluentMockServer.Utils;


namespace NinjaTools.FluentMockServer.Abstractions
{
    /*

    public class BaseClassConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var j = JObject.Load(reader);
            var retval = JObjectSerializer.From(j, serializer);
            return retval;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override bool CanConvert(Type objectType)
        {
            // important - do not cause subclasses to go through this converter
            return objectType == typeof(JObjectSerializer);
        }
    }
    */

    /*

    public abstract class JObjectSerializer
    {
        public new static JObjectSerializer From(JObject obj, JsonSerializer serializer)
        {
            // this is our object type property
            var str = (string)obj["type"];

            // we map using a dictionary, but you can do whatever you want
            var type = TypesByName[str];

            // important to pass serializer (and its settings) along
            return obj.ToObject(type, serializer) as JObjectSerializer;
        }
        
        // convenience method for deserialization
        public static JObjectSerializer Deserialize(JsonReader reader)
        {
            var ser = new JsonSerializer();
            
            // important to add converter here
            ser.Converters.Add(new BaseClassConverter());

            return ser.Deserialize<JObjectSerializer>(reader);
        }
        
        internal static readonly Type[] Types = new []
        {
            typeof(BuildableBase)
        };

        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        public string JsonObjectType {get => this.GetType().Name.Split('.').Last(); set {}}
        internal static Dictionary<string, Type> TypesByName = Types.ToDictionary(t => t.Name, t => t);
    }
    
    
    
    */

/*    public static class BuildableExtensions
    {
        [JsonIgnore]
        internal static JsonSerializerSettings  SerializerSettings => new CustomJsonSerializerSettings();
        public static string Serialize(this IBuildable buildable) => SerializeJObject(buildable).ToString(Formatting.Indented);
        public static JObject SerializeJObject(this IBuildable buildable) => JObject.FromObject(buildable, JsonSerializer.CreateDefault(SerializerSettings));
    }*/
    
//    [JsonObject(MemberSerialization.OptOut, ItemNullValueHandling = NullValueHandling.Ignore,  NamingStrategyType = typeof(CustomNamingStrategy) )]
    /*

    
    public static class BuildableBase 
    {
        [Pure]
        public static JObject SerializeJObject(IBuildable o) =>  JObject.FromObject(o, Serializer.Default);

        [Pure]
        public static string Serialize(IBuildable o) => SerializeJObject(o).ToString(Formatting.Indented);
    }
*/


    /*public abstract class Buildable : BuildableBase, IBuildable
    {
        public JObject ToJObject() => SerializeJObject(this);

        /// <inheritdoc />
        public abstract JObject SerializeJObject();

        public override string ToString() => SerializeJObject().ToString(Formatting.Indented);
    }*/
}
