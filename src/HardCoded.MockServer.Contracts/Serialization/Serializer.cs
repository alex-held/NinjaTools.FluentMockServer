using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Contracts.Attributes;

using JetBrains.Annotations;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

using Formatting = Newtonsoft.Json.Formatting;


namespace HardCoded.MockServer.Contracts.Serialization
{
    /// <inheritdoc />
    public class CustomNamingStrategy : CamelCaseNamingStrategy
    {
        /// <inheritdoc />
        public CustomNamingStrategy()
        {
            ProcessDictionaryKeys = true;
            OverrideSpecifiedNames = true;
        }   
    }
    
     /// <summary>
    /// Use this tool to reliable use the same encoding and decoding technique.
    /// </summary>
    internal static class EncodingUtils<T>
    {

        /// <summary>
        /// Use this tool to reliable use the same encoding and decoding technique.
        /// </summary>
        /// <param name="body">An instance of an object to serialize.</param>
        /// <param name="contentType">The content-type will determinate how to serialize the content.</param>
        /// <param name="encoding">Here you may like to specify a different encoding to use.</param>
        /// <returns>A byte array containing payload.</returns>
        /// <exception cref="ArgumentNullException">The <param name="body"/> might have been null. </exception>
        /// <exception cref="ArgumentNullException">The <param name="contentType"/> might have been null. </exception>
        /// <exception cref="NotSupportedException">The <param name="contentType"/> is currently not supported.</exception>
         [System.Diagnostics.Contracts.Pure]
        public static byte[] Encode([NotNull] T body, [NotNull] string contentType, [NotNull]  Encoding encoding)
        {
            if (body is null) 
                throw new ArgumentNullException(nameof(body));
            if (encoding is null) 
                throw new ArgumentNullException(nameof(encoding));

            try {
                if (!CommonContentType.TryParseContentType(contentType, out var commonContentType))
                    throw new NotSupportedException($"The content type: {contentType} is currently not supported.");

                if (commonContentType.Id == CommonContentType.Json.Id) return EncodeJson(body, encoding);
                if (commonContentType.Id == CommonContentType.Xml.Id) return EncodeXml(body, encoding);
            } catch (Exception e) {
                Console.WriteLine(e);

                throw;
            }

            return new byte[0];
        }


        [System.Diagnostics.Contracts.Pure]
        
        private static byte[]? EncodeJson([NotNull]T body, [NotNull] Encoding encoding)
        {
            try {
                var json = JsonConvert.SerializeObject(body, Formatting.Indented);

                var bytes =  encoding.GetBytes(json);

                return bytes;
            } catch (Exception exception) {
                Console.WriteLine(exception);
                throw;
            }
        }

        
        [System.Diagnostics.Contracts.Pure]
        
        private static byte[]? EncodeXml([NotNull] T body, [NotNull] Encoding encoding)
        {
            try {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using var stringWriter = new StringWriter();
                using var xmlWriter = XmlWriter.Create(stringWriter);
                xmlSerializer.Serialize(xmlWriter, body);
                var xml = stringWriter.ToString();

                var bytes =  encoding.GetBytes(xml);
                return bytes;
            } catch (Exception e) {
                Console.WriteLine(e);

                throw;
            }
        }
    }
     
     public class PropertyRenameAndIgnoreSerializerContractResolver : CamelCasePropertyNamesContractResolver
    {
        private readonly Dictionary<Type, HashSet<string>> _ignores;
        private readonly Dictionary<Type, Dictionary<string, string>> _renames;
       
        public PropertyRenameAndIgnoreSerializerContractResolver()
        {
            _ignores = new Dictionary<Type, HashSet<string>>();
            _renames = new Dictionary<Type, Dictionary<string, string>>();
            
        }
        
        internal void IgnorePropertiesWithRegex([NotNull] string pattern, [NotNull] params Type[] types)
        {
            var regex = new Regex(pattern);

            types.ToList()
                        .ForEach(
                            type => {
                                if (!_ignores.ContainsKey(type)) _ignores[type] = new HashSet<string>();

                                type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            .Where(pi => !regex.IsMatch(pi.Name))
                                            .Select(pi => pi.Name)
                                            .ToList()
                                            .ForEach(property => _ignores[type].Add(property));
                            });
        }

