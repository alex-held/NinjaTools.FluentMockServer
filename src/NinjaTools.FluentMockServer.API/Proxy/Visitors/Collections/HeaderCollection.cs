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
}
