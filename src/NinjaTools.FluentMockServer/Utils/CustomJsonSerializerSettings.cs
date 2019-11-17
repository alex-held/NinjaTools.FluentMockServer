using System;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.Abstractions;

namespace NinjaTools.FluentMockServer.Utils
{
    /// <inheritdoc />
    public class CustomJsonSerializerSettings : JsonSerializerSettings
    {
        public Action<PropertyRenameAndIgnoreSerializerContractResolver> ContractResolverSetup { get; set; } =
            resolver => {
                resolver.SerializeCompilerGeneratedMembers = false;
                resolver.IngorePropertiesWithRegexForAssignableTypes<IBuildable>("_"); };

        public CustomJsonSerializerSettings()
        {
            var resolver = new PropertyRenameAndIgnoreSerializerContractResolver();
            resolver.IgnorePropertiesWithRegex("HttpMethod");
            StringEscapeHandling = StringEscapeHandling.EscapeHtml; 
            NullValueHandling = NullValueHandling.Ignore;
            Formatting = Formatting.Indented;
            ContractResolver = resolver;
        }
    }
}