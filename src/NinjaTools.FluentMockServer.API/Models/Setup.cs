using System;
using System.Diagnostics;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Setup : IEquatable<Setup>, IScoreable, IContentValidatable
    {
        [JsonIgnore]
        public string Id { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public int Score => Matcher?.Score ?? 0;

        /// <inheritdoc />
        public bool HasContent() => Matcher?.HasContent() ?? false;

        public string DebuggerDisplay() => $"Score={Score}; Matcher={Matcher}; Action={Action?.GetType().Name ?? "<null>"}";

        public Setup()
        {
        }

        [JsonConstructor]
        public Setup(RequestMatcher matcher, ResponseAction? action = null)
        {
            Matcher = matcher;
            Action = action;

        }

        public RequestMatcher Matcher { get; set; }

        public ResponseAction? Action { get; set; }



        #region Equality members

        /// <inheritdoc />
        public bool Equals(Setup? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Matcher.Equals(other.Matcher) && Equals(Action, other.Action);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Setup) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(Matcher, Action);
        }

        public static bool operator ==(Setup? left, Setup? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Setup? left, Setup? right)
        {
            return !Equals(left, right);
        }

        #endregion


    }
}
