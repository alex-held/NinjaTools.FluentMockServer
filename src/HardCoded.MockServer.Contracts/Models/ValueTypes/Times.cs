using System;

using HardCoded.MockServer.Contracts.Abstractions;

using Newtonsoft.Json;


namespace HardCoded.MockServer.Contracts.Models.ValueTypes
{
    public class Times : BuildableBase, IEquatable<Times>
    {
        private Times(long remainingTimes)
        {
            if (remainingTimes < 0)
                Unlimited = true;
            else
                RemainingTimes = remainingTimes;
        }
        
        public static Times Once => new Times(1);
        public static Times Always => new Times(-1);
        
        public long RemainingTimes { get; set; }

        public bool Unlimited { get; set; }
        
        #region Equality Members

        /// <inheritdoc />
        public bool Equals(Times other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return RemainingTimes == other.RemainingTimes && Unlimited == other.Unlimited;
        }


        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals(( Times ) obj);
        }


        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked {
                return ( RemainingTimes.GetHashCode() * 397 ) ^ Unlimited.GetHashCode();
            }
        }


        public static bool operator ==(Times left, Times right) => Equals(left, right);


        public static bool operator !=(Times left, Times right) => !Equals(left, right);

        #endregion
    }
}
