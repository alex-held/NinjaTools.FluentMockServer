using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.API.Extensions;
using NinjaTools.FluentMockServer.API.Types;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace NinjaTools.FluentMockServer.API.Models
{
    /// <inheritdoc cref="IEquatable{T}" />
    [JsonObject(MemberSerialization.OptOut, ItemNullValueHandling = NullValueHandling.Ignore)]
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class RequestMatcher : IEquatable<RequestMatcher>, IScoreable, IContentValidatable
    {
        [YamlMember(SerializeAs = typeof(Dictionary<string, object>))]
        public RequestBodyMatcher? BodyMatcher { get; set; }

        [YamlMember(SerializeAs = typeof(string), ScalarStyle = ScalarStyle.Any)]
        public Path? Path { get; set; }


        [YamlMember(SerializeAs = typeof(string), ScalarStyle = ScalarStyle.Any)]
        public Method? Method { get; set; }


        [YamlMember(SerializeAs = typeof(Dictionary<string, string[]>))]
        public Headers? Headers { get; set; }


        [YamlMember(SerializeAs = typeof(Dictionary<string, string>))]
        public Cookies? Cookies { get; set; }


        [YamlMember(SerializeAs = typeof(Dictionary<string, string[]>))]
        public Query? Query { get; set; }


        #region IScoreable
        private IList<object> Members() => new List<object>
        {
            Path,
            Query,
            Method,
            BodyMatcher,
            Cookies,
            Headers
        };

        /// <inheritdoc cref="IContentValidatable" />
        public bool HasContent() => Members()
            .OfType<IContentValidatable>()
            .All(cv => cv?.HasContent() ?? false);

        /// <inheritdoc />
        [JsonIgnore]
        public int Score => Members().OfType<IScoreable>().SkipNullOrDefault().Sum(s => s?.Score ?? 0);

        #endregion

        public string DebuggerDisplay() => $"Method={Method?.MethodString}; Path={Path?.ToPath()}; Query={Query?.ToQuery()}; Headers={Headers?.Count}; HasBody={HasContent()};";


        public bool TryGetBodyString(out string body)
        {
            var hasBody = BodyMatcher != null && BodyMatcher.HasContent();
            body = hasBody
                ? BodyMatcher.Content
                : null;
            return hasBody;
        }

        public bool TryGetHeaders(out Headers headers)
        {
            var hasHeaders = Headers != null && Headers.Any();
            headers = hasHeaders
                ? Headers
                : null;
            return hasHeaders;
        }


        #region Equality members

        /// <inheritdoc />
        public bool Equals(RequestMatcher? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return BodyMatcher.Equals(other.BodyMatcher) && Path.Equals(other.Path) && Method.Equals(other.Method) && Headers.Equals(other.Headers) && Cookies.Equals(other.Cookies) && Query.Equals(other.Query);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RequestMatcher) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(BodyMatcher, Path, Method, Headers, Cookies, Query);
        }

        public static bool operator ==(RequestMatcher? left, RequestMatcher? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RequestMatcher? left, RequestMatcher? right)
        {
            return !Equals(left, right);
        }

        #endregion



    }



}
