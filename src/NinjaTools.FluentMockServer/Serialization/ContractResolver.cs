using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace NinjaTools.FluentMockServer.Serialization
{
    public class ContractResolver : CamelCasePropertyNamesContractResolver
    {
        private static readonly Lazy<ContractResolver> Lazy = new Lazy<ContractResolver>(() => new ContractResolver());

        public static ContractResolver Instance => Lazy.Value;

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
        
        private ContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy
            {
                ProcessDictionaryKeys = false,
                OverrideSpecifiedNames = false
            };
        }
        
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
/*

            if (property.PropertyType == typeof())
            {
                
            }*/
            /*
            if (typeof().IsAssignableFrom(property.PropertyType) && property.Readable)
            {
                property.Readable = true;
                property.PropertyType = typeof(JToken);
            }
*/

            property.NullValueHandling = NullValueHandling.Ignore;
            return property;
        }
    }
}
