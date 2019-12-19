using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.Models
{
    /// <summary>
    ///     Model to set up an Expectation on the MockServer.
    /// </summary>
    public partial class Expectation : IIdentifiable<Expectation>
    {
        /// <inheritdoc />
        public int Id { get; set; }
        
        /// <inheritdoc />
        public DateTime CreatedOn { get;  set; } = DateTime.UtcNow;

        /// <inheritdoc />
        public DateTime ModifiedOn{ get;  set; }

        /// <inheritdoc />
        [Timestamp]
        public byte[] Timestamp { get; set; }
        
        
        /// <inheritdoc />
        public bool Equals(Expectation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && CreatedOn.Equals(other.CreatedOn) && ModifiedOn.Equals(other.ModifiedOn) && Equals(Timestamp, other.Timestamp) && Equals(HttpRequest, other.HttpRequest) && Equals(HttpResponse, other.HttpResponse) && Equals(HttpResponseTemplate, other.HttpResponseTemplate) && Equals(HttpForward, other.HttpForward) && Equals(HttpForwardTemplate, other.HttpForwardTemplate) && Equals(HttpError, other.HttpError) && Equals(Times, other.Times) && Equals(TimeToLive, other.TimeToLive);
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
            var hashCode = new HashCode();
            hashCode.Add(HttpRequest);
            hashCode.Add(HttpResponse);
            hashCode.Add(HttpResponseTemplate);
            hashCode.Add(HttpForward);
            hashCode.Add(HttpForwardTemplate);
            hashCode.Add(HttpError);
            hashCode.Add(Times);
            hashCode.Add(TimeToLive);
            hashCode.Add(Id);
            hashCode.Add(CreatedOn);
            hashCode.Add(ModifiedOn);
            hashCode.Add(Timestamp);
            return hashCode.ToHashCode();
        }

        public static bool operator ==([CanBeNull] Expectation left, [CanBeNull] Expectation right)
        {
            return Equals(left, right);
        }

        public static bool operator !=([CanBeNull] Expectation left, [CanBeNull] Expectation right)
        {
            return !Equals(left, right);
        }
    }
}
