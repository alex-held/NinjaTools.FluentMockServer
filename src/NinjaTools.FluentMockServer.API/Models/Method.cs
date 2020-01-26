using JetBrains.Annotations;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.API.Proxy.Visitors;
using NinjaTools.FluentMockServer.API.Serialization.Converters;
using NinjaTools.FluentMockServer.API.Types;
using YamlDotNet.Serialization;

namespace NinjaTools.FluentMockServer.API.Models
{
    [JsonConverter(typeof(MethodConverter))]
    public class Method : IVisitable, IDontRenderWhenEmpty
    {
        public static implicit operator string(Method wrapper) => wrapper.MethodString;

        /// <inheritdoc />
        public bool IsEnabled() => !string.IsNullOrWhiteSpace(MethodString);

        [UsedImplicitly]
        public Method()
        { }

        public Method(string methodString)
        {
            MethodString = methodString;
        }




        [YamlMember(SerializeAs = typeof(string), Alias = "Method")]
        public string MethodString { get; set; }

        /// <inheritdoc />
        public void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<Method> typed)
            {
                typed.Visit(this);
            }
        }
    }
}
