using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.API.Proxy.Visitors;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.Models
{
    public class QueryConverter : JsonConverter<Query>
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, Query value, JsonSerializer serializer)
        {
            var t = JToken.FromObject(value);
            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
        }

        /// <inheritdoc />
        public override Query ReadJson(JsonReader reader, Type objectType, Query existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize<string>(reader);
            return new Query(value);
        }
    }

    /// <inheritdoc cref="IQueryCollection" />
    [JsonConverter(typeof(QueryConverter))]
    public class Query :  IVisitable, IDontRenderWhenEmpty
    {
        public QueryString QueryString { get; }

        public Query() : this(QueryString.Empty)
        {
        }

        /// <inheritdoc />
        public Query(string value) : this(new QueryString($"?{value.TrimStart('?')}"))
        { }

        public Query(QueryString inner)
        {
            QueryString = inner;
        }

        /// <inheritdoc />
        public void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<Query> typed)
            {
                typed.Visit(this);
            }
        }

        /// <inheritdoc />
        public bool IsEnabled() => QueryString.HasValue;

        public string ToQuery() => QueryString.Value;
    }
}
