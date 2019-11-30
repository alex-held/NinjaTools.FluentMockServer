using System;

namespace NinjaTools.FluentMockServer.Client.Models.ValueTypes
{
    [Serializable]
    public class LifeTime : IEquatable<LifeTime>
    {
        public LifeTime()
        {
        }

        public LifeTime(int? timeToLive = null, TimeUnit timeUnit = ValueTypes.TimeUnit.Milliseconds)
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

        /// <inheritdoc />
        public bool Equals(LifeTime other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TimeUnit == other.TimeUnit && TimeToLive == other.TimeToLive && Unlimited == other.Unlimited;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LifeTime) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = TimeUnit.GetHashCode();
                hashCode = (hashCode * 397) ^ TimeToLive.GetHashCode();
                hashCode = (hashCode * 397) ^ Unlimited.GetHashCode();
                return hashCode;
            }
        }
    }
}
