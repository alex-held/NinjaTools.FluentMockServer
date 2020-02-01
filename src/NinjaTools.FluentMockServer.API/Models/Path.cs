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
    public class Path : IVisitable, IEquatable<PathString>, IDontRenderWhenEmpty
    {
        public static implicit operator string(Path path) => path.PathString.Value;

        /// <inheritdoc />
        public Path([CanBeNull] string path) : this(new PathString($"/{path?.TrimStart('/')}"))
        { }

        public Path(PathString path)
        {
            PathString = path;
        }


        /// <inheritdoc />
        public void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<Path> typed)
            {
                typed.Visit(this);
            }
        }

        /// <inheritdoc cref="PathString" />
        public PathString PathString { get; set; }

        public string ToPath() => PathString.Value;

        /// <inheritdoc />
        public bool Equals(PathString other) => PathString.Equals(other);
        /// <inheritdoc />
        public bool IsEnabled() => PathString.HasValue;
    }
}
