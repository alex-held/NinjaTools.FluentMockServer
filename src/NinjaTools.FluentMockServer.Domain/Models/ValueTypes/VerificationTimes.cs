using System;
using Newtonsoft.Json;

namespace NinjaTools.FluentMockServer.Domain.Models.ValueTypes
{
    [Serializable]
    public class VerificationTimes : IIdentifiable<VerificationTimes>
    {
        /// <inheritdoc />
        public int Id { get; }

        /// <inheritdoc />
        public DateTime CreatedOn { get; set; }

        /// <inheritdoc />
        public DateTime ModifiedOn{ get; set; }

        /// <inheritdoc />
        public byte[] Timestamp{ get; set; }
        
        public VerificationTimes()
        {
        
        }
        
        /// <inheritdoc />
        [JsonConstructor]
        public VerificationTimes(int? atLeast, int? atMost)
        {
            AtLeast = atLeast;
            AtMost = atMost;
        }

        public static VerificationTimes Once => new VerificationTimes(1, 1);
        public static VerificationTimes Twice => new VerificationTimes(2, 2);

        public int? AtLeast { get; }
        public int? AtMost { get; }

        public static VerificationTimes Between(int atLeast, int atMost)
        {
            return new VerificationTimes(atLeast, atMost);
        }

        public static VerificationTimes MoreThan(int times)
        {
            return new VerificationTimes(times, null);
        }

        public static VerificationTimes LessThan(int times)
        {
            return new VerificationTimes(0, times);
        }

        /// <inheritdoc />
        public bool Equals(VerificationTimes other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return AtLeast == other.AtLeast && AtMost == other.AtMost;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VerificationTimes) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (AtLeast.GetHashCode() * 397) ^ AtMost.GetHashCode();
            }
        }
    }
}