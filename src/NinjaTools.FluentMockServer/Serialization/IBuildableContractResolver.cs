using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Utils;

namespace NinjaTools.FluentMockServer.Serialization
{
    public class IBuildableContractResolver : CamelCasePropertyNamesContractResolver
    {
        /// <summary>
        /// Property names and dictionary keys will be UPPERCASE.
        /// </summary>
        public class UpperCaseNamingStrategy : NamingStrategy
        {
            /// <summary>
            /// Resolves the specified property name.
            /// </summary>
            /// <param name="name">The property name to resolve.</param>
            /// <returns>The resolved property name.</returns>
            protected override string ResolvePropertyName(string name)
            {
                return name.ToUpper();
            }

            public UpperCaseNamingStrategy()
            {
                this.OverrideSpecifiedNames = true;
            }
        }
        
        /// <summary>
        /// Converts the enum values to an UPPERCASE string.
        /// <seealso cref="UpperCaseNamingStrategy"/>
        /// </summary>
        public class UpperCaseEnumConverter : StringEnumConverter
        {
            /// <inheritdoc />
            public UpperCaseEnumConverter()
            {
                NamingStrategy = new UpperCaseNamingStrategy();
            }
        }

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
        
        
        public IBuildableContractResolver()
        {
            NamingStrategy = new CustomNamingStrategy();
        }

        /// <inheritdoc />
        protected override JsonContract CreateContract(Type objectType)
        {
            var contact = base.CreateContract(objectType);
            contact.IsReference = false;
            //       contact.Converter = ResolveContractConverter(typeof(BuildableBaseConverter));
            return contact;
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
                if (target is IBuildable buildableBase)
                {
                    return buildableBase.SerializeJObject();
                }

                var fallBackValue = _fallbackValueProvider.GetValue(target);
                return fallBackValue;
            }
        }


        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (typeof(IBuildable).IsAssignableFrom(property.PropertyType) && property.Readable)
            {
                property.Readable = true;
                property.ValueProvider = new BuildableBaseValueProvider(property.ValueProvider, member as PropertyInfo);
                property.PropertyType = typeof(JToken);
            }

            property.NullValueHandling = NullValueHandling.Ignore;
            return property;
        }
    }
}
