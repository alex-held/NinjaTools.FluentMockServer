using System;

using NinjaTools.FluentMockServer.Abstractions;


namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    public class TimeToLive : BuildableBase, IEquatable<TimeToLive>
    {
        public int? Time { get; set; }
        
        public TimeUnit? TimeUnit { get; set; }
        
        public bool? Unlimited { get; set; }
        
        #region Equality Members

        /// <inheritdoc />
        public bool Equals(TimeToLive other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Time == other.Time && TimeUnit == other.TimeUnit && Unlimited == other.Unlimited;
        }


        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals(( TimeToLive ) obj);
        }


        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked {
                var hashCode = Time.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ TimeUnit.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ Unlimited.GetHashCode();

                return hashCode;
            }
        }


        public static bool operator ==(TimeToLive left, TimeToLive right) => Equals(left, right);


        public static bool operator !=(TimeToLive left, TimeToLive right) => !Equals(left, right);

        #endregion
    }
}
