using Microsoft.AspNetCore.Http;

namespace NinjaTools.FluentMockServer.API.Proxy.Visitors.Collections
{
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
}