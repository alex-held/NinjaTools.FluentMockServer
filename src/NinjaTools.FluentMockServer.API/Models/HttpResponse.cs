using System;
using System.Diagnostics;

namespace NinjaTools.FluentMockServer.API.Models
{
    /// <inheritdoc />
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
    public class HttpResponse : IEquatable<HttpResponse>
    {
        public string DebuggerDisplay() => $"Status={StatusCode}; Body={Body}";
        public int? StatusCode { get; set; }
        public string? Body { get; set; }

        #region Equality members

        /// <inheritdoc />
        public bool Equals(HttpResponse? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return StatusCode == other.StatusCode && Body == other.Body;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HttpResponse) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(StatusCode, Body);
        }

        public static bool operator ==(HttpResponse? left, HttpResponse? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(HttpResponse? left, HttpResponse? right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
