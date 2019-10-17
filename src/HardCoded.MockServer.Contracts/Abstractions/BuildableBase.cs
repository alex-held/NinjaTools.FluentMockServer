using System.Diagnostics.Contracts;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HardCoded.MockServer.Contracts.Abstractions
{

    /// <inheritdoc cref="ISupportInversionMatch" />
    [JsonObject(MemberSerialization.OptOut, ItemNullValueHandling = NullValueHandling.Ignore,  NamingStrategyType = typeof(CustomNamingStrategy) )]
    public abstract class BuildableBase : IBuildable,  ISupportInversionMatch
    {
        [JsonIgnore]
        internal virtual JsonSerializerSettings  SerializerSettings { get; set; }

        [JsonIgnore] 
        internal bool _REVERSE_MATCHING_ENABLED { get; private set; }


        [Pure]
        public JObject SerializeJObject() => JObject.FromObject(this, JsonSerializer.CreateDefault(SerializerSettings));
        
       [Pure] 
        public string Serialize() => SerializeJObject().ToString(Formatting.Indented); 

        
        /// <inheritdoc />
        public InversionState PeekInversionState()
        {
            return _REVERSE_MATCHING_ENABLED switch
            {
                        false => InversionState.ToggledOff, true => InversionState.ToggledOn
            };
        }
        
        /// <inheritdoc />
        public void MatchSetup() => ToggleInversion(InversionState.ToggledOff);


        /// <inheritdoc />
        public void MatchAnythingButTheSetup() => ToggleInversion(InversionState.ToggledOn);


        /// <inheritdoc />
        public void ToggleInversion(InversionState inversion)
        {
            _REVERSE_MATCHING_ENABLED = inversion switch
            {
                        InversionState.ToggledOff => false, 
                        InversionState.ToggledOn => true
            };
        }

        
    }
    
    
}
