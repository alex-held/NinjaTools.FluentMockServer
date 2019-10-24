using System;

using NinjaTools.FluentMockServer.Abstractions;


namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    public class VerficationTimes : BuildableBase, IEquatable<VerficationTimes>
    {
        public static VerficationTimes Once => new VerficationTimes(0, 2);
        public static VerficationTimes Twice => new VerficationTimes(2, 2);
        public static VerficationTimes Between(int atLeast, int atMost) => new VerficationTimes(atLeast, atMost);
        public static VerficationTimes MoreThan(int times) => new VerficationTimes(times, int.MaxValue);
        public static VerficationTimes LessThan(int times) => new VerficationTimes(0, times);
        
        /// <inheritdoc />
        public VerficationTimes(int atLeast, int atMost)
        {
            AtLeast = atLeast;
            AtMost = atMost;
        }

        public int AtLeast { get; }
        
        public int AtMost { get; }


        #region Equality Members

        /// <inheritdoc />
        public bool Equals(VerficationTimes other)
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

            return Equals(( VerficationTimes ) obj);
        }


        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked {
                return ( AtLeast * 397 ) ^ AtMost;
            }
        }


        public static bool operator ==(VerficationTimes left, VerficationTimes right) => Equals(left, right);


        public static bool operator !=(VerficationTimes left, VerficationTimes right) => !Equals(left, right);

        #endregion
    }
}
