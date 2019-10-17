using System;
using System.Diagnostics.CodeAnalysis;

using HardCoded.MockServer.Contracts.Abstractions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace HardCoded.MockServer.Contracts.Serialization
{
    public class Serializer : JsonSerializer
    {
        public static Serializer Default => new Serializer(new CustomJsonSerializerSettings());
        
        public CustomJsonSerializerSettings CustomJsonSerializerSettings { get; }
        
        public Serializer(CustomJsonSerializerSettings customJsonSerializerSettings) =>
                    CustomJsonSerializerSettings = customJsonSerializerSettings;
        
        public static JObject Serialize([DisallowNull] object o)  => JObject.FromObject(o, Default);
        public static T Deserialize<T>([DisallowNull] string json) => JObject.Parse(json).ToObject<T>(Default);
    }
    
    public class CustomJsonSerializerSettings : JsonSerializerSettings
    {
        public Action<PropertyRenameAndIgnoreSerializerContractResolver> ContractResolverSetup { get; set; } =
            resolver => {
                resolver.SerializeCompilerGeneratedMembers = false;
                resolver.IngorePropertiesWithRegexForAssignableTypes<IBuildable>("_"); };

        public CustomJsonSerializerSettings()
        {
            StringEscapeHandling = StringEscapeHandling.EscapeNonAscii; 
            NullValueHandling = NullValueHandling.Ignore;
            Formatting = Formatting.Indented;
            ContractResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
        }
    }
}
