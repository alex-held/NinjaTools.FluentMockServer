using System;

using NinjaTools.FluentMockServer.Abstractions;


namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    public class LifeTime : BuildableBase, IEquatable<LifeTime>
    {

        public LifeTime()
        {
        }

        public LifeTime(int? timeToLive = null, TimeUnit timeUnit = ValueTypes.TimeUnit.MILLISECONDS)
        {
            if (timeToLive.HasValue && timeToLive.Value > 0)
            {
                TimeToLive = timeToLive;
                TimeUnit = timeUnit;
                Unlimited = false;
            }
            else
            {
                Unlimited = true;
            }
        }
        
        public TimeUnit? TimeUnit { get; set; }
        public int? TimeToLive { get; set; }
        public bool? Unlimited { get; set; }
        
        #region Equality Members

        /// <inheritdoc />
        public bool Equals(LifeTime other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return TimeToLive == other.TimeToLive && TimeUnit == other.TimeUnit && Unlimited == other.Unlimited;
        }


        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals(( LifeTime ) obj);
        }


        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked {
                var hashCode = TimeToLive.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ TimeUnit.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ Unlimited.GetHashCode();

                return hashCode;
            }
        }


        public static bool operator ==(LifeTime left, LifeTime right) => Equals(left, right);


        public static bool operator !=(LifeTime left, LifeTime right) => !Equals(left, right);

        #endregion
    }
}
