/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Annotations;

namespace NinjaTools.FluentMockServer.Utils
{

    public class BuildableBaseConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                return;
            }
            
            var jo = new JObject();
            var type = value?.GetType();

            foreach (var pi in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (pi.CanRead)
                {
                    var propValue = pi.GetValue(value, null);
                    
                    if (typeof().IsAssignableFrom(pi.PropertyType))
                    {
                        var propJObject = (() propValue).SerializeJObject();
                        jo.Add(pi.Name, JToken.FromObject(propJObject, serializer));
                    }
                    
                    jo.Add(pi.Name, JToken.FromObject(propValue, serializer));
                    
                }
            }
        }

        /// <inheritdoc />
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return typeof().IsAssignableFrom(objectType);
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

        
        sealed class BuildableBaseValueProvider : IValueProvider
        {

            private PropertyInfo _propertyInfo;
            private IValueProvider _fallbackValueProvider;

            public BuildableBaseValueProvider(IValueProvider fallbackValueProvider, PropertyInfo propertyInfo)
            {
                _propertyInfo = propertyInfo;
                _fallbackValueProvider = fallbackValueProvider;
            }

            /// <inheritdoc />
            public void SetValue(object target, object? value)
            {
                _fallbackValueProvider.SetValue(target, value);
            }

            /// <inheritdoc />
            public object? GetValue(object target)
            {
                if (target is  buildableBase)
                {
                    return buildableBase.SerializeJObject();
                }

                var fallBackValue = _fallbackValueProvider.GetValue(target);
                return fallBackValue;
            }
        }


        //// <inheritdoc />
/*        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var contract = base.CreateObjectContract(objectType);
            //    contract.Converter


            // override BuildableBase Converts 
            if (typeof(BuildableBase).IsAssignableFrom(objectType))
            {
                var propertyHashSet = new HashSet<JsonProperty>();
                
                foreach (var contractProperty in contract.Properties)
                {
                    if (!typeof(BuildableBase).IsAssignableFrom(contractProperty.PropertyType))
                    {
                       continue;
                    }
                    
                    propertyHashSet.Add(contractProperty);
                    contractProperty
                }

            }
        }#1#

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

            if (property.PropertyType.IsSubclassOf(typeof()) && property.Readable)
            {
                property.Readable = true;
                property.ValueProvider = new BuildableBaseValueProvider(property.ValueProvider, member as PropertyInfo); ;
                property.PropertyType = typeof(JToken);
                property.ShouldSerialize = value => true;
            }
            
            
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
}
*/
