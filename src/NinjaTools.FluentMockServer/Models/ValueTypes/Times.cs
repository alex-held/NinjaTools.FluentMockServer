using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    [Serializable]
    public class Times : ICloneable, IEquatable<Times>
    {
        [JsonConstructor]
        public Times(int remainingTimes)
        {
            RemainingTimes = remainingTimes;
        }

        public int RemainingTimes { get;  }

        public bool Unlimited => RemainingTimes <= 0;

        [NotNull] public static Times Once => new Times(1);
        [NotNull] public static Times Always => new Times(0);

        /// <inheritdoc />
        public bool Equals(Times other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return RemainingTimes == other.RemainingTimes;
        }
        
        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Times) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return RemainingTimes;
        }

        /// <inheritdoc />
        public object Clone()
        {
           return new Times(RemainingTimes);
        }
        
        public static bool operator ==(Times left, Times right)
        {
            // Check for null on left side.
            if (ReferenceEquals(left, null))
            {
                if (ReferenceEquals(right, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            
            // Equals handles case of null on right side.
            return left.Equals(right);
        }

        public static bool operator !=([CanBeNull] Times left, [CanBeNull] Times right)
        {
            return !(left?.Equals(right) ?? false);
        }
    }
}
