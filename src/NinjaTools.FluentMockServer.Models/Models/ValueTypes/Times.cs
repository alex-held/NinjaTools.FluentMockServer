using System;

namespace NinjaTools.FluentMockServer.Client.Models.ValueTypes
{
    [Serializable]
    public class Times : IEquatable<Times>
    {
        public Times()
        {
        }

        public Times(int remainingTimes)
        {
            if (remainingTimes <= 0)
            {
                Unlimited = true;
            }
            else
            {
                RemainingTimes = remainingTimes;
                Unlimited = false;
            }
        }

        public int RemainingTimes { get; set; }

        public bool Unlimited { get; set; }

        public static Times Once => new Times(1);
        public static Times Always => new Times(0);

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
            return Equals((Times) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (RemainingTimes * 397) ^ Unlimited.GetHashCode();
            }
        }
    }
}