        public virtual Func<Type, Type, Action<Type>, bool> TypeFilter { get; set; } = (scannedType, banningType, registryCallback) => {
            var matches =
                        scannedType.IsClass           && // I need classes
                        !scannedType.IsEnum           && // I need classes
                        !scannedType.IsAbstract       && // Must be able to instantiate the class
                        !scannedType.IsNestedPrivate  && // Nested private types are not accessible
                        scannedType.Namespace != null && // Yes, it can be null!
                        !scannedType.Namespace.StartsWith("System.", StringComparison.Ordinal)
                     && // EF, for instance, is not in the GAC
                        !scannedType.Namespace.StartsWith("DevExpress.", StringComparison.Ordinal)
                     && // Exclude third party lib
                        !scannedType.Namespace.StartsWith("CySoft.Wff", StringComparison.Ordinal)
                     && // Exclude my own lib
                        !scannedType.Namespace.EndsWith(".Migrations", StringComparison.Ordinal)
                     && // Exclude EF migrations stuff
                        !scannedType.Namespace.EndsWith(".My", StringComparison.Ordinal)
                     && // Excludes types from VB My.something
                        scannedType.GetCustomAttribute<CompilerGeneratedAttribute>() == null; // Excl. compiler gen
            
             // we need to see if this works   -->  !type.Assembly.GlobalAssemblyCache     
             // Excludes most of BCL and third-party classes
             
             if (!matches) return false;
            
            // << IsSubclassOf >> will not work when banning type is an interface
            if (scannedType.IsSubclassOf(banningType) || banningType.IsAssignableFrom(scannedType)) 
                registryCallback(scannedType);

            return true;
        };
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <typeparam name="TInterfaceOrBaseClass"></typeparam>
        public void IngorePropertiesWithRegexForAssignableTypes<TInterfaceOrBaseClass>( [NotNull]  string pattern)
        {
            var typesToIgnorePropertiesFrom = new HashSet<Type>();

            AppDomain.CurrentDomain
                        .GetAssemblies()
                        .Where(assembly => assembly.GetCustomAttributes<WhiteListAssemblyReflectionAttribute>().Any())
                        .SelectMany(assembly => assembly.GetTypes())
                        .Where(
                            type => TypeFilter( type, typeof(TInterfaceOrBaseClass),
                                t => typesToIgnorePropertiesFrom.Add(t)));
            
            IgnorePropertiesWithRegex(pattern, typesToIgnorePropertiesFrom.ToArray());
        }

        public void IgnoreProperty(Type type, params string[] jsonPropertyNames)
        {
            if (!_ignores.ContainsKey(type)) _ignores[type] = new HashSet<string>();

            foreach ( var prop in jsonPropertyNames ) _ignores[type].Add(prop);
        }
        
        public void RenameProperty(Type type, string propertyName, string newJsonPropertyName)
        {
            if (!_renames.ContainsKey(type)) _renames[type] = new Dictionary<string, string>();

            _renames[type][propertyName] = newJsonPropertyName;
        }
        
        public void RenameProperty<T>(string propertyName, string newJsonPropertyName)
        {
            var type = typeof(T);
            if (!_renames.ContainsKey(type)) _renames[type] = new Dictionary<string, string>();
            
            _renames[type][propertyName] = newJsonPropertyName;
        }


        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (IsIgnored(property.DeclaringType, property.PropertyName)) {
                property.ShouldSerialize = i => false;
                property.Ignored = true;
            }

            if (IsRenamed(property.DeclaringType, property.PropertyName, out var newJsonPropertyName))
                property.PropertyName = newJsonPropertyName;

            return property;
        }


        private bool IsIgnored(Type type, string jsonPropertyName)
        {
            if (!_ignores.ContainsKey(type)) return false;

            return _ignores[type].Contains(jsonPropertyName);
        }


        private bool IsRenamed(Type type, string jsonPropertyName, out string newJsonPropertyName)
        {
            Dictionary<string, string> renames;

            if (!_renames.TryGetValue(type, out renames)
             || !renames.TryGetValue(jsonPropertyName, out newJsonPropertyName)) {
                newJsonPropertyName = null;

                return false;
            }

            return true;
        }


        /// <inheritdoc />
        protected override string ResolveDictionaryKey(string dictionaryKey) => dictionaryKey;
    }

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

    /// <inheritdoc />
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
