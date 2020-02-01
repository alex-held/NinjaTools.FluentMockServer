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
    public class Query : IVisitable, IScoreable, IContentValidatable, IEquatable<Query>
    {

        #region IEquality

        /// <inheritdoc />
        public bool Equals(Query? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return QueryString.Equals(other.QueryString);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Query) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return QueryString.GetHashCode();
        }

        public static bool operator ==(Query? left, Query? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Query? left, Query? right)
        {
            return !Equals(left, right);
        }

        #endregion

        /// <inheritdoc />
        public bool HasContent() => QueryString.HasValue;

        /// <inheritdoc />
        [JsonIgnore]
        public int Score => HasContent() ? 1 : 0;

        public QueryString QueryString { get; }

        public Query(QueryString inner) =>  QueryString = inner;

        /// <inheritdoc />
        public Query() : this(QueryString.Empty) { }

        /// <inheritdoc />
        public Query(string value) : this(new QueryString($"?{value.TrimStart('?')}")) { }


        /// <inheritdoc />
        public void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<Query> typed)
            {
                typed.Visit(this);
            }
        }

        public string ToQuery() => QueryString.Value;
    }
}
