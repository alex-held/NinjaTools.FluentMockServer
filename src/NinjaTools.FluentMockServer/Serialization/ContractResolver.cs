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
        private class UpperCaseNamingStrategy : NamingStrategy
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
        internal class UpperCaseEnumConverter : StringEnumConverter
        {
            /// <inheritdoc />
            public UpperCaseEnumConverter()
            {
                NamingStrategy = new UpperCaseNamingStrategy();
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

            property.NullValueHandling = NullValueHandling.Ignore;
            return property;
        }
    }
}
