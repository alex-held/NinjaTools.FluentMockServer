using System;
using System.ComponentModel.DataAnnotations;

namespace NinjaTools.FluentMockServer.Domain.Models
{
    /// <summary>
    ///     Model to set up an Expectation on the MockServer.
    /// </summary>
    public partial class Expectation : IIdentifiable<Expectation>
    {
        /// <inheritdoc />
        public int Id { get; set; }
        
        /// <inheritdoc />
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <inheritdoc />
        public DateTime ModifiedOn{ get; set; }

        /// <inheritdoc />
        [Timestamp]
        public byte[] Timestamp { get; set; }
        
        
        /// <inheritdoc />
        public bool Equals(Expectation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(HttpRequest, other.HttpRequest) && Equals(HttpResponse, other.HttpResponse) && Equals(HttpResponseTemplate, other.HttpResponseTemplate) && Equals(HttpForward, other.HttpForward) && Equals(HttpForwardTemplate, other.HttpForwardTemplate) && Equals(HttpError, other.HttpError) && Equals(Times, other.Times) && Equals(TimeToLive, other.TimeToLive);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Expectation) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (HttpRequest != null ? HttpRequest.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (HttpResponse != null ? HttpResponse.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (HttpResponseTemplate != null ? HttpResponseTemplate.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (HttpForward != null ? HttpForward.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (HttpForwardTemplate != null ? HttpForwardTemplate.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (HttpError != null ? HttpError.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Times != null ? Times.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TimeToLive != null ? TimeToLive.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
