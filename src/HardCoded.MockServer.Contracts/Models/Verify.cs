using System;

using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Contracts.Models.HttpEntities;
using HardCoded.MockServer.Contracts.Models.ValueTypes;


namespace HardCoded.MockServer.Contracts.Models
{
    /// <summary>
    /// Model used to describe what to verify.
    /// </summary>
    public class Verify : BuildableBase, IEquatable<Verify>
    {
        /// <summary>
        /// The to be matched <see cref="HttpRequest"/>.
        /// </summary>
        public HttpRequest HttpRequest { get; set; }
       
        /// <summary>
        /// How many <see cref="Times"/> the request is expected to have occured.
        /// </summary>
        public Times Times { get; set; }
        
        
        #region Equality Members

        /// <inheritdoc />
        public bool Equals(Verify other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(HttpRequest, other.HttpRequest) && Equals(Times, other.Times);
        }


        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals(( Verify ) obj);
        }


        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked {
                return ( ( HttpRequest != null ? HttpRequest.GetHashCode() : 0 ) * 397 ) ^ ( Times != null ? Times.GetHashCode() : 0 );
            }
        }


        public static bool operator ==(Verify left, Verify right) => Equals(left, right);


        public static bool operator !=(Verify left, Verify right) => !Equals(left, right);

        #endregion
    }
}
