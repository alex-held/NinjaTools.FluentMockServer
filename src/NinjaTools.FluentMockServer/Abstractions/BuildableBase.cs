using System.Diagnostics.Contracts;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NinjaTools.FluentMockServer.Utils;


namespace NinjaTools.FluentMockServer.Abstractions
{

    [JsonObject(MemberSerialization.OptOut, ItemNullValueHandling = NullValueHandling.Ignore,  NamingStrategyType = typeof(CustomNamingStrategy) )]
    public abstract class BuildableBase : IBuildable
    {
        [JsonIgnore]
        internal virtual JsonSerializerSettings  SerializerSettings => new CustomJsonSerializerSettings();

        [JsonIgnore] 
        internal bool _REVERSE_MATCHING_ENABLED { get; private set; }
        
        [Pure]
        public virtual JObject SerializeJObject() => JObject.FromObject(this, JsonSerializer.CreateDefault(SerializerSettings));
        
       [Pure] 
        public virtual string Serialize() => SerializeJObject().ToString(Formatting.Indented);
    }
    
    
}
