using Microsoft.AspNetCore.Http;

namespace NinjaTools.FluentMockServer.API.Proxy.Visitors.Collections
{
    public class PathCollection : IVisitable
    {
        public static implicit operator string(PathCollection? query) => query?.Path;
        public static implicit operator PathCollection(string? str) => new PathCollection {PathString = $"/{str?.TrimStart('/')}"};

        /// <inheritdoc />
        public PathString PathString { get; set; }
        public string Path => PathString.Value;

        /// <inheritdoc />
        public void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<PathCollection> typed)
            {
                typed.Visit(this);
            }
        }
    }
}