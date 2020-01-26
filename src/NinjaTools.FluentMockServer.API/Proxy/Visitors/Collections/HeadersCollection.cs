using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace NinjaTools.FluentMockServer.API.Proxy.Visitors.Collections
{

    public class HeaderCollection : HeaderDictionary, IVisitable
    {
        public HeaderCollection() : this(new Dictionary<string, string[]>())
        { }

        public HeaderCollection(IDictionary<string, string[]> dict)
        {
            dict ??= new Dictionary<string, string[]>();
            foreach (var (key, value) in dict)
            {
                Add(key, value);
            }
        }

        public static implicit operator HeaderCollection(Dictionary<string, string[]> headers) => From(headers);
        public static implicit operator Dictionary<string, string[]>(HeaderCollection headers) =>
            headers.ToDictionary(k => k.Key,
                v => v.Value.ToArray());

        public static HeaderCollection From(IDictionary<string, string[]> headers)
        {
            var headerCollection = new HeaderCollection();
            headers ??= new Dictionary<string, string[]>();
            foreach (var header in headers)
            {
                headerCollection.Add(header.Key, header.Value);
            }

            return headerCollection;
        }

        /// <inheritdoc />
        public void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<HeaderCollection> typed)
            {
                typed.Visit(this);
            }
        }
    }

    public class CookieCollection : Dictionary<string, string>,  IVisitable
    {
        /// <inheritdoc />
        public void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<CookieCollection> typed)
            {
                typed.Visit(this);
            }
        }
    }

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

    public class QueryCollection : IVisitable
    {
        public static implicit operator string(QueryCollection query) => query.Query;
        public static implicit operator QueryCollection(QueryString query) => new QueryCollection {QuryString = query};
        public static implicit operator QueryCollection(string str) => new QueryCollection {QuryString = new QueryString($"?{str.TrimStart('?')}")};

        /// <inheritdoc />
        public QueryString QuryString { get; set; }
        public string Query => QuryString.Value;

        /// <inheritdoc />
        public void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<QueryCollection> typed)
            {
                typed.Visit(this);
            }
        }
    }

    public class HttpMethodWrapper : IVisitable
    {
        public HttpMethodWrapper(string methodString)
        {
            MethodString = methodString;
        }

        public HttpMethodWrapper()
        { }

        public string MethodString { get; set; }
        public static implicit operator string(HttpMethodWrapper wrapper) => wrapper.MethodString;
        public static implicit operator HttpMethodWrapper(string str) => new HttpMethodWrapper {MethodString = str};

        /// <inheritdoc />
        public void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<HttpMethodWrapper> typed)
            {
               typed.Visit(this);
            }
        }
    }
}
