using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.API.Proxy.Visitors;
using NinjaTools.FluentMockServer.API.Serialization.Converters;
using NinjaTools.FluentMockServer.API.Types;
using YamlDotNet.Serialization;

namespace NinjaTools.FluentMockServer.API.Models
{
    [JsonConverter(typeof(MethodConverter))]
    public class Method : IVisitable, IContentValidatable, IScoreable, IEquatable<Method>
    {

        #region IEquality

        /// <inheritdoc />
        public bool Equals(Method? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(MethodString, other.MethodString, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Method) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode(MethodString);
        }

        public static bool operator ==(Method? left, Method? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Method? left, Method? right)
        {
            return !Equals(left, right);
        }

        #endregion IEquality

        public static implicit operator string(Method wrapper) => wrapper?.MethodString;

        /// <inheritdoc />
        [JsonIgnore]
        public int Score => HasContent() ? 1 : 0;

        /// <inheritdoc />
        public bool HasContent() => !string.IsNullOrWhiteSpace(MethodString);

        [UsedImplicitly]
        public Method()
        { }

        public Method(string methodString) => MethodString = methodString?.ToUpperInvariant();

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
