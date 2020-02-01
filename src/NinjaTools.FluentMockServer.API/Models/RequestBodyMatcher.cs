using System;
using System.Diagnostics;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class RequestBodyMatcher : IScoreable, IContentValidatable, IEquatable<RequestBodyMatcher>
    {
        public string DebuggerDisplay() => $"Kind={Kind?.ToString()}; HasContent={HasContent()}; MatchExact={IsExactMatch}; Content={Content}";
        public string? Content { get; set; }
        public RequestBodyKind?  Kind { get; set; }
        public bool? MatchExact { get; set; }
        public bool IsExactMatch => MatchExact != null && MatchExact == true;

        /// <inheritdoc />
        public bool HasContent() => !string.IsNullOrWhiteSpace(Content) || Kind.HasValue || MatchExact.HasValue;

        /// <inheritdoc cref="IScoreable" />
        [JsonIgnore]
        public int Score
        {
            get
            {

                if (!HasContent())
                {
                    return 0;
                }

                var score = 0;
                if (MatchExact.HasValue)
                {
                    score += 1;
                }

                if (!string.IsNullOrWhiteSpace(Content))
                {
                    score += 1;
                }

                if (Kind.HasValue)
                {
                    score += 1;
                }

                return score;
            }
        }

        #region IEquality

        /// <inheritdoc />
        public bool Equals(RequestBodyMatcher? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Content, other.Content, StringComparison.InvariantCultureIgnoreCase) && Kind == other.Kind && MatchExact == other.MatchExact;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RequestBodyMatcher) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Content, StringComparer.InvariantCultureIgnoreCase);
            hashCode.Add(Kind);
            hashCode.Add(MatchExact);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(RequestBodyMatcher? left, RequestBodyMatcher? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RequestBodyMatcher? left, RequestBodyMatcher? right)
        {
            return !Equals(left, right);
        }
        #endregion

    }
}
