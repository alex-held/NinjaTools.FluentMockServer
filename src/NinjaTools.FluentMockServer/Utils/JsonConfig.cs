using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Serialization;

namespace NinjaTools.FluentMockServer.Utils
{
    /// <inheritdoc />
    public class JsonConfig : JsonSerializerSettings
    {
        /*public Action<PropertyRenameAndIgnoreSerializerContractResolver> ContractResolverSetup { get; set; } =
            resolver =>
            {
                resolver.SerializeCompilerGeneratedMembers = false;
                resolver.IngorePropertiesWithRegexForAssignableTypes<IBuildable>("_");
            };*/

        public static readonly JsonConfig Instance = new JsonConfig();

        public JsonConfig()
        {
            //       var resolver = new PropertyRenameAndIgnoreSerializerContractResolver();
            //     resolver.IgnorePropertiesWithRegex("HttpMethod");

            PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            var resolver = new IBuildableContractResolver();
            StringEscapeHandling = StringEscapeHandling.EscapeHtml;
            NullValueHandling = NullValueHandling.Ignore;
            Formatting = Formatting.Indented;
            ContractResolver = resolver;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            TypeNameHandling = TypeNameHandling.Objects;
        }

        /*
        Converters = new List<JsonConverter>
        {
            new ConcreteTypeConverter<IBuildable>(),
            new ConcreteTypeConverter<HttpResponse>(),
            new ConcreteTypeConverter<HttpError>(),
            new ConcreteTypeConverter<HttpForward>(),
            new ConcreteTypeConverter<HttpRequest>(),
            new ConcreteTypeConverter<HttpTemplate>(),
        */
        }
    }
    

