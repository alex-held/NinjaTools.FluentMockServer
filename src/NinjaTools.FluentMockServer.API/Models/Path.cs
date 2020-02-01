using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.API.Proxy.Visitors;
using NinjaTools.FluentMockServer.API.Serialization.Converters;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.Models
{
    /// <inheritdoc cref="PathString" />
    [JsonConverter(typeof(PathConverter))]
    public class Path : IVisitable, IScoreable, IContentValidatable, IEquatable<Path>
    {

        /// <inheritdoc />
        public bool HasContent() => PathString.HasValue;

        /// <inheritdoc />
        [JsonIgnore]
        public int Score => HasContent() ? 1 : 0;

        public static implicit operator string(Path path) => path.PathString.Value;

        /// <inheritdoc />
        public Path([CanBeNull] string path) : this(new PathString($"/{path?.TrimStart('/')}"))
        { }

        public Path(PathString path) => PathString = path;

        /// <inheritdoc />
        public void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<Path> typed)
            {
                typed.Visit(this);
            }
        }

        /// <inheritdoc cref="PathString" />
        [JsonIgnore]
        public PathString PathString { get; set; }

        public string ToPath() => PathString.Value;

        #region IEquality

        /// <inheritdoc />
        public bool Equals(Path? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return PathString.Equals(other.PathString);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Path) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return PathString.GetHashCode();
        }

        public static bool operator ==(Path? left, Path? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Path? left, Path? right)
        {
            return !Equals(left, right);
        }


        #endregion

    }
}
